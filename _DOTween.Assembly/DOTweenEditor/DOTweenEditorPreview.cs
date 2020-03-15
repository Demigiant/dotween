// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/08/17 12:18
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DG.DOTweenEditor
{
    public static class DOTweenEditorPreview
    {
        static bool _isPreviewing;
        static double _previewTime;
        static Action _onPreviewUpdated;
//        static GameObject _previewObj; // Used so it can be set dirty (otherwise canvas-only tweens won't refresh the view) - apparently not needed anymore (test)
        static readonly List<Tween> _Tweens = new List<Tween>();

//        static DOTweenEditorPreview()
//        {
//            ClearPreviewObject();
//        }

        #region Public Methods

        /// <summary>
        /// Starts the update loop of tween in the editor. Has no effect during playMode.
        /// </summary>
        /// <param name="onPreviewUpdated">Eventual callback to call after every update</param>
        public static void Start(Action onPreviewUpdated = null)
        {
            if (_isPreviewing || EditorApplication.isPlayingOrWillChangePlaymode) return;

            _isPreviewing = true;
            _onPreviewUpdated = onPreviewUpdated;
            _previewTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += PreviewUpdate;
//            _previewObj = new GameObject("-[ DOTween Preview ► ]-", typeof(PreviewComponent));
        }

        /// <summary>
        /// Stops the update loop and clears the onPreviewUpdated callback.
        /// </summary>
        /// <param name="resetTweenTargets">If TRUE also resets the tweened objects to their original state</param>
        public static void Stop(bool resetTweenTargets = false)
        {
            _isPreviewing = false;
            EditorApplication.update -= PreviewUpdate;
            _onPreviewUpdated = null;
            if (resetTweenTargets) {
                foreach (Tween t in _Tweens) {
                    try {
                        if (t.isFrom) t.Complete();
                        else t.Rewind();
                    } catch {
                        // Ignore
                    }
                }
            }
            ValidateTweens();
//            ClearPreviewObject();
        }

        /// <summary>
        /// Readies the tween for editor preview by setting its UpdateType to Manual plus eventual extra settings.
        /// </summary>
        /// <param name="t">The tween to ready</param>
        /// <param name="clearCallbacks">If TRUE (recommended) removes all callbacks (OnComplete/Rewind/etc)</param>
        /// <param name="preventAutoKill">If TRUE prevents the tween from being auto-killed at completion</param>
        /// <param name="andPlay">If TRUE starts playing the tween immediately</param>
        public static void PrepareTweenForPreview(Tween t, bool clearCallbacks = true, bool preventAutoKill = true, bool andPlay = true)
        {
            _Tweens.Add(t);
            t.SetUpdate(UpdateType.Manual);
            if (preventAutoKill) t.SetAutoKill(false);
            if (clearCallbacks) {
                t.OnComplete(null)
                    .OnStart(null).OnPlay(null).OnPause(null).OnUpdate(null).OnWaypointChange(null)
                    .OnStepComplete(null).OnRewind(null).OnKill(null);
            }
            if (andPlay) t.Play();
        }

        #endregion

        #region Methods

//        static void ClearPreviewObject()
//        {
//            _previewObj = null;
//            // Find and destroy any existing preview objects
//            PreviewComponent[] objs = Object.FindObjectsOfType<PreviewComponent>();
//            for (int i = 0; i < objs.Length; ++i) Object.DestroyImmediate(objs[i].gameObject);
//        }

        static void PreviewUpdate()
        {
            double currTime = _previewTime;
            _previewTime = EditorApplication.timeSinceStartup;
            float elapsed = (float)(_previewTime - currTime);
            DOTween.ManualUpdate(elapsed, elapsed);
            
//            if (_previewObj != null) EditorUtility.SetDirty(_previewObj);

            if (_onPreviewUpdated != null) _onPreviewUpdated();
        }

        static void ValidateTweens()
        {
            for (int i = _Tweens.Count - 1; i > -1; --i) {
                if (_Tweens[i] == null || !_Tweens[i].active) _Tweens.RemoveAt(i);
            }
        }

        #endregion
//
//        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
//        // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
//        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
//
//        class PreviewComponent : MonoBehaviour {}
    }
}