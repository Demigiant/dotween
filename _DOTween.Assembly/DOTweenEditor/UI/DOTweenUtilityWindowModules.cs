// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/07/14 18:56
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.UI
{
    public static class DOTweenUtilityWindowModules
    {
        static bool _refreshed;
        static bool _hasAudioModule;
        static bool _hasPhysicsModule;
        static bool _hasPhysics2DModule;
        static bool _hasSpriteModule;
        static bool _hasUIModule;

        static bool _hasTextMeshProModule;
        static bool _hasTk2DModule;

        static EditorWindow _editor;
        static bool _isWaitingForCompilation;

        public static void Refresh()
        {
            _refreshed = true;

            _hasAudioModule = EditorUtils.HasGlobalDefine(DOTweenDefines.GlobalDefine_AudioModule);
            _hasPhysicsModule = EditorUtils.HasGlobalDefine(DOTweenDefines.GlobalDefine_PhysicsModule);
            _hasPhysics2DModule = EditorUtils.HasGlobalDefine(DOTweenDefines.GlobalDefine_Physics2DModule);
            _hasSpriteModule = EditorUtils.HasGlobalDefine(DOTweenDefines.GlobalDefine_SpriteModule);
            _hasUIModule = EditorUtils.HasGlobalDefine(DOTweenDefines.GlobalDefine_UIModule);

            _hasTextMeshProModule = EditorUtils.HasGlobalDefine(DOTweenDefines.GlobalDefine_TextMeshPro);
            _hasTk2DModule = EditorUtils.HasGlobalDefine(DOTweenDefines.GlobalDefine_TK2D);
        }

        // Returns TRUE if it should be closed
        public static bool Draw(EditorWindow editor)
        {
            _editor = editor;
            if (!_refreshed) Refresh();

            GUILayout.Label("Add/Remove Modules", EditorGUIUtils.titleStyle);

            GUILayout.BeginVertical();
            EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);
            GUILayout.BeginVertical(UnityEngine.GUI.skin.box);
            GUILayout.Label("Unity", EditorGUIUtils.boldLabelStyle);
            _hasAudioModule = EditorGUILayout.Toggle("Audio", _hasAudioModule);
            _hasPhysicsModule = EditorGUILayout.Toggle("Physics", _hasPhysicsModule);
            _hasPhysics2DModule = EditorGUILayout.Toggle("Physics2D", _hasPhysics2DModule);
            _hasSpriteModule = EditorGUILayout.Toggle("Sprites", _hasSpriteModule);
            _hasUIModule = EditorGUILayout.Toggle("UI", _hasUIModule);
            EditorGUILayout.EndVertical();
            if (EditorUtils.hasPro) {
                GUILayout.BeginVertical(UnityEngine.GUI.skin.box);
                GUILayout.Label("External Assets (Pro)", EditorGUIUtils.boldLabelStyle);
                _hasTk2DModule = EditorGUILayout.Toggle("2D Toolkit", _hasTk2DModule);
                _hasTextMeshProModule = EditorGUILayout.Toggle("TextMesh Pro", _hasTextMeshProModule);
                EditorGUILayout.EndVertical();
            }

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply")) {
                Apply();
                return true;
            }
            if (GUILayout.Button("Cancel")) {
                return true;
            }
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
            GUILayout.EndVertical();

            if (EditorApplication.isCompiling) WaitForCompilation();

            return false;
        }

        static void Apply()
        {
            ModifyDefineIfChanged(_hasAudioModule, DOTweenDefines.GlobalDefine_AudioModule);
            ModifyDefineIfChanged(_hasPhysicsModule, DOTweenDefines.GlobalDefine_PhysicsModule);
            ModifyDefineIfChanged(_hasPhysics2DModule, DOTweenDefines.GlobalDefine_Physics2DModule);
            ModifyDefineIfChanged(_hasSpriteModule, DOTweenDefines.GlobalDefine_SpriteModule);
            ModifyDefineIfChanged(_hasUIModule, DOTweenDefines.GlobalDefine_UIModule);

            if (EditorUtils.hasPro) {
                ModifyDefineIfChanged(_hasTextMeshProModule, DOTweenDefines.GlobalDefine_TextMeshPro);
                ModifyDefineIfChanged(_hasTk2DModule, DOTweenDefines.GlobalDefine_TK2D);
            }
        }

        static void ModifyDefineIfChanged(bool wantsToBeSet, string defineId)
        {
            bool hasDefine = EditorUtils.HasGlobalDefine(defineId);
            if (wantsToBeSet != hasDefine) {
                if (wantsToBeSet) EditorUtils.AddGlobalDefine(defineId);
                else EditorUtils.RemoveGlobalDefine(defineId);
            }
        }

        static void WaitForCompilation()
        {
            if (!_isWaitingForCompilation) {
                _isWaitingForCompilation = true;
                EditorApplication.update += WaitForCompilation_Update;
                WaitForCompilation_Update();
            }

//            Rect r = GUILayoutUtility.GetLastRect();
//            EditorGUI.HelpBox(r, "Waiting for Unity to finish the compilation process...", MessageType.Info);
            EditorGUILayout.HelpBox("Waiting for Unity to finish the compilation process...", MessageType.Info);
        }

        static void WaitForCompilation_Update()
        {
            if (!EditorApplication.isCompiling) {
                EditorApplication.update -= WaitForCompilation_Update;
                _isWaitingForCompilation = false;
                Refresh();
            }
            _editor.Repaint();
        }
    }
}