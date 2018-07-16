// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/07/14 18:56
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.DOTweenEditor.Core;
using UnityEditor;
using UnityEngine;
using EditorUtils = DG.DOTweenEditor.Core.EditorUtils;

namespace DG.DOTweenEditor
{
    public static class DOTweenModulesSetupGUI
    {
        static bool _hasAudioModule;
        static bool _hasPhysicsModule;
        static bool _hasPhysics2DModule;
        static bool _hasSpriteModule;
        static bool _hasUIModule;

        static bool _hasTextMeshProModule;
        static bool _hasTk2DModule;

        public static void Refresh()
        {
            _hasAudioModule = EditorUtils.HasGlobalDefine(DOTweenSetup.GlobalDefine_AudioModule);
            _hasPhysicsModule = EditorUtils.HasGlobalDefine(DOTweenSetup.GlobalDefine_PhysicsModule);
            _hasPhysics2DModule = EditorUtils.HasGlobalDefine(DOTweenSetup.GlobalDefine_Physics2DModule);
            _hasSpriteModule = EditorUtils.HasGlobalDefine(DOTweenSetup.GlobalDefine_SpriteModule);
            _hasUIModule = EditorUtils.HasGlobalDefine(DOTweenSetup.GlobalDefine_UIModule);

            _hasTextMeshProModule = EditorUtils.HasGlobalDefine(DOTweenSetup.GlobalDefine_TextMeshPro);
            _hasTk2DModule = EditorUtils.HasGlobalDefine(DOTweenSetup.GlobalDefine_TK2D);
        }

        // Returns TRUE if it should be closed
        public static bool Draw()
        {
            GUILayout.Label("Add/Remove Modules", EditorGUIUtils.titleStyle);

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Unity", EditorGUIUtils.boldLabelStyle);
            _hasAudioModule = EditorGUILayout.Toggle("Audio", _hasAudioModule);
            _hasPhysicsModule = EditorGUILayout.Toggle("Physics", _hasPhysicsModule);
            _hasPhysics2DModule = EditorGUILayout.Toggle("Physics2D", _hasPhysics2DModule);
            _hasSpriteModule = EditorGUILayout.Toggle("Sprites", _hasSpriteModule);
            _hasUIModule = EditorGUILayout.Toggle("UI", _hasUIModule);
            EditorGUILayout.EndVertical();
            if (EditorUtils.hasPro) {
                GUILayout.BeginVertical(GUI.skin.box);
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
            if (GUILayout.Button("Cancel")) return true;
            GUILayout.EndHorizontal();

//            EditorGUILayout.HelpBox(
//                "NOTE: if you get \"PlayerSettings Validation\" or [CS0618] errors when you press apply don't worry:" +
//                " it's ok and it allows the setup to work on all possible Unity versions",
//                MessageType.Warning
//            );
            return false;
        }

        static void Apply()
        {
            ModifyDefineIfChanged(_hasAudioModule, DOTweenSetup.GlobalDefine_AudioModule);
            ModifyDefineIfChanged(_hasPhysicsModule, DOTweenSetup.GlobalDefine_PhysicsModule);
            ModifyDefineIfChanged(_hasPhysics2DModule, DOTweenSetup.GlobalDefine_Physics2DModule);
            ModifyDefineIfChanged(_hasSpriteModule, DOTweenSetup.GlobalDefine_SpriteModule);
            ModifyDefineIfChanged(_hasUIModule, DOTweenSetup.GlobalDefine_UIModule);

            if (EditorUtils.hasPro) {
                ModifyDefineIfChanged(_hasTextMeshProModule, DOTweenSetup.GlobalDefine_TextMeshPro);
                ModifyDefineIfChanged(_hasTk2DModule, DOTweenSetup.GlobalDefine_TK2D);
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
    }
}