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
        readonly StringBuilder _strb = new StringBuilder();
        bool _isRuntime;
        Texture2D _headerImg;
        string _playingTweensHex;
        string _pausedTweensHex;

        #region Unity + GUI

        void OnEnable()
        {
            _isRuntime = EditorApplication.isPlaying;
            ConnectToSource(true);

            _strb.Length = 0;
            _strb.Append("DOTween v").Append(DOTween.Version);
            if (TweenManager.isDebugBuild) _strb.Append(" [Debug build]");
            else _strb.Append(" [Release build]");

            if (EditorUtils.hasPro) _strb.Append("\nDOTweenPro v").Append(EditorUtils.proVersion);
            else _strb.Append("\nDOTweenPro not installed");
            _title = _strb.ToString();

            _playingTweensHex = EditorGUIUtility.isProSkin ? "<color=#00c514>" : "<color=#005408>";
            _pausedTweensHex = EditorGUIUtility.isProSkin ? "<color=#ff832a>" : "<color=#873600>";
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
            int totActiveFixedTweens = TweenManager.totActiveFixedTweens;
            int totActiveManualTweens = TweenManager.totActiveManualTweens;

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
                GUILayout.Label("<b>Legend: </b> TW = Tweener, SE = Sequence", EditorGUIUtils.wordWrapRichTextLabelStyle);

                GUILayout.Space(8);
                _strb.Length = 0;
                _strb.Append("Active tweens: ").Append(totActiveTweens)
                    .Append(" (").Append(TweenManager.totActiveTweeners).Append(" TW, ")
                    .Append(TweenManager.totActiveSequences).Append(" SE)")
                    .Append("\nDefault/Late/Fixed/Manual tweens: ").Append(totActiveDefaultTweens)
                    .Append("/").Append(totActiveLateTweens)
                    .Append("/").Append(totActiveFixedTweens)
                    .Append("/").Append(totActiveManualTweens)
                    .Append(_playingTweensHex).Append("\nPlaying tweens: ").Append(totPlayingTweens);
                if (_settings.showPlayingTweens) {
                    foreach (Tween t in TweenManager._activeTweens) {
                        if (t == null || !t.isPlaying) continue;
                        _strb.Append("\n   - [").Append(t.tweenType == TweenType.Tweener ? "TW" : "SE");
                        AppendTweenIdLabel(_strb, t);
                        _strb.Append("] ").Append(GetTargetTypeLabel(t.target));
                    }
                }
                _strb.Append("</color>");
                _strb.Append(_pausedTweensHex).Append("\nPaused tweens: ").Append(totPausedTweens);
                if (_settings.showPausedTweens) {
                    foreach (Tween t in TweenManager._activeTweens) {
                        if (t == null || t.isPlaying) continue;
                        _strb.Append("\n   - [").Append(t.tweenType == TweenType.Tweener ? "TW" : "SE");
                        AppendTweenIdLabel(_strb, t);
                        _strb.Append("] ").Append(GetTargetTypeLabel(t.target));
                    }
                }
                _strb.Append("</color>");
                _strb.Append("\nPooled tweens: ").Append(TweenManager.TotalPooledTweens())
                    .Append(" (").Append(TweenManager.totPooledTweeners).Append(" TW, ")
                    .Append(TweenManager.totPooledSequences).Append(" SE)");
                GUILayout.Label(_strb.ToString(), EditorGUIUtils.wordWrapRichTextLabelStyle);

                GUILayout.Space(8);
                _strb.Remove(0, _strb.Length);
                _strb.Append("Tweens Capacity: ").Append(TweenManager.maxTweeners).Append(" TW, ").Append(TweenManager.maxSequences).Append(" SE")
                    .Append("\nMax Simultaneous Active Tweens: ").Append(DOTween.maxActiveTweenersReached).Append(" TW, ")
                    .Append(DOTween.maxActiveSequencesReached).Append(" SE");
                GUILayout.Label(_strb.ToString(), EditorGUIUtils.wordWrapRichTextLabelStyle);
            }

            GUILayout.Space(8);
            _strb.Remove(0, _strb.Length);
            _strb.Append("<b>SETTINGS ▼</b>");
            _strb.Append("\nSafe Mode: ").Append((_isRuntime ? DOTween.useSafeMode : _settings.useSafeMode) ? "ON" : "OFF");
            _strb.Append("\nLog Behaviour: ").Append(_isRuntime ? DOTween.logBehaviour : _settings.logBehaviour);
            _strb.Append("\nShow Unity Editor Report: ").Append(_isRuntime ? DOTween.showUnityEditorReport : _settings.showUnityEditorReport);
            _strb.Append("\nTimeScale (Unity/DOTween): ").Append(Time.timeScale).Append("/").Append(_isRuntime ? DOTween.timeScale : _settings.timeScale);
            GUILayout.Label(_strb.ToString(), EditorGUIUtils.wordWrapRichTextLabelStyle);
            GUILayout.Label(
                "NOTE: DOTween's TimeScale is not the same as Unity's Time.timeScale: it is actually multiplied by it except for tweens that are set to update independently",
                EditorGUIUtils.wordWrapRichTextLabelStyle
            );

            GUILayout.Space(8);
            _strb.Remove(0, _strb.Length);
            _strb.Append("<b>DEFAULTS ▼</b>");
            _strb.Append("\ndefaultRecyclable: ").Append(_isRuntime ? DOTween.defaultRecyclable : _settings.defaultRecyclable);
            _strb.Append("\ndefaultUpdateType: ").Append(_isRuntime ? DOTween.defaultUpdateType : _settings.defaultUpdateType);
            _strb.Append("\ndefaultTSIndependent: ").Append(_isRuntime ? DOTween.defaultTimeScaleIndependent : _settings.defaultTimeScaleIndependent);
            _strb.Append("\ndefaultAutoKill: ").Append(_isRuntime ? DOTween.defaultAutoKill : _settings.defaultAutoKill);
            _strb.Append("\ndefaultAutoPlay: ").Append(_isRuntime ? DOTween.defaultAutoPlay : _settings.defaultAutoPlay);
            _strb.Append("\ndefaultEaseType: ").Append(_isRuntime ? DOTween.defaultEaseType : _settings.defaultEaseType);
            _strb.Append("\ndefaultLoopType: ").Append(_isRuntime ? DOTween.defaultLoopType : _settings.defaultLoopType);
            GUILayout.Label(_strb.ToString(), EditorGUIUtils.wordWrapRichTextLabelStyle);

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

        #region Helpers

        void AppendTweenIdLabel(StringBuilder strb, Tween t)
        {
            if (!string.IsNullOrEmpty(t.stringId)) strb.Append(":<b>").Append(t.stringId).Append("</b>");
            else if (t.intId != -999) strb.Append(":<b>").Append(t.intId).Append("</b>");
            else if (t.id != null) strb.Append(":<b>").Append(t.id).Append("</b>");
        }

        string GetTargetTypeLabel(object tweenTarget)
        {
            if (tweenTarget == null) return null;
            string s = tweenTarget.ToString();
            int dotIndex = s.LastIndexOf('.');
            if (dotIndex != -1) s = '(' + s.Substring(dotIndex + 1);
            return s;
        }

        #endregion
    }
}