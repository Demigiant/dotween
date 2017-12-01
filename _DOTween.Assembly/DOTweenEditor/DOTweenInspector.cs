// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/06/29 20:37
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DG.DOTweenEditor.Core;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    [CustomEditor(typeof(DOTweenComponent))]
    public class DOTweenInspector : Editor
    {
        DOTweenSettings _settings;
        string _title;
        readonly StringBuilder _strBuilder = new StringBuilder();

        // ===================================================================================
        // MONOBEHAVIOUR METHODS -------------------------------------------------------------

        void OnEnable()
        {
            if (_settings == null) _settings = Resources.Load(DOTweenSettings.AssetName) as DOTweenSettings;

            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("DOTween v").Append(DOTween.Version);
            if (DOTween.isDebugBuild) _strBuilder.Append(" [Debug build]");
            else _strBuilder.Append(" [Release build]");

            if (EditorUtils.hasPro) _strBuilder.Append("\nDOTweenPro v").Append(EditorUtils.proVersion);
            else _strBuilder.Append("\nDOTweenPro not installed");
            _title = _strBuilder.ToString();
        }

        override public void OnInspectorGUI()
        {
            EditorGUIUtils.SetGUIStyles();

            int totActiveTweens = TweenManager.totActiveTweens;
            int totPlayingTweens = TweenManager.TotalPlayingTweens();
            int totPausedTweens = totActiveTweens - totPlayingTweens;
            int totActiveDefaultTweens = TweenManager.totActiveDefaultTweens;
            int totActiveLateTweens = TweenManager.totActiveLateTweens;

            GUILayout.Space(4);
            GUILayout.Label(_title, DOTween.isDebugBuild ? EditorGUIUtils.redLabelStyle : EditorGUIUtils.boldLabelStyle);

            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Documentation")) Application.OpenURL("http://dotween.demigiant.com/documentation.php");
            if (GUILayout.Button("Check Updates")) Application.OpenURL("http://dotween.demigiant.com/download.php?v=" + DOTween.Version);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_settings.showPlayingTweens ? "Hide Playing Tweens" : "Show Playing Tweens")) {
                _settings.showPlayingTweens = !_settings.showPlayingTweens;
                EditorUtility.SetDirty(_settings);
            }
            if (GUILayout.Button(_settings.showPausedTweens ? "Hide Paused Tweens" : "Show Paused Tweens")) {
                _settings.showPausedTweens = !_settings.showPausedTweens;
                EditorUtility.SetDirty(_settings);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Play all")) DOTween.PlayAll();
            if (GUILayout.Button("Pause all")) DOTween.PauseAll();
            if (GUILayout.Button("Kill all")) DOTween.KillAll();
            GUILayout.EndHorizontal();

            GUILayout.Space(8);
            _strBuilder.Length = 0;
            _strBuilder.Append("Active tweens: ").Append(totActiveTweens)
                    .Append(" (").Append(TweenManager.totActiveTweeners)
                    .Append("/").Append(TweenManager.totActiveSequences).Append(")")
                .Append("\nDefault/Late tweens: ").Append(totActiveDefaultTweens)
                    .Append("/").Append(totActiveLateTweens)
                    .Append("\nPlaying tweens: ").Append(totPlayingTweens);
            if (_settings.showPlayingTweens) {
                foreach (Tween t in TweenManager._activeTweens) {
                    if (t != null && t.isPlaying) _strBuilder.Append("\n   - [").Append(t.tweenType).Append("] ").Append(t.target);
                }
            }
            _strBuilder.Append("\nPaused tweens: ").Append(totPausedTweens);
            if (_settings.showPausedTweens) {
                foreach (Tween t in TweenManager._activeTweens) {
                    if (t != null && !t.isPlaying) _strBuilder.Append("\n   - [").Append(t.tweenType).Append("] ").Append(t.target);
                }
            }
            _strBuilder.Append("\nPooled tweens: ").Append(TweenManager.TotalPooledTweens())
                    .Append(" (").Append(TweenManager.totPooledTweeners)
                    .Append("/").Append(TweenManager.totPooledSequences).Append(")");
            GUILayout.Label(_strBuilder.ToString());

            GUILayout.Space(8);
            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("Tweens Capacity: ").Append(TweenManager.maxTweeners).Append("/").Append(TweenManager.maxSequences)
                .Append("\nMax Simultaneous Active Tweens: ").Append(DOTween.maxActiveTweenersReached).Append("/").Append(DOTween.maxActiveSequencesReached);
            GUILayout.Label(_strBuilder.ToString());

            GUILayout.Space(8);
            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("SETTINGS ▼");
            _strBuilder.Append("\nSafe Mode: ").Append(DOTween.useSafeMode ? "ON" : "OFF");
            _strBuilder.Append("\nLog Behaviour: ").Append(DOTween.logBehaviour);
            _strBuilder.Append("\nShow Unity Editor Report: ").Append(DOTween.showUnityEditorReport);
            _strBuilder.Append("\nTimeScale (Unity/DOTween): ").Append(Time.timeScale).Append("/").Append(DOTween.timeScale);
            GUILayout.Label(_strBuilder.ToString());
            GUILayout.Label("NOTE: DOTween's TimeScale is not the same as Unity's Time.timeScale: it is actually multiplied by it except for tweens that are set to update independently", EditorGUIUtils.wordWrapItalicLabelStyle);

            GUILayout.Space(8);
            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("DEFAULTS ▼");
            _strBuilder.Append("\ndefaultRecyclable: ").Append(DOTween.defaultRecyclable);
            _strBuilder.Append("\ndefaultUpdateType: ").Append(DOTween.defaultUpdateType);
            _strBuilder.Append("\ndefaultTSIndependent: ").Append(DOTween.defaultTimeScaleIndependent);
            _strBuilder.Append("\ndefaultAutoKill: ").Append(DOTween.defaultAutoKill);
            _strBuilder.Append("\ndefaultAutoPlay: ").Append(DOTween.defaultAutoPlay);
            _strBuilder.Append("\ndefaultEaseType: ").Append(DOTween.defaultEaseType);
            _strBuilder.Append("\ndefaultLoopType: ").Append(DOTween.defaultLoopType);
            GUILayout.Label(_strBuilder.ToString());

            GUILayout.Space(10);
        }
    }
}