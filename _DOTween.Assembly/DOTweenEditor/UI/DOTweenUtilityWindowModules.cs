// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/07/14 18:56
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System.Collections.Generic;
using System.IO;
using System.Text;
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
        static readonly ModuleInfo _deAudioModule = new ModuleInfo("DOTweenDeAudio.cs", "DEAUDIO");
        static readonly ModuleInfo _deUnityExtendedModule = new ModuleInfo("DOTweenDeUnityExtended.cs", "DEUNITYEXTENDED");
        static readonly ModuleInfo _dotweenProModule = new ModuleInfo(null, "DOTWEENPRO");
        // External assets included in free version
        static readonly ModuleInfo _epoOutlineModule = new ModuleInfo("Modules/DOTweenModuleEPOOutline.cs", "EPO_DOTWEEN");

        // Files that contain multiple module dependencies and which have specific define markers to change
        static readonly string[] _ModuleDependentFiles = new[] {
            "DOTWEENDIR/Modules/DOTweenModuleUtils.cs",
            "DOTWEENPRODIR/DOTweenAnimation.cs",
            "DOTWEENPRODIR/DOTweenProShortcuts.cs",
            "DOTWEENPRODIR/Editor/DOTweenAnimationInspector.cs",
            "DOTWEENTIMELINEDIR/Scripts/Core/Plugins/DefaultActionPlugins.cs",
            "DOTWEENTIMELINEDIR/Scripts/Core/Plugins/DefaultTweenPlugins.cs",
            "DOTWEENTIMELINEDIR/Scripts/Core/Plugins/OptionalPlugins.cs"
        };

        static EditorWindow _editor;
        static DOTweenSettings _src;
        static bool _refreshed;
        static bool _isWaitingForCompilation;
        static readonly List<int> _LinesToChange = new List<int>();

        static DOTweenUtilityWindowModules()
        {
            for (int i = 0; i < _ModuleDependentFiles.Length; ++i) {
                _ModuleDependentFiles[i] = _ModuleDependentFiles[i].Replace("DOTWEENDIR/", EditorUtils.dotweenDir);
                _ModuleDependentFiles[i] = _ModuleDependentFiles[i].Replace("DOTWEENPRODIR/", EditorUtils.dotweenProDir);
                _ModuleDependentFiles[i] = _ModuleDependentFiles[i].Replace("DOTWEENTIMELINEDIR/", EditorUtils.dotweenTimelineDir);
                _ModuleDependentFiles[i] = _ModuleDependentFiles[i].Replace(EditorUtils.pathSlashToReplace, EditorUtils.pathSlash);
            }

            _audioModule.filePath = EditorUtils.dotweenDir + _audioModule.filePath;
            _physicsModule.filePath = EditorUtils.dotweenDir + _physicsModule.filePath;
            _physics2DModule.filePath = EditorUtils.dotweenDir + _physics2DModule.filePath;
            _spriteModule.filePath = EditorUtils.dotweenDir + _spriteModule.filePath;
            _uiModule.filePath = EditorUtils.dotweenDir + _uiModule.filePath;
            _textMeshProModule.filePath = EditorUtils.dotweenProDir + _textMeshProModule.filePath;
            _tk2DModule.filePath = EditorUtils.dotweenProDir + _tk2DModule.filePath;
            _deAudioModule.filePath = EditorUtils.dotweenProDir + _deAudioModule.filePath;
            _deUnityExtendedModule.filePath = EditorUtils.dotweenProDir + _deUnityExtendedModule.filePath;
            _epoOutlineModule.filePath = EditorUtils.dotweenDir + _epoOutlineModule.filePath;
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
            GUILayout.EndVertical();
            // External assets modules - free
            GUILayout.BeginVertical(UnityEngine.GUI.skin.box);
            GUILayout.Label("External Assets", EditorGUIUtils.boldLabelStyle);
            EditorGUILayout.HelpBox(
                "These modules are for external Unity assets." +
                "\nDO NOT activate them unless you have the relative asset in your project.",
                MessageType.Warning
            );
            GUILayout.BeginVertical(UnityEngine.GUI.skin.box);
            GUILayout.Label("DOTween Free/Core", EditorGUIUtils.boldLabelStyle);
            _epoOutlineModule.enabled = EditorGUILayout.Toggle("Easy Performant Outline", _epoOutlineModule.enabled);
            GUILayout.EndVertical();
            // Pro modules
            using (new EditorGUI.DisabledScope(!EditorUtils.hasPro && !EditorUtils.hasDOTweenTimeline)) {
                GUILayout.BeginVertical(UnityEngine.GUI.skin.box);
                GUILayout.Label("DOTween Pro / DOTween Timeline", EditorGUIUtils.boldLabelStyle);
                _deAudioModule.enabled = EditorGUILayout.Toggle("DeAudio", _deAudioModule.enabled);
                _deUnityExtendedModule.enabled = EditorGUILayout.Toggle("DeUnityExtended", _deUnityExtendedModule.enabled);
                _textMeshProModule.enabled = EditorGUILayout.Toggle("TextMesh Pro", _textMeshProModule.enabled);
                _tk2DModule.enabled = EditorGUILayout.Toggle("2D Toolkit (legacy)", _tk2DModule.enabled);
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();

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

        #region Public Methods

        // Also called via Reflection by Autorun
        public static void ApplyModulesSettings()
        {
            DOTweenSettings src = DOTweenUtilityWindow.GetDOTweenSettings();
            if (src != null) Refresh(src, true);
        }

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
            _deAudioModule.enabled = ModuleIsEnabled(_deAudioModule);
            _deUnityExtendedModule.enabled = ModuleIsEnabled(_deUnityExtendedModule);
            _dotweenProModule.enabled = ModuleIsEnabled(_dotweenProModule);
            //
            _epoOutlineModule.enabled = ModuleIsEnabled(_epoOutlineModule);

            CheckAutoModuleSettings(applySrcSettings, _audioModule, ref src.modules.audioEnabled);
            CheckAutoModuleSettings(applySrcSettings, _physicsModule, ref src.modules.physicsEnabled);
            CheckAutoModuleSettings(applySrcSettings, _physics2DModule, ref src.modules.physics2DEnabled);
            CheckAutoModuleSettings(applySrcSettings, _spriteModule, ref src.modules.spriteEnabled);
            CheckAutoModuleSettings(applySrcSettings, _uiModule, ref src.modules.uiEnabled);
            //
            CheckAutoModuleSettings(applySrcSettings, _textMeshProModule, ref src.modules.textMeshProEnabled);
            CheckAutoModuleSettings(applySrcSettings, _tk2DModule, ref src.modules.tk2DEnabled);
            CheckAutoModuleSettings(applySrcSettings, _deAudioModule, ref src.modules.deAudioEnabled);
            CheckAutoModuleSettings(applySrcSettings, _deUnityExtendedModule, ref src.modules.deUnityExtendedEnabled);
            //
            CheckAutoModuleSettings(applySrcSettings, _epoOutlineModule, ref src.modules.epoOutlineEnabled);
            // Not dependent by DOTween Preferences but by simple presence of other assets
            bool proEnabled = EditorUtils.hasPro;
            CheckAutoModuleSettings(applySrcSettings, _dotweenProModule, ref proEnabled);

            AssetDatabase.StopAssetEditing();

            EditorUtility.SetDirty(_src);
        }

        #endregion

        #region Methods

        static void Apply()
        {
            AssetDatabase.StartAssetEditing();
            bool audioToggled = ToggleModule(_audioModule, ref _src.modules.audioEnabled);
            bool physicsToggled = ToggleModule(_physicsModule, ref _src.modules.physicsEnabled);
            bool physics2DToggled = ToggleModule(_physics2DModule, ref _src.modules.physics2DEnabled);
            bool spriteToggled = ToggleModule(_spriteModule, ref _src.modules.spriteEnabled);
            bool uiToggled = ToggleModule(_uiModule, ref _src.modules.uiEnabled);

            bool textMeshProToggled = false;
            bool tk2DToggled = false;
            bool deAudioToggled = false;
            bool deUnityExtendedToggled = false;
            if (EditorUtils.hasPro || EditorUtils.hasDOTweenTimeline) {
                textMeshProToggled = ToggleModule(_textMeshProModule, ref _src.modules.textMeshProEnabled);
                tk2DToggled = ToggleModule(_tk2DModule, ref _src.modules.tk2DEnabled);
                deAudioToggled = ToggleModule(_deAudioModule, ref _src.modules.deAudioEnabled);
                deUnityExtendedToggled = ToggleModule(_deUnityExtendedModule, ref _src.modules.deUnityExtendedEnabled);
            }

            bool epoOutlineToggled = ToggleModule(_epoOutlineModule, ref _src.modules.epoOutlineEnabled);

            AssetDatabase.StopAssetEditing();
            EditorUtility.SetDirty(_src);

            bool anyToggled = audioToggled || physicsToggled || physics2DToggled || spriteToggled || uiToggled
                              || textMeshProToggled || tk2DToggled || deAudioToggled || deUnityExtendedToggled
                              || epoOutlineToggled;
            if (anyToggled) {
                StringBuilder strb = new StringBuilder();
                strb.Append("<b>DOTween module files modified ► </b>");
                if (audioToggled) Apply_AppendLog(strb, _src.modules.audioEnabled, "Audio");
                if (physicsToggled) Apply_AppendLog(strb, _src.modules.physicsEnabled, "Physics");
                if (physics2DToggled) Apply_AppendLog(strb, _src.modules.physics2DEnabled, "Physics2D");
                if (spriteToggled) Apply_AppendLog(strb, _src.modules.spriteEnabled, "Sprites");
                if (uiToggled) Apply_AppendLog(strb, _src.modules.uiEnabled, "UI");
                if (textMeshProToggled) Apply_AppendLog(strb, _src.modules.textMeshProEnabled, "TextMesh Pro");
                if (tk2DToggled) Apply_AppendLog(strb, _src.modules.tk2DEnabled, "2D Toolkit");
                if (deAudioToggled) Apply_AppendLog(strb, _src.modules.deAudioEnabled, "DeAudio");
                if (deUnityExtendedToggled) Apply_AppendLog(strb, _src.modules.deUnityExtendedEnabled, "DeUnityExtended");
                if (epoOutlineToggled) Apply_AppendLog(strb, _src.modules.epoOutlineEnabled, "Easy Performant Outline");
                // Remove last divider
                strb.Remove(strb.Length - 3, 3);
                Debug.Log(strb.ToString());
            }

            ASMDEFManager.RefreshExistingASMDEFFiles();
        }

        static void Apply_AppendLog(StringBuilder strb, bool enabled, string id)
        {
            strb.Append("<color=#").Append(enabled ? "00ff00" : "ff0000").Append('>').Append(id).Append("</color>").Append(" - ");
        }

        static bool ModuleIsEnabled(ModuleInfo m)
        {
            bool result = false;
            bool hasMainFile = !string.IsNullOrEmpty(m.filePath) && File.Exists(m.filePath);
            if (hasMainFile) {
                ModuleIsEnabled_Retrieve(m.filePath, ref result, ModuleMarkerId);
                return result;
            } else {
                // Main module file doesn't exist: look in dependent files to find the first specific marker line
                for (int i = 0; i < _ModuleDependentFiles.Length; ++i) {
                    if (!File.Exists(_ModuleDependentFiles[i])) continue;
                    if (ModuleIsEnabled_Retrieve(_ModuleDependentFiles[i], ref result, string.Format("// {0}_MARKER", m.id))) break;
                }
            }
            return result;
        }
        // Returns TRUE if a module-relative line was found
        static bool ModuleIsEnabled_Retrieve(string filePath, ref bool isEnabled, string marker)
        {
            using (StreamReader sr = new StreamReader(filePath)) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    if (line.EndsWith(marker) && line.StartsWith("#if")) {
                        isEnabled = line.Contains("true");
                        return true;
                    }
                }
            }
            return false;
        }

        static void CheckAutoModuleSettings(bool applySettings, ModuleInfo m, ref bool srcModuleEnabled)
        {
            bool moduleSettingsToggled = false;
            if (m.enabled != srcModuleEnabled) {
                if (applySettings) {
                    m.enabled = srcModuleEnabled;
                    moduleSettingsToggled = ToggleModule(m, ref srcModuleEnabled);
                } else {
                    srcModuleEnabled = m.enabled;
                    EditorUtility.SetDirty(_src);
                }
            }
            if (applySettings && !moduleSettingsToggled) {
                // If we were applying modifications but nothing changed in the main module file,
                // check the modules dependent files anyway
                string marker = "// " + m.id + "_MARKER";
                for (int i = 0; i < _ModuleDependentFiles.Length; ++i) {
                    ToggleModuleInDependentFile(_ModuleDependentFiles[i], m.enabled, marker);
                }
            }
        }

        // Returns TRUE if files were actually modified
        static bool ToggleModule(ModuleInfo m, ref bool srcSetting)
        {
            srcSetting = m.enabled;
            bool modifiedFiles = false;

            // Toggle full module-based script
            if (File.Exists(m.filePath)) {
                _LinesToChange.Clear();
                string[] lines = File.ReadAllLines(m.filePath);
                for (int i = 0; i < lines.Length; ++i) {
                    string s = lines[i];
                    if (s.EndsWith(ModuleMarkerId) && s.StartsWith("#if") && (m.enabled && s.Contains("false") || !m.enabled && s.Contains("true"))) {
                        _LinesToChange.Add(i);
                    }
                }
                if (_LinesToChange.Count > 0) {
                    modifiedFiles = true;
                    using (StreamWriter sw = new StreamWriter(m.filePath)) {
                        for (int i = 0; i < lines.Length; ++i) {
                            string s = lines[i];
                            if (_LinesToChange.Contains(i)) {
                                s = m.enabled ? s.Replace("false", "true") : s.Replace("true", "false");
                            }
                            sw.WriteLine(s);
                        }
                    }
                    AssetDatabase.ImportAsset(EditorUtils.FullPathToADBPath(m.filePath), ImportAssetOptions.Default);
                }
            }

            // Enable/disable conditions inside dependent files
            string marker = "// " + m.id + "_MARKER";
            for (int i = 0; i < _ModuleDependentFiles.Length; ++i) {
                bool mod = ToggleModuleInDependentFile(_ModuleDependentFiles[i], m.enabled, marker);
                if (mod) modifiedFiles = true;
            }

            _LinesToChange.Clear();
            return modifiedFiles;
        }

        // Returns TRUE if files were actually modified
        static bool ToggleModuleInDependentFile(string filePath, bool enable, string marker)
        {
            if (!File.Exists(filePath)) return false;

            bool modifiedFiles = false;
            _LinesToChange.Clear();
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; ++i) {
                string s = lines[i];
                if (s.EndsWith(marker) && s.StartsWith("#if") && (enable && s.Contains("false") || !enable && s.Contains("true"))) {
                    _LinesToChange.Add(i);
                }
            }
            if (_LinesToChange.Count > 0) {
                modifiedFiles = true;
                using (StreamWriter sw = new StreamWriter(filePath)) {
                    for (int i = 0; i < lines.Length; ++i) {
                        string s = lines[i];
                        if (_LinesToChange.Contains(i)) {
                            s = enable ? s.Replace("false", "true") : s.Replace("true", "false");
                        }
                        sw.WriteLine(s);
                    }
                }
                AssetDatabase.ImportAsset(EditorUtils.FullPathToADBPath(filePath), ImportAssetOptions.Default);
            }
            return modifiedFiles;
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