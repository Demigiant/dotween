// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/06/29 20:37
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System.Text;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.UI
{
    [CustomEditor(typeof(DOTweenComponent))]
    public class DOTweenComponentInspector : Editor
    {
        DOTweenSettings _settings;
        string _title;
        readonly StringBuilder _strBuilder = new StringBuilder();
        bool _isRuntime;
        Texture2D _headerImg;

        #region Unity + GUI

        void OnEnable()
        {
            _isRuntime = EditorApplication.isPlaying;
            ConnectToSource(true);

            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("DOTween v").Append(DOTween.Version);
            if (TweenManager.isDebugBuild) _strBuilder.Append(" [Debug build]");
            else _strBuilder.Append(" [Release build]");

            if (EditorUtils.hasPro) _strBuilder.Append("\nDOTweenPro v").Append(EditorUtils.proVersion);
            else _strBuilder.Append("\nDOTweenPro not installed");
            _title = _strBuilder.ToString();
        }

        override public void OnInspectorGUI()
        {
            _isRuntime = EditorApplication.isPlaying;
            ConnectToSource();

            EditorGUIUtils.SetGUIStyles();

            // Header img
            GUILayout.Space(4);
            GUILayout.BeginHorizontal();
            Rect headeR = GUILayoutUtility.GetRect(0, 93, 18, 18);
            GUI.DrawTexture(headeR, _headerImg, ScaleMode.ScaleToFit, true);
            GUILayout.Label(_isRuntime ? "RUNTIME MODE" : "EDITOR MODE");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            int totActiveTweens = TweenManager.totActiveTweens;
            int totPlayingTweens = TweenManager.TotalPlayingTweens();
            int totPausedTweens = totActiveTweens - totPlayingTweens;
            int totActiveDefaultTweens = TweenManager.totActiveDefaultTweens;
            int totActiveLateTweens = TweenManager.totActiveLateTweens;

            GUILayout.Label(_title, TweenManager.isDebugBuild ? EditorGUIUtils.redLabelStyle : EditorGUIUtils.boldLabelStyle);

            if (!_isRuntime) {
                GUI.backgroundColor = new Color(0f, 0.31f, 0.48f);
                GUI.contentColor = Color.white;
                GUILayout.Label(
                    "This component is <b>added automatically</b> by DOTween at runtime." +
                    "\nAdding it yourself is <b>not recommended</b> unless you really know what you're doing:" +
                    " you'll have to be sure it's <b>never destroyed</b> and that it's present <b>in every scene</b>.",
                    EditorGUIUtils.infoboxStyle
                );
                GUI.backgroundColor = GUI.contentColor = GUI.contentColor = Color.white;
            }

            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Documentation")) Application.OpenURL("http://dotween.demigiant.com/documentation.php");
            if (GUILayout.Button("Check Updates")) Application.OpenURL("http://dotween.demigiant.com/download.php?v=" + DOTween.Version);
            GUILayout.EndHorizontal();

            if (_isRuntime) {
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
            }

            GUILayout.Space(8);
            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("SETTINGS ▼");
            _strBuilder.Append("\nSafe Mode: ").Append((_isRuntime ? DOTween.useSafeMode : _settings.useSafeMode) ? "ON" : "OFF");
            _strBuilder.Append("\nLog Behaviour: ").Append(_isRuntime ? DOTween.logBehaviour : _settings.logBehaviour);
            _strBuilder.Append("\nShow Unity Editor Report: ").Append(_isRuntime ? DOTween.showUnityEditorReport : _settings.showUnityEditorReport);
            _strBuilder.Append("\nTimeScale (Unity/DOTween): ").Append(Time.timeScale).Append("/").Append(_isRuntime ? DOTween.timeScale : _settings.timeScale);
            GUILayout.Label(_strBuilder.ToString());
            GUILayout.Label("NOTE: DOTween's TimeScale is not the same as Unity's Time.timeScale: it is actually multiplied by it except for tweens that are set to update independently", EditorGUIUtils.wordWrapItalicLabelStyle);

            GUILayout.Space(8);
            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("DEFAULTS ▼");
            _strBuilder.Append("\ndefaultRecyclable: ").Append(_isRuntime ? DOTween.defaultRecyclable : _settings.defaultRecyclable);
            _strBuilder.Append("\ndefaultUpdateType: ").Append(_isRuntime ? DOTween.defaultUpdateType : _settings.defaultUpdateType);
            _strBuilder.Append("\ndefaultTSIndependent: ").Append(_isRuntime ? DOTween.defaultTimeScaleIndependent : _settings.defaultTimeScaleIndependent);
            _strBuilder.Append("\ndefaultAutoKill: ").Append(_isRuntime ? DOTween.defaultAutoKill : _settings.defaultAutoKill);
            _strBuilder.Append("\ndefaultAutoPlay: ").Append(_isRuntime ? DOTween.defaultAutoPlay : _settings.defaultAutoPlay);
            _strBuilder.Append("\ndefaultEaseType: ").Append(_isRuntime ? DOTween.defaultEaseType : _settings.defaultEaseType);
            _strBuilder.Append("\ndefaultLoopType: ").Append(_isRuntime ? DOTween.defaultLoopType : _settings.defaultLoopType);
            GUILayout.Label(_strBuilder.ToString());

            GUILayout.Space(10);
        }

        #endregion

        #region Methods

        void ConnectToSource(bool forceReconnection = false)
        {
            _headerImg = AssetDatabase.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + "Imgs/DOTweenIcon.png", typeof(Texture2D)) as Texture2D;

            if (_settings == null || forceReconnection) {
                _settings = _isRuntime
                    ? Resources.Load(DOTweenSettings.AssetName) as DOTweenSettings
                    : DOTweenUtilityWindow.GetDOTweenSettings();
            }
        }

        #endregion
    }
}