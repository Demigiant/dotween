// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/07/14 18:56
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace DG.DOTweenEditor.UI
{
    public static class DOTweenUtilityWindowModules
    {
        static EditorWindow _editor;
        static DOTweenSettings _src;
        static bool _dotweenSettingsRefreshed;
        static bool _isWaitingForCompilation;

        #region GUI

        // Returns TRUE if it should be closed
        public static bool Draw(EditorWindow editor, DOTweenSettings src)
        {
            _editor = editor;
            _src = src;

            GUILayout.Label("Add/Remove Modules", EditorGUIUtils.titleStyle);

            float prevLabelWidth = EditorGUIUtility.labelWidth;
            GUILayout.BeginVertical();
            {
                EditorGUIUtility.labelWidth = 186;
                EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);
                {
                    // Core Unity modules
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        GUILayout.Label("Unity", EditorGUIUtils.boldLabelStyle);
                        DefineToggle("Audio", DOTweenDefines.NoAudio);
                        DefineToggle("Physics", DOTweenDefines.NoPhysics);
                        DefineToggle("Physics2D", DOTweenDefines.NoPhysics2D);
                        DefineToggle("Sprites", DOTweenDefines.NoSprites);
                        DefineToggle("UI", DOTweenDefines.NoUI);
                        bool uiToolkitModulesDisabled = UnityEditorVersion.MajorVersion < 2021 || UnityEditorVersion.MajorVersion == 2021 && UnityEditorVersion.MinorVersion < 3;
                        using (new EditorGUI.DisabledScope(uiToolkitModulesDisabled)) {
                            DefineToggle("UI Toolkit (Unity 2021.3 or later)", DOTweenDefines.UIToolkit);
                        }
                    }
                    GUILayout.EndVertical();
                    // External assets modules
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        GUILayout.Label("External Assets", EditorGUIUtils.boldLabelStyle);
                        EditorGUILayout.HelpBox(
                            "These modules are for external Unity assets." +
                            "\nDO NOT activate them unless you have the relative asset in your project.",
                            MessageType.Warning
                        );
                        // - Free modules
                        GUILayout.BeginVertical(GUI.skin.box);
                        {
                            GUILayout.Label("DOTween Free/Core", EditorGUIUtils.boldLabelStyle);
                            DefineToggle("Easy Performant Outline", DOTweenDefines.EasyPerformantOutline);
                        }
                        GUILayout.EndVertical();
                        // - Pro modules
                        using (new EditorGUI.DisabledScope(!EditorUtils.hasPro && !EditorUtils.hasDOTweenTimeline)) {
                            GUILayout.BeginVertical(GUI.skin.box);
                            GUILayout.Label("DOTween Pro / DOTween Timeline", EditorGUIUtils.boldLabelStyle);
                            DefineToggle("DeAudio", DOTweenDefines.DeAudio);
                            DefineToggle("DeUnityExtended", DOTweenDefines.DeUnityExtended);
                            DefineToggle("TextMesh Pro", DOTweenDefines.TextMeshPro);
                            DefineToggle("2D Toolkit (legacy)", DOTweenDefines.TK2D);
                            GUILayout.EndVertical();
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Apply")) {
                            Apply();
                            RefreshDOTweenSettings(_src);
                            return true;
                        }
                        if (GUILayout.Button("Cancel")) {
                            return true;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndVertical();
            EditorGUIUtility.labelWidth = prevLabelWidth;

            if (EditorApplication.isCompiling) {
                EditorGUILayout.HelpBox("Waiting for Unity to finish the compilation process...", MessageType.Info);
            }

            return false;
        }

        static void DefineToggle(string label, DOTweenDefines.Def define)
        {
            define.guiEnabled = define.isInverted
                ? !EditorGUILayout.Toggle(label, !define.guiEnabled)
                : EditorGUILayout.Toggle(label, define.guiEnabled);
        }

        [DidReloadScripts]
        static void OnScriptsReloaded()
        {
            DOTweenDefines.RefreshAll();
        }

        #endregion

        #region Public Methods

        // Applies modules settings based on src.modules
        public static void ApplyModulesSettings()
        {
            DOTweenSettings src = DOTweenUtilityWindow.GetDOTweenSettings();
            if (src == null) return;

            DOTweenDefines.NoAudio.Enable(!src.modules.audioEnabled);
            DOTweenDefines.NoPhysics.Enable(!src.modules.physicsEnabled);
            DOTweenDefines.NoPhysics2D.Enable(!src.modules.physics2DEnabled);
            DOTweenDefines.NoSprites.Enable(!src.modules.spriteEnabled);
            DOTweenDefines.NoUI.Enable(!src.modules.uiEnabled);
            
            DOTweenDefines.UIToolkit.Enable(src.modules.uiToolkitEnabled);
            
            DOTweenDefines.EasyPerformantOutline.Enable(src.modules.epoOutlineEnabled);
            
            DOTweenDefines.DeAudio.Enable(src.modules.deAudioEnabled);
            DOTweenDefines.DeUnityExtended.Enable(src.modules.deUnityExtendedEnabled);
            DOTweenDefines.TextMeshPro.Enable(src.modules.textMeshProEnabled);
            DOTweenDefines.TK2D.Enable(src.modules.tk2DEnabled);

            EditorUtility.SetDirty(src);
        }

        public static void RefreshDOTweenSettings(DOTweenSettings src)
        {
            _dotweenSettingsRefreshed = true;
            DOTweenDefines.RefreshDOTweenSettings(src);
        }

        #endregion

        #region Methods

        static void Apply()
        {
            DOTweenDefines.ApplyGUIEnabledToAll();
            ASMDEFManager.RefreshExistingASMDEFFiles();
        }

        #endregion
    }
}