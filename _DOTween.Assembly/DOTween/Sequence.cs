// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/15 17:50
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Controls other tweens as a group
    /// </summary>
    public sealed class Sequence : Tween
    {
        // SETUP DATA ////////////////////////////////////////////////

        internal readonly List<Tween> sequencedTweens = new List<Tween>(); // Only Tweens (used for despawning and validation)
        readonly List<ABSSequentiable> _sequencedObjs = new List<ABSSequentiable>(); // Tweens plus SequenceCallbacks
        internal float lastTweenInsertTime; // Used to insert a tween at the position of the previous one

        #region Constructor

        internal Sequence()
        {
            tweenType = TweenType.Sequence;
            Reset();
        }

        #endregion

        #region Creation Methods

        internal static Sequence DoPrepend(Sequence inSequence, Tween t)
        {
            if (t.loops == -1) t.loops = 1;
            float tFullTime = t.delay + (t.duration * t.loops);
//            float tFullTime = t.duration * t.loops;
            inSequence.duration += tFullTime;
            int len = inSequence._sequencedObjs.Count;
            for (int i = 0; i < len; ++i) {
                ABSSequentiable sequentiable = inSequence._sequencedObjs[i];
                sequentiable.sequencedPosition += tFullTime;
                sequentiable.sequencedEndPosition += tFullTime;
            }

            return DoInsert(inSequence, t, 0);
        }

        internal static Sequence DoInsert(Sequence inSequence, Tween t, float atPosition)
        {
            TweenManager.AddActiveTweenToSequence(t);

            // If t has a delay add it as an interval
            atPosition += t.delay;
            inSequence.lastTweenInsertTime = atPosition;

            t.isSequenced = t.creationLocked = true;
            t.sequenceParent = inSequence;
            if (t.loops == -1) t.loops = 1;
            float tFullTime = t.duration * t.loops;
            t.autoKill = false;
            t.delay = t.elapsedDelay = 0;
            t.delayComplete = true;
            t.isSpeedBased = false;
            t.sequencedPosition = atPosition;
            t.sequencedEndPosition = atPosition + tFullTime;

            if (t.sequencedEndPosition > inSequence.duration) inSequence.duration = t.sequencedEndPosition;
            inSequence._sequencedObjs.Add(t);
            inSequence.sequencedTweens.Add(t);

            return inSequence;
        }

        internal static Sequence DoAppendInterval(Sequence inSequence, float interval)
        {
            inSequence.lastTweenInsertTime = inSequence.duration;
            inSequence.duration += interval;
            return inSequence;
        }

        internal static Sequence DoPrependInterval(Sequence inSequence, float interval)
        {
            inSequence.lastTweenInsertTime = 0;
            inSequence.duration += interval;
            int len = inSequence._sequencedObjs.Count;
            for (int i = 0; i < len; ++i) {
                ABSSequentiable sequentiable = inSequence._sequencedObjs[i];
                sequentiable.sequencedPosition += interval;
                sequentiable.sequencedEndPosition += interval;
            }

            return inSequence;
        }

        internal static Sequence DoInsertCallback(Sequence inSequence, TweenCallback callback, float atPosition)
        {
            inSequence.lastTweenInsertTime = atPosition;
            SequenceCallback c = new SequenceCallback(atPosition, callback);
            c.sequencedPosition = c.sequencedEndPosition = atPosition;
            inSequence._sequencedObjs.Add(c);
            if (inSequence.duration < atPosition) inSequence.duration = atPosition;
            return inSequence;
        }

        #endregion

        internal override void Reset()
        {
            base.Reset();

            sequencedTweens.Clear();
            _sequencedObjs.Clear();
            lastTweenInsertTime = 0;
        }

        // Called by TweenManager.Validate.
        // Returns TRUE if the tween is valid
        internal override bool Validate()
        {
            int len = sequencedTweens.Count;
            for (int i = 0; i < len; i++) {
                if (!sequencedTweens[i].Validate()) return false;
            }
            return true;
        }

        // CALLED BY Tween the moment the tween starts.
        // Returns TRUE in case of success
        internal override bool Startup()
        {
            return DoStartup(this);
        }

        internal override bool ApplyTween(float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode, UpdateNotice updateNotice)
        {
            return DoApplyTween(this, prevPosition, prevCompletedLoops, newCompletedSteps, useInversePosition, updateMode);
        }

        // Called by DOTween when spawning/creating a new Sequence.
        internal static void Setup(Sequence s)
        {
            s.autoKill = DOTween.defaultAutoKill;
            s.isRecyclable = DOTween.defaultRecyclable;
            s.isPlaying = DOTween.defaultAutoPlay == AutoPlay.All || DOTween.defaultAutoPlay == AutoPlay.AutoPlaySequences;
            s.loopType = DOTween.defaultLoopType;
            s.easeType = Ease.Linear;
            s.easeOvershootOrAmplitude = DOTween.defaultEaseOvershootOrAmplitude;
            s.easePeriod = DOTween.defaultEasePeriod;
        }

        // Returns TRUE in case of success
        internal static bool DoStartup(Sequence s)
        {
            if (s.sequencedTweens.Count == 0 && s._sequencedObjs.Count == 0
                && s.onComplete == null && s.onKill == null && s.onPause == null && s.onPlay == null && s.onRewind == null
                && s.onStart == null && s.onStepComplete == null && s.onUpdate == null
            ) return false; // Empty Sequence without any callback set

            s.startupDone = true;
            s.fullDuration = s.loops > -1 ? s.duration * s.loops : Mathf.Infinity;
            // Order sequencedObjs by start position
            s._sequencedObjs.Sort(SortSequencedObjs);
            // Set relative nested tweens
            if (s.isRelative) {
                for (int len = s.sequencedTweens.Count, i = 0; i < len; ++i) {
                    Tween t = s.sequencedTweens[i];
                    if (!s.isBlendable) s.sequencedTweens[i].isRelative = true;
                }
            }
            return true;
        }

        // Applies the tween set by DoGoto.
        // Returns TRUE if the tween needs to be killed
        internal static bool DoApplyTween(Sequence s, float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode)
        {
            // Adapt to eventual ease position
            float prevPos = prevPosition;
            float newPos = s.position;
            if (s.easeType != Ease.Linear) {
                prevPos = s.duration * EaseManager.Evaluate(s.easeType, s.customEase, prevPos, s.duration, s.easeOvershootOrAmplitude, s.easePeriod);
                newPos = s.duration * EaseManager.Evaluate(s.easeType, s.customEase, newPos, s.duration, s.easeOvershootOrAmplitude, s.easePeriod);
            }


            float from, to = 0;
            // Determine if prevPos was inverse.
            // Used to calculate correct "from" value when applying internal cycle
            // and also in case of multiple loops within a single update
            bool prevPosIsInverse = s.loopType == LoopType.Yoyo
                && (prevPos < s.duration ? prevCompletedLoops % 2 != 0 : prevCompletedLoops % 2 == 0);
            if (s.isBackwards) prevPosIsInverse = !prevPosIsInverse;
            // Update multiple loop cycles within the same update
            if (newCompletedSteps > 0) {
//                Debug.Log(Time.frameCount + " <color=#FFEC03>newCompletedSteps = " + newCompletedSteps + "</color> - completedLoops: " + s.completedLoops + " - updateMode: " + updateMode);
                // Store expected completedLoops and position, in order to check them after the update cycles.
                int expectedCompletedLoops = s.completedLoops;
                float expectedPosition = s.position;
                //
                int cycles = newCompletedSteps;
                int cyclesDone = 0;
                from = prevPos;
                if (updateMode == UpdateMode.Update) {
                    // Run all cycles elapsed since last update
                    while (cyclesDone < cycles) {
                        if (cyclesDone > 0) from = to;
                        else if (prevPosIsInverse && !s.isBackwards) from = s.duration - from;
                        to = prevPosIsInverse ? 0 : s.duration;
                        if (ApplyInternalCycle(s, from, to, updateMode, useInversePosition, prevPosIsInverse, true)) return true;
                        cyclesDone++;
                        if (s.loopType == LoopType.Yoyo) prevPosIsInverse = !prevPosIsInverse;
                    }
                    // If completedLoops or position were changed by some callback, exit here
//                    Debug.Log("     Internal Cycle Ended > expecteCompletedLoops/completedLoops: " + expectedCompletedLoops + "/" + s.completedLoops + " - expectedPosition/position: " + expectedPosition + "/" + s.position);
                    if (expectedCompletedLoops != s.completedLoops || Math.Abs(expectedPosition - s.position) > Single.Epsilon) return !s.active;
                } else {
                    // Simply determine correct prevPosition after steps
                    if (s.loopType == LoopType.Yoyo && newCompletedSteps % 2 != 0) {
                        prevPosIsInverse = !prevPosIsInverse;
                        prevPos = s.duration - prevPos;
                    }
                    newCompletedSteps = 0;
                }
            }
            // Run current cycle
            if (newCompletedSteps == 1 && s.isComplete) return false; // Skip update if complete because multicycle took care of it
            if (newCompletedSteps > 0 && !s.isComplete) {
                from = useInversePosition ? s.duration : 0;
                // In case of Restart loop rewind all tweens (keep "to > 0" or remove it?)
                if (s.loopType == LoopType.Restart && to > 0) ApplyInternalCycle(s, s.duration, 0, UpdateMode.Goto, false, false, false);
            } else from = useInversePosition ? s.duration - prevPos : prevPos;
            return ApplyInternalCycle(s, from, useInversePosition ? s.duration - newPos : newPos, updateMode, useInversePosition, prevPosIsInverse);
        }

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        // Returns TRUE if the tween needs to be killed
        static bool ApplyInternalCycle(Sequence s, float fromPos, float toPos, UpdateMode updateMode, bool useInverse, bool prevPosIsInverse, bool multiCycleStep = false)
        {
            bool isBackwardsUpdate = toPos < fromPos;
//            Debug.Log(Time.frameCount + " " + s.id + " " + (multiCycleStep ? "<color=#FFEC03>Multicycle</color> > " : "Cycle > ") + s.position + "/" + s.duration + " - s.isBackwards: " + s.isBackwards + ", useInverse/prevInverse: " + useInverse + "/" + prevPosIsInverse + " - " + fromPos + " > " + toPos + " - UpdateMode: " + updateMode + ", isPlaying: " + s.isPlaying + ", completedLoops: " + s.completedLoops);
            if (isBackwardsUpdate) {
                int len = s._sequencedObjs.Count - 1;
                for (int i = len; i > -1; --i) {
                    if (!s.active) return true; // Killed by some internal callback
                    ABSSequentiable sequentiable = s._sequencedObjs[i];
                    if (sequentiable.sequencedEndPosition < toPos || sequentiable.sequencedPosition > fromPos) continue;
                    if (sequentiable.tweenType == TweenType.Callback) {
                        if (updateMode == UpdateMode.Update && prevPosIsInverse) {
//                            Debug.Log("<color=#FFEC03>BACKWARDS Callback > " + s.id + " - s.isBackwards: " + s.isBackwards + ", useInverse/prevInverse: " + useInverse + "/" + prevPosIsInverse + " - " + fromPos + " > " + toPos + "</color>");
                            OnTweenCallback(sequentiable.onStart);
                        }
                    } else {
                        // Nested Tweener/Sequence
                        float gotoPos = toPos - sequentiable.sequencedPosition;
//                        float gotoPos = (float)((decimal)toPos - (decimal)sequentiable.sequencedPosition);
                        if (gotoPos < 0) gotoPos = 0;
                        Tween t = (Tween)sequentiable;
                        if (!t.startupDone) continue; // since we're going backwards and this tween never started just ignore it
                        t.isBackwards = true;
                        if (TweenManager.Goto(t, gotoPos, false, updateMode)) return true;

                        // Fixes nested callbacks not being called correctly if main sequence has loops and nested ones don't
                        if (multiCycleStep && t.tweenType == TweenType.Sequence) {
                            if (s.position <= 0 && s.completedLoops == 0) t.position = 0;
                            else {
                                bool toZero = s.completedLoops == 0 || s.isBackwards && (s.completedLoops < s.loops || s.loops == -1);
                                if (t.isBackwards) toZero = !toZero;
                                if (useInverse) toZero = !toZero;
                                if (s.isBackwards && !useInverse && !prevPosIsInverse) toZero = !toZero;
                                t.position = toZero ? 0 : t.duration;
                            }
                        }
                    }
                }
            } else {
                int len = s._sequencedObjs.Count;
                for (int i = 0; i < len; ++i) {
                    if (!s.active) return true; // Killed by some internal callback
                    ABSSequentiable sequentiable = s._sequencedObjs[i];
                    if (sequentiable.sequencedPosition > toPos || sequentiable.sequencedEndPosition < fromPos) continue;
                    if (sequentiable.tweenType == TweenType.Callback) {
                        if (updateMode == UpdateMode.Update) {
//                            Debug.Log("<color=#FFEC03>FORWARD Callback > " + s.id + " - s.isBackwards: " + s.isBackwards + ", useInverse/prevInverse: " + useInverse + "/" + prevPosIsInverse + " - " + fromPos + " > " + toPos + "</color>");
                            bool fire = !s.isBackwards && !useInverse && !prevPosIsInverse
                                || s.isBackwards && useInverse && !prevPosIsInverse;
                            if (fire) OnTweenCallback(sequentiable.onStart);
                        }
                    } else {
                        // Nested Tweener/Sequence
                        float gotoPos = toPos - sequentiable.sequencedPosition;
//                        float gotoPos = (float)((decimal)toPos - (decimal)sequentiable.sequencedPosition);
                        if (gotoPos < 0) gotoPos = 0;
                        Tween t = (Tween)sequentiable;
                        // Fix for final nested tween not calling OnComplete in some cases
                        if (toPos >= sequentiable.sequencedEndPosition) {
                            if (!t.startupDone) TweenManager.ForceInit(t, true);
                            if (gotoPos < t.fullDuration) gotoPos = t.fullDuration;
                        }
                        //
                        t.isBackwards = false;
                        if (TweenManager.Goto(t, gotoPos, false, updateMode)) return true;

                        // Fixes nested callbacks not being called correctly if main sequence has loops and nested ones don't
                        if (multiCycleStep && t.tweenType == TweenType.Sequence) {
                            if (s.position <= 0 && s.completedLoops == 0) t.position = 0;
                            else {
                                bool toZero = s.completedLoops == 0 || !s.isBackwards && (s.completedLoops < s.loops || s.loops == -1);
                                if (t.isBackwards) toZero = !toZero;
                                if (useInverse) toZero = !toZero;
                                if (s.isBackwards && !useInverse && !prevPosIsInverse) toZero = !toZero;
                                t.position = toZero ? 0 : t.duration;
                            }
                        }
                    }
                }
            }
            return false;
        }

        static int SortSequencedObjs(ABSSequentiable a, ABSSequentiable b)
        {
            if (a.sequencedPosition > b.sequencedPosition) return 1;
            if (a.sequencedPosition < b.sequencedPosition) return -1;
            return 0;
        }
    }
}