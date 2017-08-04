// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/20 17:40
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System.Collections;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core
{
    /// <summary>
    /// Used to separate DOTween class from the MonoBehaviour instance (in order to use static constructors on DOTween).
    /// Contains all instance-based methods
    /// </summary>
    [AddComponentMenu("")]
    public class DOTweenComponent : MonoBehaviour, IDOTweenInit
    {
        /// <summary>Used internally inside Unity Editor, as a trick to update DOTween's inspector at every frame</summary>
        public int inspectorUpdater;

        float _unscaledTime;
        float _unscaledDeltaTime;

        bool _duplicateToDestroy;

        #region Unity Methods

        void Awake()
        {
            inspectorUpdater = 0;
            _unscaledTime = Time.realtimeSinceStartup;
        }

        void Start()
        {
            // Check if there's a leftover persistent DOTween object
            // (should be impossible but some weird Unity freeze caused that to happen on Seith's project
            if (DOTween.instance != this) {
                _duplicateToDestroy = true;
                Destroy(this.gameObject);
            }
        }

        void Update()
        {
            _unscaledDeltaTime = Time.realtimeSinceStartup - _unscaledTime;
            if (DOTween.useSmoothDeltaTime && _unscaledDeltaTime > DOTween.maxSmoothUnscaledTime) _unscaledDeltaTime = DOTween.maxSmoothUnscaledTime;
            if (TweenManager.hasActiveDefaultTweens) {
                TweenManager.Update(UpdateType.Normal, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, _unscaledDeltaTime * DOTween.timeScale);
            }
            _unscaledTime = Time.realtimeSinceStartup;

            if (DOTween.isUnityEditor) {
                inspectorUpdater++;
                if (DOTween.showUnityEditorReport && TweenManager.hasActiveTweens) {
                    if (TweenManager.totActiveTweeners > DOTween.maxActiveTweenersReached) DOTween.maxActiveTweenersReached = TweenManager.totActiveTweeners;
                    if (TweenManager.totActiveSequences > DOTween.maxActiveSequencesReached) DOTween.maxActiveSequencesReached = TweenManager.totActiveSequences;
                }
            }
        }

        void LateUpdate()
        {
            if (TweenManager.hasActiveLateTweens) {
                TweenManager.Update(UpdateType.Late, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, _unscaledDeltaTime * DOTween.timeScale);
            }
        }

        void FixedUpdate()
        {
            if (TweenManager.hasActiveFixedTweens && Time.timeScale > 0) {
                TweenManager.Update(UpdateType.Fixed, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, ((DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) / Time.timeScale) * DOTween.timeScale);
            }
        }

        internal void ManualUpdate(float deltaTime, float unscaledDeltaTime)
        {
            if (TweenManager.hasActiveManualTweens) {
                TweenManager.Update(UpdateType.Manual, deltaTime * DOTween.timeScale, unscaledDeltaTime * DOTween.timeScale);
            }
        }

        // Removed to allow compatibility with Unity 5.4 and later
//        void OnLevelWasLoaded()
//        {
//            if (DOTween.useSafeMode) DOTween.Validate();
//        }

        void OnDrawGizmos()
        {
            if (!DOTween.drawGizmos || !DOTween.isUnityEditor) return;

            int len = DOTween.GizmosDelegates.Count;
            if (len == 0) return;

            for (int i = 0; i < len; ++i) DOTween.GizmosDelegates[i]();
        }

        void OnDestroy()
        {
            if (_duplicateToDestroy) return;

            if (DOTween.showUnityEditorReport) {
                string s = "REPORT > Max overall simultaneous active Tweeners/Sequences: " + DOTween.maxActiveTweenersReached + "/" + DOTween.maxActiveSequencesReached;
                Debugger.LogReport(s);
            }
//            DOTween.initialized = false;
//            DOTween.instance = null;
            if (DOTween.instance == this) DOTween.instance = null;
        }

        void OnApplicationQuit()
        {
            DOTween.isQuitting = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Directly sets the current max capacity of Tweeners and Sequences
        /// (meaning how many Tweeners and Sequences can be running at the same time),
        /// so that DOTween doesn't need to automatically increase them in case the max is reached
        /// (which might lead to hiccups when that happens).
        /// Sequences capacity must be less or equal to Tweeners capacity
        /// (if you pass a low Tweener capacity it will be automatically increased to match the Sequence's).
        /// Beware: use this method only when there are no tweens running.
        /// </summary>
        /// <param name="tweenersCapacity">Max Tweeners capacity.
        /// Default: 200</param>
        /// <param name="sequencesCapacity">Max Sequences capacity.
        /// Default: 50</param>
        public IDOTweenInit SetCapacity(int tweenersCapacity, int sequencesCapacity)
        {
            TweenManager.SetCapacities(tweenersCapacity, sequencesCapacity);
            return this;
        }

        #endregion

        #region Yield Coroutines

        // CALLED BY TweenExtensions, creates a coroutine that waits for the tween to be complete (or killed)
        internal IEnumerator WaitForCompletion(Tween t)
        {
            while (t.active && !t.isComplete) yield return null;
        }

        // CALLED BY TweenExtensions, creates a coroutine that waits for the tween to be rewinded (or killed)
        internal IEnumerator WaitForRewind(Tween t)
        {
            while (t.active && (!t.playedOnce || t.position * (t.completedLoops + 1) > 0)) yield return null;
        }

        // CALLED BY TweenExtensions, creates a coroutine that waits for the tween to be killed
        internal IEnumerator WaitForKill(Tween t)
        {
            while (t.active) yield return null;
        }

        // CALLED BY TweenExtensions, creates a coroutine that waits for the tween to reach a given amount of loops (or to be killed)
        internal IEnumerator WaitForElapsedLoops(Tween t, int elapsedLoops)
        {
            while (t.active && t.completedLoops < elapsedLoops) yield return null;
        }

        // CALLED BY TweenExtensions, creates a coroutine that waits for the tween to reach a given time position (or to be killed)
        internal IEnumerator WaitForPosition(Tween t, float position)
        {
            while (t.active && t.position * (t.completedLoops + 1) < position) yield return null;
        }

        // CALLED BY TweenExtensions, creates a coroutine that waits for the tween to be started (or killed)
        internal IEnumerator WaitForStart(Tween t)
        {
            while (t.active && !t.playedOnce) yield return null;
        }

        #endregion

        internal static void Create()
        {
            if (DOTween.instance != null) return;

            GameObject go = new GameObject("[DOTween]");
            DontDestroyOnLoad(go);
            DOTween.instance = go.AddComponent<DOTweenComponent>();
        }

        internal static void DestroyInstance()
        {
            if (DOTween.instance != null) Destroy(DOTween.instance.gameObject);
            DOTween.instance = null;
        }
    }
}