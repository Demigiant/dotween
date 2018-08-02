// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/07/14 18:56
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System.Collections.Generic;
using System.IO;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.UI
{
    public static class DOTweenUtilityWindowModules
    {
        const string ModuleMarkerId = "MODULE_MARKER";

        static readonly ModuleInfo _audioModule = new ModuleInfo("Modules/DOTweenModuleAudio.cs", "AUDIO");
        static readonly ModuleInfo _physicsModule = new ModuleInfo("Modules/DOTweenModulePhysics.cs", "PHYSICS");
        static readonly ModuleInfo _physics2DModule = new ModuleInfo("Modules/DOTweenModulePhysics2D.cs", "PHYSICS2D");
        static readonly ModuleInfo _spriteModule = new ModuleInfo("Modules/DOTweenModuleSprite.cs", "SPRITE");
        static readonly ModuleInfo _uiModule = new ModuleInfo("Modules/DOTweenModuleUI.cs", "UI");
        static readonly ModuleInfo _textMeshProModule = new ModuleInfo("DOTweenTextMeshPro.cs", "TEXTMESHPRO");
        static readonly ModuleInfo _tk2DModule = new ModuleInfo("DOTweenTk2D.cs", "TK2D");

        static readonly string _ModuleUtilsPath = "Modules/DOTweenModuleUtils.cs";

        static EditorWindow _editor;
        static DOTweenSettings _src;
        static bool _refreshed;
        static bool _isWaitingForCompilation;
        static readonly List<int> _LinesToChange = new List<int>();

        static DOTweenUtilityWindowModules()
        {
            _ModuleUtilsPath = EditorUtils.dotweenDir + _ModuleUtilsPath;
            _audioModule.filePath = EditorUtils.dotweenDir + _audioModule.filePath;
            _physicsModule.filePath = EditorUtils.dotweenDir + _physicsModule.filePath;
            _physics2DModule.filePath = EditorUtils.dotweenDir + _physics2DModule.filePath;
            _spriteModule.filePath = EditorUtils.dotweenDir + _spriteModule.filePath;
            _uiModule.filePath = EditorUtils.dotweenDir + _uiModule.filePath;
            _textMeshProModule.filePath = EditorUtils.dotweenProDir + _textMeshProModule.filePath;
            _tk2DModule.filePath = EditorUtils.dotweenProDir + _tk2DModule.filePath;
        }

        #region GUI

        // Returns TRUE if it should be closed
        public static bool Draw(EditorWindow editor, DOTweenSettings src)
        {
            _editor = editor;
            _src = src;
            if (!_refreshed) Refresh(_src);

            GUILayout.Label("Add/Remove Modules", EditorGUIUtils.titleStyle);

            GUILayout.BeginVertical();
            EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);
            GUILayout.BeginVertical(UnityEngine.GUI.skin.box);
            GUILayout.Label("Unity", EditorGUIUtils.boldLabelStyle);
            _audioModule.enabled = EditorGUILayout.Toggle("Audio", _audioModule.enabled);
            _physicsModule.enabled = EditorGUILayout.Toggle("Physics", _physicsModule.enabled);
            _physics2DModule.enabled = EditorGUILayout.Toggle("Physics2D", _physics2DModule.enabled);
            _spriteModule.enabled = EditorGUILayout.Toggle("Sprites", _spriteModule.enabled);
            _uiModule.enabled = EditorGUILayout.Toggle("UI", _uiModule.enabled);
            EditorGUILayout.EndVertical();
            if (EditorUtils.hasPro) {
                GUILayout.BeginVertical(UnityEngine.GUI.skin.box);
                GUILayout.Label("External Assets (Pro)", EditorGUIUtils.boldLabelStyle);
                _textMeshProModule.enabled = EditorGUILayout.Toggle("TextMesh Pro", _textMeshProModule.enabled);
                _tk2DModule.enabled = EditorGUILayout.Toggle("2D Toolkit", _tk2DModule.enabled);
                EditorGUILayout.EndVertical();
            }

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply")) {
                Apply();
                Refresh(_src);
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

        static void WaitForCompilation()
        {
            if (!_isWaitingForCompilation) {
                _isWaitingForCompilation = true;
                EditorApplication.update += WaitForCompilation_Update;
                WaitForCompilation_Update();
            }
            EditorGUILayout.HelpBox("Waiting for Unity to finish the compilation process...", MessageType.Info);
        }

        static void WaitForCompilation_Update()
        {
            if (!EditorApplication.isCompiling) {
                EditorApplication.update -= WaitForCompilation_Update;
                _isWaitingForCompilation = false;
                Refresh(_src);
            }
            _editor.Repaint();
        }

        #endregion

        #region Methods

        public static void Refresh(DOTweenSettings src, bool applySrcSettings = false)
        {
            _src = src;
            _refreshed = true;

            AssetDatabase.StartAssetEditing();
            _audioModule.enabled = ModuleIsEnabled(_audioModule);
            _physicsModule.enabled = ModuleIsEnabled(_physicsModule);
            _physics2DModule.enabled = ModuleIsEnabled(_physics2DModule);
            _spriteModule.enabled = ModuleIsEnabled(_spriteModule);
            _uiModule.enabled = ModuleIsEnabled(_uiModule);
            //
            _textMeshProModule.enabled = ModuleIsEnabled(_textMeshProModule);
            _tk2DModule.enabled = ModuleIsEnabled(_tk2DModule);

            CheckAutoModuleSettings(applySrcSettings, _audioModule, ref src.modules.audioEnabled);
            CheckAutoModuleSettings(applySrcSettings, _physicsModule, ref src.modules.physicsEnabled);
            CheckAutoModuleSettings(applySrcSettings, _physics2DModule, ref src.modules.physics2DEnabled);
            CheckAutoModuleSettings(applySrcSettings, _spriteModule, ref src.modules.spriteEnabled);
            CheckAutoModuleSettings(applySrcSettings, _uiModule, ref src.modules.uiEnabled);
            //
            CheckAutoModuleSettings(applySrcSettings, _textMeshProModule, ref src.modules.textMeshProEnabled);
            CheckAutoModuleSettings(applySrcSettings, _tk2DModule, ref src.modules.tk2DEnabled);
            AssetDatabase.StopAssetEditing();

            EditorUtility.SetDirty(_src);
        }

        static void Apply()
        {
            AssetDatabase.StartAssetEditing();
            ToggleModule(_audioModule);
            ToggleModule(_physicsModule);
            ToggleModule(_physics2DModule);
            ToggleModule(_spriteModule);
            ToggleModule(_uiModule);

            if (EditorUtils.hasPro) {
                ToggleModule(_textMeshProModule);
                ToggleModule(_tk2DModule);
            }
            AssetDatabase.StopAssetEditing();
        }

        static bool ModuleIsEnabled(ModuleInfo m)
        {
            if (!File.Exists(m.filePath)) return false;

            using (StreamReader sr = new StreamReader(m.filePath)) {
                string line = sr.ReadLine();
                while (line != null) {
                    if (line.EndsWith(ModuleMarkerId) && line.StartsWith("#if")) return line.StartsWith("#if true");
                    line = sr.ReadLine();
                }
            }
            return true;
        }

        static void CheckAutoModuleSettings(bool applySettings, ModuleInfo m, ref bool srcModuleEnabled)
        {
            if (m.enabled != srcModuleEnabled) {
                if (applySettings) {
                    m.enabled = srcModuleEnabled;
                    ToggleModule(m);
                } else {
                    srcModuleEnabled = m.enabled;
                    EditorUtility.SetDirty(_src);
                }
            }
        }

        static void ToggleModule(ModuleInfo m)
        {
            if (!File.Exists(m.filePath)) return;
            if (ModuleIsEnabled(m) == m.enabled) return; // Already set

            _LinesToChange.Clear();
            string[] lines = File.ReadAllLines(m.filePath);
            for (int i = 0; i < lines.Length; ++i) {
                string s = lines[i];
                if (s.EndsWith(ModuleMarkerId) && (m.enabled && s.StartsWith("#if false") || !m.enabled && s.StartsWith("#if true"))) {
                    _LinesToChange.Add(i);
                }
            }
            if (_LinesToChange.Count > 0) {
                using (StreamWriter sw = new StreamWriter(m.filePath)) {
                    for (int i = 0; i < lines.Length; ++i) {
                        string s = lines[i];
                        if (_LinesToChange.Contains(i)) {
                            s = m.enabled ? s.Replace("#if false", "#if true") : s.Replace("#if true", "#if false");
                        }
                        sw.WriteLine(s);
                    }
                }
                AssetDatabase.ImportAsset(EditorUtils.FullPathToADBPath(m.filePath), ImportAssetOptions.Default);
            }

            // Enable/disable conditions inside DOTweenModuleUtils.cs
            if (!File.Exists(_ModuleUtilsPath)) return;
            string marker = m.id + "_MARKER";
            lines = File.ReadAllLines(_ModuleUtilsPath);
            _LinesToChange.Clear();
            for (int i = 0; i < lines.Length; ++i) {
                string s = lines[i];
                if (s.EndsWith(marker) && (m.enabled && s.StartsWith("#if false") || !m.enabled && s.StartsWith("#if true"))) {
                    _LinesToChange.Add(i);
                }
            }
            if (_LinesToChange.Count > 0) {
                using (StreamWriter sw = new StreamWriter(_ModuleUtilsPath)) {
                    for (int i = 0; i < lines.Length; ++i) {
                        string s = lines[i];
                        if (_LinesToChange.Contains(i)) {
                            s = m.enabled ? s.Replace("#if false", "#if true") : s.Replace("#if true", "#if false");
                        }
                        sw.WriteLine(s);
                    }
                }
                AssetDatabase.ImportAsset(EditorUtils.FullPathToADBPath(_ModuleUtilsPath), ImportAssetOptions.Default);
            }

            _LinesToChange.Clear();
        }

        #endregion

        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        class ModuleInfo
        {
            public bool enabled;
            public string filePath;
            public readonly string id; // ID is used exclusively with DOTweenModuleUtils, to determine if the line is related to this module

            public ModuleInfo(string filePath, string id)
            {
                this.filePath = filePath;
                this.id = id;
            }
        }
    }
}