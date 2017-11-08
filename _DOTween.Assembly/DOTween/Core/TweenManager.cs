// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/07 13:00
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.Collections.Generic;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Core
{
    internal static class TweenManager
    {
        const int _DefaultMaxTweeners = 200;
        const int _DefaultMaxSequences = 50;
        const string _MaxTweensReached = "Max Tweens reached: capacity has automatically been increased from #0 to #1. Use DOTween.SetTweensCapacity to set it manually at startup";

        internal static int maxActive = _DefaultMaxTweeners + _DefaultMaxSequences; // Always equal to maxTweeners + maxSequences
        internal static int maxTweeners = _DefaultMaxTweeners; // Always >= maxSequences
        internal static int maxSequences = _DefaultMaxSequences; // Always <= maxTweeners
        internal static bool hasActiveTweens, hasActiveDefaultTweens, hasActiveLateTweens, hasActiveFixedTweens, hasActiveManualTweens;
        internal static int totActiveTweens, totActiveDefaultTweens, totActiveLateTweens, totActiveFixedTweens, totActiveManualTweens;
        internal static int totActiveTweeners, totActiveSequences;
        internal static int totPooledTweeners, totPooledSequences;
        internal static int totTweeners, totSequences; // Both active and pooled
        internal static bool isUpdateLoop; // TRUE while an update cycle is running (used to treat direct tween Kills differently)

        // Tweens contained in Sequences are not inside the active lists
        // Arrays are organized (max once per update) so that existing elements are next to each other from 0 to (totActiveTweens - 1)
        internal static Tween[] _activeTweens = new Tween[_DefaultMaxTweeners + _DefaultMaxSequences]; // Internal just to allow DOTweenInspector to access it
        static Tween[] _pooledTweeners = new Tween[_DefaultMaxTweeners];
        static readonly Stack<Tween> _PooledSequences = new Stack<Tween>();

        static readonly List<Tween> _KillList = new List<Tween>(_DefaultMaxTweeners + _DefaultMaxSequences);
        static int _maxActiveLookupId = -1; // Highest full ID in _activeTweens
        static bool _requiresActiveReorganization; // True when _activeTweens need to be reorganized to fill empty spaces
        static int _reorganizeFromId = -1; // First null ID from which to reorganize
        static int _minPooledTweenerId = -1; // Lowest PooledTweeners id that is actually full
        static int _maxPooledTweenerId = -1; // Highest PooledTweeners id that is actually full

        // Used to prevent tweens from being re-killed at the end of an update loop if KillAll was called during said loop
        static bool _despawnAllCalledFromUpdateLoopCallback;

#if DEBUG
        static public int updateLoopCount;
#endif

        #region Main

        // Returns a new Tweener, from the pool if there's one available,
        // otherwise by instantiating a new one
        internal static TweenerCore<T1,T2,TPlugOptions> GetTweener<T1,T2,TPlugOptions>()
            where TPlugOptions : struct, IPlugOptions
        {
            TweenerCore<T1,T2,TPlugOptions> t;
            // Search inside pool
            if (totPooledTweeners > 0) {
                Type typeofT1 = typeof(T1);
                Type typeofT2 = typeof(T2);
                Type typeofTPlugOptions = typeof(TPlugOptions);
                for (int i = _maxPooledTweenerId; i > _minPooledTweenerId - 1; --i) {
                    Tween tween = _pooledTweeners[i];
                    if (tween != null && tween.typeofT1 == typeofT1 && tween.typeofT2 == typeofT2 && tween.typeofTPlugOptions == typeofTPlugOptions) {
                        // Pooled Tweener exists: spawn it
                        t = (TweenerCore<T1, T2, TPlugOptions>)tween;
                        AddActiveTween(t);
                        _pooledTweeners[i] = null;
                        if (_maxPooledTweenerId != _minPooledTweenerId) {
                            if (i == _maxPooledTweenerId) _maxPooledTweenerId--;
                            else if (i == _minPooledTweenerId) _minPooledTweenerId++;
                        }
                        totPooledTweeners--;
                        return t;
                    }
                }
                // Not found: remove a tween from the pool in case it's full
                if (totTweeners >= maxTweeners) {
                    _pooledTweeners[_maxPooledTweenerId] = null;
                    _maxPooledTweenerId--;
                    totPooledTweeners--;
                    totTweeners--;
                }
            } else {
                // Increase capacity in case max number of Tweeners has already been reached, then continue
                if (totTweeners >= maxTweeners - 1) {
                    int prevMaxTweeners = maxTweeners;
                    int prevMaxSequences = maxSequences;
                    IncreaseCapacities(CapacityIncreaseMode.TweenersOnly);
                    if (Debugger.logPriority >= 1) Debugger.LogWarning(_MaxTweensReached
                        .Replace("#0", prevMaxTweeners + "/" + prevMaxSequences)
                        .Replace("#1", maxTweeners + "/" + maxSequences)
                    );
                }
            }
            // Not found: create new TweenerController
            t = new TweenerCore<T1,T2,TPlugOptions>();
            totTweeners++;
            AddActiveTween(t);
            return t;
        }

        // Returns a new Sequence, from the pool if there's one available,
        // otherwise by instantiating a new one
        internal static Sequence GetSequence()
        {
            Sequence s;
            if (totPooledSequences > 0) {
                s = (Sequence)_PooledSequences.Pop();
                AddActiveTween(s);
                totPooledSequences--;
                return s;
            }
            // Increase capacity in case max number of Sequences has already been reached, then continue
            if (totSequences >= maxSequences - 1) {
                int prevMaxTweeners = maxTweeners;
                int prevMaxSequences = maxSequences;
                IncreaseCapacities(CapacityIncreaseMode.SequencesOnly);
                if (Debugger.logPriority >= 1) Debugger.LogWarning(_MaxTweensReached
                    .Replace("#0", prevMaxTweeners + "/" + prevMaxSequences)
                        .Replace("#1", maxTweeners + "/" + maxSequences)
                );
            }
            // Not found: create new Sequence
            s = new Sequence();
            totSequences++;
            AddActiveTween(s);
            return s;
        }

        internal static void SetUpdateType(Tween t, UpdateType updateType, bool isIndependentUpdate)
        {
            if (!t.active || t.updateType == updateType) {
                t.updateType = updateType;
                t.isIndependentUpdate = isIndependentUpdate;
                return;
            }
            // Remove previous update type
            if (t.updateType == UpdateType.Normal) {
                totActiveDefaultTweens--;
                hasActiveDefaultTweens = totActiveDefaultTweens > 0;
            } else {
                switch (t.updateType) {
                case UpdateType.Fixed:
                    totActiveFixedTweens--;
                    hasActiveFixedTweens = totActiveFixedTweens > 0;
                    break;
                case UpdateType.Late:
                    totActiveLateTweens--;
                    hasActiveLateTweens = totActiveLateTweens > 0;
                    break;
                default: // Manual
                    totActiveManualTweens--;
                    hasActiveManualTweens = totActiveManualTweens > 0;
                    break;
                }
            }
            // Assign new one
            t.updateType = updateType;
            t.isIndependentUpdate = isIndependentUpdate;
            if (updateType == UpdateType.Normal) {
                totActiveDefaultTweens++;
                hasActiveDefaultTweens = true;
            } else {
                switch (updateType) {
                case UpdateType.Fixed:
                    totActiveFixedTweens++;
                    hasActiveFixedTweens = true;
                    break;
                case UpdateType.Late:
                    totActiveLateTweens++;
                    hasActiveLateTweens = true;
                    break;
                default: // Manual
                    totActiveManualTweens++;
                    hasActiveManualTweens = true;
                    break;
                }
            }
        }

        // Removes the given tween from the active tweens list
        internal static void AddActiveTweenToSequence(Tween t)
        {
            RemoveActiveTween(t);
        }

        // Despawn all
        internal static int DespawnAll()
        {
            int totDespawned = totActiveTweens;
            for (int i = 0; i < _maxActiveLookupId + 1; ++i) {
                Tween t = _activeTweens[i];
                if (t != null) Despawn(t, false);
            }
            ClearTweenArray(_activeTweens);
            hasActiveTweens = hasActiveDefaultTweens = hasActiveLateTweens = hasActiveFixedTweens = hasActiveManualTweens = false;
            totActiveTweens = totActiveDefaultTweens = totActiveLateTweens = totActiveFixedTweens = totActiveManualTweens = 0;
            totActiveTweeners = totActiveSequences = 0;
            _maxActiveLookupId = _reorganizeFromId = -1;
            _requiresActiveReorganization = false;

            if (isUpdateLoop) _despawnAllCalledFromUpdateLoopCallback = true;

            return totDespawned;
        }

        internal static void Despawn(Tween t, bool modifyActiveLists = true)
        {
            // Callbacks
            if (t.onKill != null) Tween.OnTweenCallback(t.onKill);

            if (modifyActiveLists) {
                // Remove tween from active list
                RemoveActiveTween(t);
            }
            if (t.isRecyclable) {
                // Put the tween inside a pool
                switch (t.tweenType) {
                case TweenType.Sequence:
                    _PooledSequences.Push(t);
                    totPooledSequences++;
                    // Despawn sequenced tweens
                    Sequence s = (Sequence)t;
                    int len = s.sequencedTweens.Count;
                    for (int i = 0; i < len; ++i) Despawn(s.sequencedTweens[i], false);
                    break;
                case TweenType.Tweener:
                    if (_maxPooledTweenerId == -1) {
                        _maxPooledTweenerId = maxTweeners - 1;
                        _minPooledTweenerId = maxTweeners - 1;
                    }
                    if (_maxPooledTweenerId < maxTweeners - 1) {
                        _pooledTweeners[_maxPooledTweenerId + 1] = t;
                        _maxPooledTweenerId++;
                        if (_minPooledTweenerId > _maxPooledTweenerId) _minPooledTweenerId = _maxPooledTweenerId;
                    } else {
                        for (int i = _maxPooledTweenerId; i > -1; --i) {
                            if (_pooledTweeners[i] != null) continue;
                            _pooledTweeners[i] = t;
                            if (i < _minPooledTweenerId) _minPooledTweenerId = i;
                            if (_maxPooledTweenerId < _minPooledTweenerId) _maxPooledTweenerId = _minPooledTweenerId;
                            break;
                        }
                    }
                    totPooledTweeners++;
                    break;
                }
            } else {
                // Remove
                switch (t.tweenType) {
                case TweenType.Sequence:
                    totSequences--;
                    // Despawn sequenced tweens
                    Sequence s = (Sequence)t;
                    int len = s.sequencedTweens.Count;
                    for (int i = 0; i < len; ++i) Despawn(s.sequencedTweens[i], false);
                    break;
                case TweenType.Tweener:
                    totTweeners--;
                    break;
                }
            }
            t.active = false;
            t.Reset();
        }

        // Destroys any active tween without putting them back in a pool,
        // then purges all pools and resets capacities
        internal static void PurgeAll()
        {
            // Fire eventual onKill callbacks
            for (int i = 0; i < totActiveTweens; ++i) {
                Tween t = _activeTweens[i];
                if (t != null) {
                    t.active = false;
                    if (t.onKill != null) Tween.OnTweenCallback(t.onKill);
                }
            }

            ClearTweenArray(_activeTweens);
            hasActiveTweens = hasActiveDefaultTweens = hasActiveLateTweens = hasActiveFixedTweens = hasActiveManualTweens = false;
            totActiveTweens = totActiveDefaultTweens = totActiveLateTweens = totActiveFixedTweens = totActiveManualTweens = 0;
            totActiveTweeners = totActiveSequences = 0;
            _maxActiveLookupId = _reorganizeFromId = -1;
            _requiresActiveReorganization = false;
            PurgePools();
            ResetCapacities();
            totTweeners = totSequences = 0;
        }

        // Removes any cached tween from the pools
        internal static void PurgePools()
        {
            totTweeners -= totPooledTweeners;
            totSequences -= totPooledSequences;
            ClearTweenArray(_pooledTweeners);
            _PooledSequences.Clear();
            totPooledTweeners = totPooledSequences = 0;
            _minPooledTweenerId = _maxPooledTweenerId = -1;
        }

        internal static void ResetCapacities()
        {
            SetCapacities(_DefaultMaxTweeners, _DefaultMaxSequences);
        }

        internal static void SetCapacities(int tweenersCapacity, int sequencesCapacity)
        {
            if (tweenersCapacity < sequencesCapacity) tweenersCapacity = sequencesCapacity;

//            maxActive = tweenersCapacity;
            maxActive = tweenersCapacity + sequencesCapacity;
            maxTweeners = tweenersCapacity;
            maxSequences = sequencesCapacity;
            Array.Resize(ref _activeTweens, maxActive);
            Array.Resize(ref _pooledTweeners, tweenersCapacity);
            _KillList.Capacity = maxActive;
        }

        // Looks through all active tweens and removes the ones whose getters generate errors
        // (usually meaning their target has become NULL).
        // Returns the total number of invalid tweens found and removed
        // BEWARE: this is an expensive operation
        internal static int Validate()
        {
            if (_requiresActiveReorganization) ReorganizeActiveTweens();

            int totInvalid = 0;
            for (int i = 0; i < _maxActiveLookupId + 1; ++i) {
                Tween t = _activeTweens[i];
                if (!t.Validate()) {
                    totInvalid++;
                    MarkForKilling(t);
                }
            }
            // Kill all eventually marked tweens
            if (totInvalid > 0) {
                DespawnActiveTweens(_KillList);
                _KillList.Clear();
            }
            return totInvalid;
        }

        // deltaTime will be passed as fixedDeltaTime in case of UpdateType.Fixed
        internal static void Update(UpdateType updateType, float deltaTime, float independentTime)
        {
            if (_requiresActiveReorganization) ReorganizeActiveTweens();

            isUpdateLoop = true;
#if DEBUG
            updateLoopCount++;
            VerifyActiveTweensList();
#endif
            bool willKill = false;
//            Debug.Log("::::::::::: " + updateType + " > " + (_maxActiveLookupId + 1));
            int len = _maxActiveLookupId + 1; // Stored here so if _maxActiveLookupId changed during update loop (like if new tween is created at onComplete) new tweens are still ignored
            for (int i = 0; i < len; ++i) {
                Tween t = _activeTweens[i];
                if (t == null || t.updateType != updateType) continue; // Wrong updateType or was added to a Sequence (thus removed from active list) while inside current updateLoop
                if (!t.active) {
                    // Manually killed by another tween's callback
                    willKill = true;
                    MarkForKilling(t);
                    continue;
                }
                if (!t.isPlaying) continue;
                t.creationLocked = true; // Lock tween creation methods from now on
                float tDeltaTime = (t.isIndependentUpdate ? independentTime : deltaTime) * t.timeScale;
                if (tDeltaTime <= 0) continue; // Skip update in case time is 0
                if (!t.delayComplete) {
                    tDeltaTime = t.UpdateDelay(t.elapsedDelay + tDeltaTime);
                    if (tDeltaTime <= -1) {
                        // Error during startup (can happen with FROM tweens): mark tween for killing
                        willKill = true;
                        MarkForKilling(t);
                        continue;
                    }
                    if (tDeltaTime <= 0) continue;
                    // Delay elapsed - call OnPlay if required
                    if (t.playedOnce && t.onPlay != null) {
                        // Don't call in case it hasn't started because onStart routine will call it
                        Tween.OnTweenCallback(t.onPlay);
                    }
                }
                // Startup (needs to be here other than in Tween.DoGoto in case of speed-based tweens, to calculate duration correctly)
                if (!t.startupDone) {
                    if (!t.Startup()) {
                        // Startup failure: mark for killing
                        willKill = true;
                        MarkForKilling(t);
                        continue;
                    }
                }
                // Find update data
                float toPosition = t.position;
                bool wasEndPosition = toPosition >= t.duration;
                int toCompletedLoops = t.completedLoops;
                if (t.duration <= 0) {
                    toPosition = 0;
                    toCompletedLoops = t.loops == -1 ? t.completedLoops + 1 : t.loops;
                } else {
                    if (t.isBackwards) {
                        toPosition -= tDeltaTime;
                        while (toPosition < 0 && toCompletedLoops > -1) {
                            toPosition += t.duration;
                            toCompletedLoops--;
                        }
                        if (toCompletedLoops < 0 || wasEndPosition && toCompletedLoops < 1) {
                            // Result is equivalent to a rewind, so set values according to it
                            toPosition = 0;
                            toCompletedLoops = wasEndPosition ? 1 : 0;
                        }
//                        while (toPosition < 0 && toCompletedLoops > 0) {
//                            toPosition += t.duration;
//                            toCompletedLoops--;
//                        }
//                        if (wasEndPosition && toCompletedLoops <= 0) {
//                            // Force-rewind
//                            Rewind(t, false);
//                            continue;
//                        }
                    } else {
                        toPosition += tDeltaTime;
                        while (toPosition >= t.duration && (t.loops == -1 || toCompletedLoops < t.loops)) {
                            toPosition -= t.duration;
                            toCompletedLoops++;
                        }
                    }
                    if (wasEndPosition) toCompletedLoops--;
                    if (t.loops != -1 && toCompletedLoops >= t.loops) toPosition = t.duration;
                }
                // Goto
                bool needsKilling = Tween.DoGoto(t, toPosition, toCompletedLoops, UpdateMode.Update);
                if (needsKilling) {
                    willKill = true;
                    MarkForKilling(t);
                }
            }
            // Kill all eventually marked tweens
            if (willKill) {
                if (_despawnAllCalledFromUpdateLoopCallback) {
                    // Do not despawn tweens again, since Kill/DespawnAll was already called
                    _despawnAllCalledFromUpdateLoopCallback = false;
                } else {
                    DespawnActiveTweens(_KillList);
                }
                _KillList.Clear();
            }
            isUpdateLoop = false;
        }

        internal static int FilteredOperation(OperationType operationType, FilterType filterType, object id, bool optionalBool, float optionalFloat, object optionalObj = null, object[] optionalArray = null)
        {
            int totInvolved = 0;
            bool hasDespawned = false;
            int optionalArrayLen = optionalArray == null ? 0 : optionalArray.Length;
            // Determine if ID is required, and if it's stringId
            bool useStringId = false;
            string stringId = null;
            bool useIntId = false;
            int intId = 0;
            switch (filterType) {
            case FilterType.TargetOrId:
            case FilterType.TargetAndId:
                if (id is string) {
                    useStringId = true;
                    stringId = (string)id;
                } else if (id is int) {
                    useIntId = true;
                    intId = (int)id;
                }
                break;
            }
            for (int i = _maxActiveLookupId; i > -1; --i) {
                Tween t = _activeTweens[i];
                if (t == null || !t.active) continue;

                bool isFilterCompliant = false;
                switch (filterType) {
                case FilterType.All:
                    isFilterCompliant = true;
                    break;
                case FilterType.TargetOrId:
                    if (useStringId) isFilterCompliant = t.stringId != null && t.stringId == stringId;
                    else if (useIntId) isFilterCompliant = t.intId == intId;
                    else isFilterCompliant = t.id != null && id.Equals(t.id) || t.target != null && id.Equals(t.target);
                    break;
                case FilterType.TargetAndId:
                    if (useStringId) isFilterCompliant = t.target != null && t.stringId == stringId && optionalObj != null && optionalObj.Equals(t.target);
                    else if (useIntId) isFilterCompliant = t.target != null && t.intId == intId && optionalObj != null && optionalObj.Equals(t.target);
                    else isFilterCompliant = t.id != null && t.target != null && optionalObj != null && id.Equals(t.id) && optionalObj.Equals(t.target);
                    break;
                case FilterType.AllExceptTargetsOrIds:
                    isFilterCompliant = true;
                    for (int c = 0; c < optionalArrayLen; ++c) {
                        object objId = optionalArray[c];
                        if (useStringId && t.stringId == stringId) {
                            isFilterCompliant = false;
                            break;
                        } else if (useIntId && t.intId == intId) {
                            isFilterCompliant = false;
                            break;
                        } else if (t.id != null && objId.Equals(t.id) || t.target != null && objId.Equals(t.target)) {
                            isFilterCompliant = false;
                            break;
                        }
                    }
                    break;
                }
                if (isFilterCompliant) {
                    switch (operationType) {
                    case OperationType.Despawn:
                        totInvolved++;
                        t.active = false; // Mark it as inactive immediately, so eventual kills called inside a kill won't have effect
//                        if (isUpdateLoop) t.active = false; // Just mark it for killing, so the update loop will take care of it
                        if (isUpdateLoop) break; // Just mark it for killing, the update loop will take care of the rest
                        Despawn(t, false);
                        hasDespawned = true;
                        _KillList.Add(t);
                        break;
                    case OperationType.Complete:
                        bool hasAutoKill = t.autoKill;
                        // If optionalFloat is > 0 completes with callbacks
                        if (Complete(t, false, optionalFloat > 0 ? UpdateMode.Update : UpdateMode.Goto)) {
                            // If optionalBool is TRUE only returns tweens killed by completion
                            totInvolved += !optionalBool ? 1 : hasAutoKill ? 1 : 0;
                            if (hasAutoKill) {
                                if (isUpdateLoop) t.active = false; // Just mark it for killing, so the update loop will take care of it
                                else {
                                    hasDespawned = true;
                                    _KillList.Add(t);
                                }
                            }
                        }
                        break;
                    case OperationType.Flip:
                        if (Flip(t)) totInvolved++;
                        break;
                    case OperationType.Goto:
                        Goto(t, optionalFloat, optionalBool);
                        totInvolved++;
                        break;
                    case OperationType.Pause:
                        if (Pause(t)) totInvolved++;
                        break;
                    case OperationType.Play:
                        if (Play(t)) totInvolved++;
                        break;
                    case OperationType.PlayBackwards:
                        if (PlayBackwards(t)) totInvolved++;
                        break;
                    case OperationType.PlayForward:
                        if (PlayForward(t)) totInvolved++;
                        break;
                    case OperationType.Restart:
                        if (Restart(t, optionalBool, optionalFloat)) totInvolved++;
                        break;
                    case OperationType.Rewind:
                        if (Rewind(t, optionalBool)) totInvolved++;
                        break;
                    case OperationType.SmoothRewind:
                        if (SmoothRewind(t)) totInvolved++;
                        break;
                    case OperationType.TogglePause:
                        if (TogglePause(t)) totInvolved++;
                        break;
                    case OperationType.IsTweening:
                        if ((!t.isComplete || !t.autoKill) && (!optionalBool || t.isPlaying)) totInvolved++;
                        break;
                    }
                }
            }
            // Special additional operations in case of despawn
            if (hasDespawned) {
                int count = _KillList.Count - 1;
                for (int i = count; i > -1; --i) {
                    Tween t = _KillList[i];
                    // Ignore tweens with activeId -1, since they were already killed and removed
                    //  by nested OnComplete callbacks
                    if (t.activeId != -1) RemoveActiveTween(t);
                }
                _KillList.Clear();
            }

            return totInvolved;
        }

        #endregion

        #region Play Operations

        internal static bool Complete(Tween t, bool modifyActiveLists = true, UpdateMode updateMode = UpdateMode.Goto)
        {
            if (t.loops == -1) return false;
            if (!t.isComplete) {
                Tween.DoGoto(t, t.duration, t.loops, updateMode);
                t.isPlaying = false;
                // Despawn if needed
                if (t.autoKill) {
                    if (isUpdateLoop) t.active = false; // Just mark it for killing, so the update loop will take care of it
                    else Despawn(t, modifyActiveLists);
                }
                return true;
            }
            return false;
        }

        internal static bool Flip(Tween t)
        {
            t.isBackwards = !t.isBackwards;
            return true;
        }

        // Forces the tween to startup and initialize all its data
        internal static void ForceInit(Tween t, bool isSequenced = false)
        {
            if (t.startupDone) return;

            if (!t.Startup() && !isSequenced) {
                // Startup failed: kill tween
                if (isUpdateLoop) t.active = false; // Just mark it for killing, so the update loop will take care of it
                else RemoveActiveTween(t);
            }
        }

        // Returns TRUE if there was an error and the tween needs to be destroyed
        internal static bool Goto(Tween t, float to, bool andPlay = false, UpdateMode updateMode = UpdateMode.Goto)
        {
            bool wasPlaying = t.isPlaying;
            t.isPlaying = andPlay;
            t.delayComplete = true;
            t.elapsedDelay = t.delay;
//            int toCompletedLoops = (int)(to / t.duration); // With very small floats creates floating points imprecisions
            int toCompletedLoops = Mathf.FloorToInt(to / t.duration); // Still generates imprecision with some values (like 0.4)
//            int toCompletedLoops = (int)((decimal)to / (decimal)t.duration); // Takes care of floating points imprecision (nahh doesn't work correctly either)
            float toPosition = to % t.duration;
            if (t.loops != -1 && toCompletedLoops >= t.loops) {
                toCompletedLoops = t.loops;
                toPosition = t.duration;
            } else if (toPosition >= t.duration) toPosition = 0;
            // If andPlay is FALSE manage onPause from here because DoGoto won't detect it (since t.isPlaying was already set from here)
            bool needsKilling = Tween.DoGoto(t, toPosition, toCompletedLoops, updateMode);
            if (!andPlay && wasPlaying && !needsKilling && t.onPause != null) Tween.OnTweenCallback(t.onPause);
            return needsKilling;
        }

        // Returns TRUE if the given tween was not already paused
        internal static bool Pause(Tween t)
        {
            if (t.isPlaying) {
                t.isPlaying = false;
                if (t.onPause != null) Tween.OnTweenCallback(t.onPause);
                return true;
            }
            return false;
        }

        // Returns TRUE if the given tween was not already playing and is not complete
        internal static bool Play(Tween t)
        {
            if (!t.isPlaying && (!t.isBackwards && !t.isComplete || t.isBackwards && (t.completedLoops > 0 || t.position > 0))) {
                t.isPlaying = true;
                if (t.playedOnce && t.delayComplete && t.onPlay != null) {
                    // Don't call in case there's a delay to run or if it hasn't started because onStart routine will call it
                    Tween.OnTweenCallback(t.onPlay);
                }
                return true;
            }
            return false;
        }

        internal static bool PlayBackwards(Tween t)
        {
            if (t.completedLoops == 0 && t.position <= 0) {
                // Already rewinded, manage OnRewind callback
                ManageOnRewindCallbackWhenAlreadyRewinded(t, true);
            }
            if (!t.isBackwards) {
                t.isBackwards = true;
                Play(t);
                return true;
            }
            return Play(t);
        }

        internal static bool PlayForward(Tween t)
        {
            if (t.isBackwards) {
                t.isBackwards = false;
                Play(t);
                return true;
            }
            return Play(t);
        }

        internal static bool Restart(Tween t, bool includeDelay = true, float changeDelayTo = -1)
        {
            bool wasPaused = !t.isPlaying;
            t.isBackwards = false;
            if (changeDelayTo >= 0) t.delay = changeDelayTo;
            Rewind(t, includeDelay);
            t.isPlaying = true;
            if (wasPaused && t.playedOnce && t.delayComplete && t.onPlay != null) {
                // Don't call in case there's a delay to run or if it hasn't started because onStart routine will call it
                Tween.OnTweenCallback(t.onPlay);
            }
            return true;
        }

        internal static bool Rewind(Tween t, bool includeDelay = true)
        {
            bool wasPlaying = t.isPlaying; // Manage onPause from this method because DoGoto won't detect it
            t.isPlaying = false;
            bool rewinded = false;
            if (t.delay > 0) {
                if (includeDelay) {
                    rewinded = t.delay > 0 && t.elapsedDelay > 0;
                    t.elapsedDelay = 0;
                    t.delayComplete = false;
                } else {
                    rewinded = t.elapsedDelay < t.delay;
                    t.elapsedDelay = t.delay;
                    t.delayComplete = true;
                }
            }
            if (t.position > 0 || t.completedLoops > 0 || !t.startupDone) {
                rewinded = true;
                bool needsKilling = Tween.DoGoto(t, 0, 0, UpdateMode.Goto);
                if (!needsKilling && wasPlaying && t.onPause != null) Tween.OnTweenCallback(t.onPause);
            } else {
                // Alread rewinded
                ManageOnRewindCallbackWhenAlreadyRewinded(t, false);
            }
            return rewinded;
        }

        internal static bool SmoothRewind(Tween t)
        {
            bool rewinded = false;
            if (t.delay > 0) {
                rewinded = t.elapsedDelay < t.delay;
                t.elapsedDelay = t.delay;
                t.delayComplete = true;
            }
            if (t.position > 0 || t.completedLoops > 0 || !t.startupDone) {
                rewinded = true;
                if (t.loopType == LoopType.Incremental) t.PlayBackwards();
                else {
                    t.Goto(t.ElapsedDirectionalPercentage() * t.duration);
                    t.PlayBackwards();
                }
            } else {
                // Already rewinded
                t.isPlaying = false;
                ManageOnRewindCallbackWhenAlreadyRewinded(t, true);
            }
            return rewinded;
        }

        internal static bool TogglePause(Tween t)
        {
            if (t.isPlaying) return Pause(t);
            return Play(t);
        }

        #endregion

        #region Info Getters

        internal static int TotalPooledTweens()
        {
            return totPooledTweeners + totPooledSequences;
        }

        internal static int TotalPlayingTweens()
        {
            if (!hasActiveTweens) return 0;

            if (_requiresActiveReorganization) ReorganizeActiveTweens();

            int tot = 0;
            for (int i = 0; i < _maxActiveLookupId + 1; ++i) {
                Tween t = _activeTweens[i];
                if (t != null && t.isPlaying) tot++;
            }
            return tot;
        }

        // If playing is FALSE returns active paused tweens, otherwise active playing tweens
        internal static List<Tween> GetActiveTweens(bool playing, List<Tween> fillableList = null)
        {
            if (_requiresActiveReorganization) ReorganizeActiveTweens();

            if (totActiveTweens <= 0) return null;
            int len = totActiveTweens;
            if (fillableList == null) fillableList = new List<Tween>(len);
            for (int i = 0; i < len; ++i) {
                Tween t = _activeTweens[i];
                if (t.isPlaying == playing) fillableList.Add(t);
            }
            if (fillableList.Count > 0) return fillableList;
            return null;
        }

        // Returns all active tweens with the given id
        internal static List<Tween> GetTweensById(object id, bool playingOnly, List<Tween> fillableList = null)
        {
            if (_requiresActiveReorganization) ReorganizeActiveTweens();

            if (totActiveTweens <= 0) return null;
            int len = totActiveTweens;
            if (fillableList == null) fillableList = new List<Tween>(len);
            for (int i = 0; i < len; ++i) {
                Tween t = _activeTweens[i];
                if (t == null || !Equals(id, t.id)) continue;
                if (!playingOnly || t.isPlaying) fillableList.Add(t);
            }
            if (fillableList.Count > 0) return fillableList;
            return null;
        }

        // Returns all active tweens with the given target
        internal static List<Tween> GetTweensByTarget(object target, bool playingOnly, List<Tween> fillableList = null)
        {
            if (_requiresActiveReorganization) ReorganizeActiveTweens();

            if (totActiveTweens <= 0) return null;
            int len = totActiveTweens;
            if (fillableList == null) fillableList = new List<Tween>(len);
            for (int i = 0; i < len; ++i) {
                Tween t = _activeTweens[i];
                if (t.target != target) continue;
                if (!playingOnly || t.isPlaying) fillableList.Add(t);
            }
            if (fillableList.Count > 0) return fillableList;
            return null;
        }

        #endregion

        #region Private Methods

        static void MarkForKilling(Tween t)
        {
            t.active = false;
            _KillList.Add(t);
        }

        // Adds the given tween to the active tweens list (updateType is always Normal, but can be changed by SetUpdateType)
        static void AddActiveTween(Tween t)
        {
            if (_requiresActiveReorganization) ReorganizeActiveTweens();

            t.active = true;
            t.updateType = DOTween.defaultUpdateType;
            t.isIndependentUpdate = DOTween.defaultTimeScaleIndependent;
            t.activeId = _maxActiveLookupId = totActiveTweens;
            _activeTweens[totActiveTweens] = t;
            if (t.updateType == UpdateType.Normal) {
                totActiveDefaultTweens++;
                hasActiveDefaultTweens = true;
            } else {
                switch (t.updateType) {
                case UpdateType.Fixed:
                    totActiveFixedTweens++;
                    hasActiveFixedTweens = true;
                    break;
                case UpdateType.Late:
                    totActiveLateTweens++;
                    hasActiveLateTweens = true;
                    break;
                default:
                    totActiveManualTweens++;
                    hasActiveManualTweens = true;
                    break;
                }
            }

            totActiveTweens++;
            if (t.tweenType == TweenType.Tweener) totActiveTweeners++;
            else totActiveSequences++;
            hasActiveTweens = true;
        }

        static void ReorganizeActiveTweens()
        {
            if (totActiveTweens <= 0) {
                _maxActiveLookupId = -1;
                _requiresActiveReorganization = false;
                _reorganizeFromId = -1;
                return;
            } else if (_reorganizeFromId == _maxActiveLookupId) {
                _maxActiveLookupId--;
                _requiresActiveReorganization = false;
                _reorganizeFromId = -1;
                return;
            }

            int shift = 1;
            int len = _maxActiveLookupId + 1;
            _maxActiveLookupId = _reorganizeFromId - 1;
            for (int i = _reorganizeFromId + 1; i < len; ++i) {
                Tween t = _activeTweens[i];
                if (t == null) {
                    shift++;
                    continue;
                }
                t.activeId = _maxActiveLookupId = i - shift;
                _activeTweens[i - shift] = t;
                _activeTweens[i] = null;
            }
            _requiresActiveReorganization = false;
            _reorganizeFromId = -1;
        }

        static void DespawnActiveTweens(List<Tween> tweens)
        {
            int count = tweens.Count - 1;
            for (int i = count; i > -1; --i) Despawn(tweens[i]);
        }

        // Removes a tween from the active list, reorganizes said list
        // and decreases the given total
        static void RemoveActiveTween(Tween t)
        {
            int index = t.activeId;

            t.activeId = -1;
            _requiresActiveReorganization = true;
            if (_reorganizeFromId == -1 || _reorganizeFromId > index) _reorganizeFromId = index;
            _activeTweens[index] = null;

            if (t.updateType == UpdateType.Normal) {
                if (totActiveDefaultTweens > 0) {
                    totActiveDefaultTweens--;
                    hasActiveDefaultTweens = totActiveDefaultTweens > 0;
                } else {
                    Debugger.LogRemoveActiveTweenError("totActiveDefaultTweens");
                }
            } else {
                switch (t.updateType) {
                case UpdateType.Fixed:
                    if (totActiveFixedTweens > 0) {
                        totActiveFixedTweens--;
                        hasActiveFixedTweens = totActiveFixedTweens > 0;
                    } else {
                        Debugger.LogRemoveActiveTweenError("totActiveFixedTweens");
                    }
                    break;
                case UpdateType.Late:
                    if (totActiveLateTweens > 0) {
                        totActiveLateTweens--;
                        hasActiveLateTweens = totActiveLateTweens > 0;
                    } else {
                        Debugger.LogRemoveActiveTweenError("totActiveLateTweens");
                    }
                    break;
                default:
                    if (totActiveManualTweens > 0) {
                        totActiveManualTweens--;
                        hasActiveManualTweens = totActiveManualTweens > 0;
                    } else {
                        Debugger.LogRemoveActiveTweenError("totActiveManualTweens");
                    }
                    break;
                }
            }
            totActiveTweens--;
            hasActiveTweens = totActiveTweens > 0;
            if (t.tweenType == TweenType.Tweener) totActiveTweeners--;
            else totActiveSequences--;
            if (totActiveTweens < 0) {
                totActiveTweens = 0;
                Debugger.LogRemoveActiveTweenError("totActiveTweens");
            }
            if (totActiveTweeners < 0) {
                totActiveTweeners = 0;
                Debugger.LogRemoveActiveTweenError("totActiveTweeners");
            }
            if (totActiveSequences < 0) {
                totActiveSequences = 0;
                Debugger.LogRemoveActiveTweenError("totActiveSequences");
            }
        }

        static void ClearTweenArray(Tween[] tweens)
        {
            int len = tweens.Length;
            for (int i = 0; i < len; i++) tweens[i] = null;
        }

        static void IncreaseCapacities(CapacityIncreaseMode increaseMode)
        {
            int killAdd = 0;
//            int increaseTweenersBy = _DefaultMaxTweeners;
//            int increaseSequencesBy = _DefaultMaxSequences;
            int increaseTweenersBy = Mathf.Max((int)(maxTweeners * 1.5f), _DefaultMaxTweeners);
            int increaseSequencesBy = Mathf.Max((int)(maxSequences * 1.5f), _DefaultMaxSequences);
            switch (increaseMode) {
            case CapacityIncreaseMode.TweenersOnly:
                killAdd += increaseTweenersBy;
                maxTweeners += increaseTweenersBy;
                Array.Resize(ref _pooledTweeners, maxTweeners);
                break;
            case CapacityIncreaseMode.SequencesOnly:
                killAdd += increaseSequencesBy;
                maxSequences += increaseSequencesBy;
                break;
            default:
                killAdd += increaseTweenersBy;
                maxTweeners += increaseTweenersBy;
                maxSequences += increaseSequencesBy;
                Array.Resize(ref _pooledTweeners, maxTweeners);
                break;
            }
//            maxActive = Mathf.Max(maxTweeners, maxSequences);
            maxActive = maxTweeners + maxSequences;
            Array.Resize(ref _activeTweens, maxActive);
            if (killAdd > 0) _KillList.Capacity += killAdd;
        }

        #region Helpers

        // If isPlayBackwardsOrSmoothRewind is FALSE, it means this was a Rewind command
        static void ManageOnRewindCallbackWhenAlreadyRewinded(Tween t, bool isPlayBackwardsOrSmoothRewind)
        {
            if (t.onRewind == null) return;
            if (isPlayBackwardsOrSmoothRewind) {
                // PlayBackwards or SmoothRewind
                if (DOTween.rewindCallbackMode == RewindCallbackMode.FireAlways) t.onRewind();
            } else {
                // Rewind
                if (DOTween.rewindCallbackMode != RewindCallbackMode.FireIfPositionChanged) t.onRewind();
            }
        }

        #endregion

        #endregion

        #region Debug Methods
#if DEBUG
        static void VerifyActiveTweensList()
        {
            int nullTweensWithinLookup = 0, inactiveTweensWithinLookup = 0, activeTweensAfterNull = 0;
            List<int> activeTweensAfterNullIds = new List<int>();
            
            for (int i = 0; i < _maxActiveLookupId + 1; ++i) {
                Tween t = _activeTweens[i];
                if (t == null) nullTweensWithinLookup++;
                else if (!t.active) inactiveTweensWithinLookup++;
            }
            int len = _activeTweens.Length;
            int firstNullIndex = -1;
            for (int i = 0; i < len; ++i) {
                if (firstNullIndex == -1 && _activeTweens[i] == null) firstNullIndex = i;
                else if (firstNullIndex > -1 && _activeTweens[i] != null) {
                    activeTweensAfterNull++;
                    activeTweensAfterNullIds.Add(i);
                }
            }

            if (nullTweensWithinLookup > 0 || inactiveTweensWithinLookup > 0 || activeTweensAfterNull > 0) {
                string s = "VerifyActiveTweensList WARNING:";
                if (isUpdateLoop) s += " - UPDATE LOOP (" + updateLoopCount + ")";
                if (nullTweensWithinLookup > 0) s += " - NULL Tweens Within Lookup (" + nullTweensWithinLookup + ")";
                if (inactiveTweensWithinLookup > 0) s += " - Inactive Tweens Within Lookup (" + inactiveTweensWithinLookup + ")";
                if (activeTweensAfterNull > 0) {
                    string indexes = "";
                    len = activeTweensAfterNullIds.Count;
                    for (int i = 0; i < len; ++i) {
                        if (i > 0) indexes += ",";
                        indexes += activeTweensAfterNullIds[i];
                    }
                    s += " - Active tweens after NULL ones (" + (firstNullIndex - 1) + "/" + activeTweensAfterNull + "[" + indexes + "]" + ")";
                }
                Debug.LogWarning(s);
            }
        }
#endif
        #endregion

        #region Internal Classes

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // ||| INTERNAL CLASSES ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        internal enum CapacityIncreaseMode
        {
            TweenersAndSequences,
            TweenersOnly,
            SequencesOnly
        }

        #endregion
    }
}