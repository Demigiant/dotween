// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/12/24 13:37

using System.IO;
using System.Reflection;
using DG.DOTweenEditor.Core;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    public class UtilityWindowModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        // Checks if deleted folder contains DOTween Pro and in case removes scripting define symbols
        static AssetDeleteResult OnWillDeleteAsset(string asset, RemoveAssetOptions options)
        {
            // Check if asset is a directory
            string dir = EditorUtils.ADBPathToFullPath(asset);
            if (!Directory.Exists(dir)) return AssetDeleteResult.DidNotDelete;
            // Check if directory contains DOTween.dll
            string[] files = Directory.GetFiles(dir, "DOTween.dll", SearchOption.AllDirectories);
            int len = files.Length;
            bool containsDOTween = false;
            for (int i = 0; i < len; ++i) {
                if (!files[i].EndsWith("DOTween.dll")) continue;
                containsDOTween = true;
                break;
            }
            if (!containsDOTween) return AssetDeleteResult.DidNotDelete;
            // DOTween found: remove scripting define symbols
            EditorUtils.RemoveGlobalDefine(DOTweenSetupMenuItem.GlobalDefine_AudioModule);
            EditorUtils.RemoveGlobalDefine(DOTweenSetupMenuItem.GlobalDefine_PhysicsModule);
            EditorUtils.RemoveGlobalDefine(DOTweenSetupMenuItem.GlobalDefine_Physics2DModule);
            EditorUtils.RemoveGlobalDefine(DOTweenSetupMenuItem.GlobalDefine_SpriteModule);
            EditorUtils.RemoveGlobalDefine(DOTweenSetupMenuItem.GlobalDefine_UIModule);
            EditorUtils.RemoveGlobalDefine(DOTweenSetupMenuItem.GlobalDefine_TK2D);
            EditorUtils.RemoveGlobalDefine(DOTweenSetupMenuItem.GlobalDefine_TextMeshPro);
            EditorUtils.RemoveGlobalDefine(DOTweenSetupMenuItem.GlobalDefine_Legacy_NoRigidbody);
            // Remove EditorPrefs
            EditorPrefs.DeleteKey(Application.dataPath + DOTweenUtilityWindow.Id);
            EditorPrefs.DeleteKey(Application.dataPath + DOTweenUtilityWindow.IdPro);
            //
            EditorUtility.DisplayDialog("DOTween Deleted", "DOTween was deleted and all of its scripting define symbols removed.\nThis might show an error depending on your previous setup. If this happens, please close and reopen Unity or reimport DOTween.", "Ok");
            return AssetDeleteResult.DidNotDelete;
        }
    }

    public class UtilityWindowPostProcessor : AssetPostprocessor
    {
        static bool _setupDialogRequested; // Used to prevent OnPostProcessAllAssets firing twice (because of a Unity bug/feature)

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (_setupDialogRequested) return;

            string[] dotweenEntries = System.Array.FindAll(importedAssets, name => name.Contains("DOTween") && !name.EndsWith(".meta") && !name.EndsWith(".jpg") && !name.EndsWith(".png"));
            bool dotweenImported = dotweenEntries.Length > 0;
            if (dotweenImported) {
                // Delete old DOTween files
                EditorUtils.DeleteLegacyNoModulesDOTweenFiles();
                // Delete old DemiLib configuration
                EditorUtils.DeleteOldDemiLibCore();
                // Remove old NoRigidbody define
                EditorUtils.RemoveGlobalDefine(DOTweenSetupMenuItem.GlobalDefine_Legacy_NoRigidbody);
                //
                bool openSetupDialog = EditorPrefs.GetString(Application.dataPath + DOTweenUtilityWindow.Id) != Application.dataPath + DOTween.Version
                                       || EditorPrefs.GetString(Application.dataPath + DOTweenUtilityWindow.IdPro) != Application.dataPath + EditorUtils.proVersion;
                if (openSetupDialog) {
                    Debug.Log("Should open setup dialogue");
                    _setupDialogRequested = true;
                    EditorPrefs.SetString(Application.dataPath + DOTweenUtilityWindow.Id, Application.dataPath + DOTween.Version);
                    EditorPrefs.SetString(Application.dataPath + DOTweenUtilityWindow.IdPro, Application.dataPath + EditorUtils.proVersion);
                    EditorUtility.DisplayDialog("DOTween", "New version of DOTween imported.\n\nSelect \"Tools > Demigiant > DOTween Utility Panel\" and press \"Setup DOTween...\" in the window that opens to set it up.", "Ok");
//                    EditorUtility.DisplayDialog("DOTween", "New version of DOTween imported.\n\nUse the Setup Panel to add/remove its Modules.", "Ok");
                    // Opening window after a postProcess doesn't work on Unity 3 so check that
                    string[] vs = Application.unityVersion.Split("."[0]);
                    int majorVersion = System.Convert.ToInt32(vs[0]);
                    if (majorVersion >= 4) EditorUtils.DelayedCall(0.5f, DOTweenUtilityWindow.Open);
                    EditorUtils.DelayedCall(8, ()=> _setupDialogRequested = false);
                }
            }
        }
    }

    class DOTweenUtilityWindow : EditorWindow
    {
        [MenuItem("Tools/Demigiant/" + _Title)]
        static void ShowWindow() { Open(); }
		
        const string _Title = "DOTween Utility Panel";
        static readonly Vector2 _WinSize = new Vector2(370,490);
        public const string Id = "DOTweenVersion";
        public const string IdPro = "DOTweenProVersion";
        static readonly float _HalfBtSize = _WinSize.x * 0.5f - 6;

        DOTweenSettings _src;
        Texture2D _headerImg, _footerImg;
        Vector2 _headerSize, _footerSize;
        string _innerTitle;
        bool _setupRequired;

        int _selectedTab;
        bool _isModulesMode;
        string[] _tabLabels = new[] { "Setup", "Preferences" };
        string[] _settingsLocation = new[] {"Assets > Resources", "DOTween > Resources", "Demigiant > Resources"};

        // If force is FALSE opens the window only if DOTween's version has changed
        // (set to FALSE by OnPostprocessAllAssets)
        public static void Open()
        {
            EditorWindow window = EditorWindow.GetWindow<DOTweenUtilityWindow>(true, _Title, true);
            window.minSize = _WinSize;
            window.maxSize = _WinSize;
            window.ShowUtility();
            EditorPrefs.SetString(Id, DOTween.Version);
            EditorPrefs.SetString(IdPro, EditorUtils.proVersion);
        }

        // ===================================================================================
        // UNITY METHODS ---------------------------------------------------------------------

        void OnHierarchyChange()
        { Repaint(); }

        void OnEnable()
        {
#if COMPATIBLE
            _innerTitle = "DOTween v" + DOTween.Version + " [Compatibility build]";
#else
            _innerTitle = "DOTween v" + DOTween.Version + (DOTween.isDebugBuild ? " [Debug build]" : " [Release build]");
#endif
            if (EditorUtils.hasPro) _innerTitle += "\nDOTweenPro v" + EditorUtils.proVersion;
            else _innerTitle += "\nDOTweenPro not installed";

            if (_headerImg == null) {
                _headerImg = AssetDatabase.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + "Imgs/Header.jpg", typeof(Texture2D)) as Texture2D;
                EditorUtils.SetEditorTexture(_headerImg, FilterMode.Bilinear, 512);
                _headerSize.x = _WinSize.x;
                _headerSize.y = (int)((_WinSize.x * _headerImg.height) / _headerImg.width);
                _footerImg = AssetDatabase.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + (EditorGUIUtility.isProSkin ? "Imgs/Footer.png" : "Imgs/Footer_dark.png"), typeof(Texture2D)) as Texture2D;
                EditorUtils.SetEditorTexture(_footerImg, FilterMode.Bilinear, 256);
                _footerSize.x = _WinSize.x;
                _footerSize.y = (int)((_WinSize.x * _footerImg.height) / _footerImg.width);
            }

            _setupRequired = EditorUtils.DOTweenSetupRequired();
        }

        void OnDisable()
        {
            _isModulesMode = false;
        }

        void OnGUI()
        {
            Connect();
            EditorGUIUtils.SetGUIStyles(_footerSize);

            if (Application.isPlaying) {
                GUILayout.Space(40);
                GUILayout.BeginHorizontal();
                GUILayout.Space(40);
                GUILayout.Label("DOTween Utility Panel\nis disabled while in Play Mode", EditorGUIUtils.wrapCenterLabelStyle, GUILayout.ExpandWidth(true));
                GUILayout.Space(40);
                GUILayout.EndHorizontal();
            } else {
                if (_isModulesMode) {
                    if (DOTweenModulesSetupGUI.Draw()) _isModulesMode = false;
                } else {
                    Rect areaRect = new Rect(0, 0, _headerSize.x, 30);
                    _selectedTab = GUI.Toolbar(areaRect, _selectedTab, _tabLabels);

                    switch (_selectedTab) {
                    case 1:
                        DrawPreferencesGUI();
                        break;
                    default:
                        DrawSetupGUI();
                        break;
                    }
                }
            }
        }

        // ===================================================================================
        // GUI METHODS -----------------------------------------------------------------------

        void DrawSetupGUI()
        {
            Rect areaRect = new Rect(0, 30, _headerSize.x, _headerSize.y);
            GUI.DrawTexture(areaRect, _headerImg, ScaleMode.StretchToFill, false);
            GUILayout.Space(areaRect.y + _headerSize.y + 2);
            GUILayout.Label(_innerTitle, DOTween.isDebugBuild ? EditorGUIUtils.redLabelStyle : EditorGUIUtils.boldLabelStyle);

            if (_setupRequired) {
                GUI.backgroundColor = Color.red;
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("DOTWEEN SETUP REQUIRED", EditorGUIUtils.setupLabelStyle);
                GUILayout.EndVertical();
                GUI.backgroundColor = Color.white;
            } else GUILayout.Space(8);
            if (GUILayout.Button("Setup DOTween...", EditorGUIUtils.btStyle)) {
//                DOTweenSetupMenuItem.Setup();
//                _setupRequired = EditorUtils.DOTweenSetupRequired();
                DOTweenModulesSetupGUI.Refresh();
                _isModulesMode = true;
            }

//            EditorGUILayout.HelpBox(
//                "NOTE: if you get \"Requested build target group (N) doesn't exist\" or [CS0618] errors during the setup don't worry: it's ok and allows the setup to work on all possible Unity versions",
//                MessageType.Info
//            );

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Documentation", EditorGUIUtils.btStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/documentation.php");
            if (GUILayout.Button("Support", EditorGUIUtils.btStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/support.php");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Changelog", EditorGUIUtils.btStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/download.php");
            if (GUILayout.Button("Check Updates", EditorGUIUtils.btStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/download.php?v=" + DOTween.Version);
            GUILayout.EndHorizontal();
            GUILayout.Space(14);
            if (GUILayout.Button(_footerImg, EditorGUIUtils.btImgStyle)) Application.OpenURL("http://www.demigiant.com/");
        }

        void DrawPreferencesGUI()
        {
            GUILayout.Space(40);
            if (GUILayout.Button("Reset", EditorGUIUtils.btStyle)) {
                // Reset to original defaults
                _src.useSafeMode = true;
                _src.showUnityEditorReport = false;
                _src.timeScale = 1;
                _src.useSmoothDeltaTime = false;
                _src.maxSmoothUnscaledTime = 0.15f;
                _src.rewindCallbackMode = RewindCallbackMode.FireIfPositionChanged;
                _src.logBehaviour = LogBehaviour.ErrorsOnly;
                _src.drawGizmos = true;
                _src.defaultRecyclable = false;
                _src.defaultAutoPlay = AutoPlay.All;
                _src.defaultUpdateType = UpdateType.Normal;
                _src.defaultTimeScaleIndependent = false;
                _src.defaultEaseType = Ease.OutQuad;
                _src.defaultEaseOvershootOrAmplitude = 1.70158f;
                _src.defaultEasePeriod = 0;
                _src.defaultAutoKill = true;
                _src.defaultLoopType = LoopType.Restart;
                EditorUtility.SetDirty(_src);
            }
            GUILayout.Space(8);
            _src.useSafeMode = EditorGUILayout.Toggle("Safe Mode", _src.useSafeMode);
            _src.timeScale = EditorGUILayout.FloatField("DOTween's TimeScale", _src.timeScale);
            _src.useSmoothDeltaTime = EditorGUILayout.Toggle("Smooth DeltaTime", _src.useSmoothDeltaTime);
            _src.maxSmoothUnscaledTime = EditorGUILayout.Slider("Max SmoothUnscaledTime", _src.maxSmoothUnscaledTime, 0.01f, 1f);
            _src.rewindCallbackMode = (RewindCallbackMode)EditorGUILayout.EnumPopup("OnRewind Callback Mode", _src.rewindCallbackMode);
            GUILayout.Space(-5);
            GUILayout.BeginHorizontal();
                GUILayout.Space(154);
                EditorGUILayout.HelpBox(
                    _src.rewindCallbackMode == RewindCallbackMode.FireIfPositionChanged
                        ? "When calling Rewind or PlayBackwards/SmoothRewind, OnRewind callbacks will be fired only if the tween isn't already rewinded"
                        : _src.rewindCallbackMode == RewindCallbackMode.FireAlwaysWithRewind
                            ? "When calling Rewind, OnRewind callbacks will always be fired, even if the tween is already rewinded."
                            : "When calling Rewind or PlayBackwards/SmoothRewind, OnRewind callbacks will always be fired, even if the tween is already rewinded",
                    MessageType.None
                );
            GUILayout.EndHorizontal();
            _src.showUnityEditorReport = EditorGUILayout.Toggle("Editor Report", _src.showUnityEditorReport);
            _src.logBehaviour = (LogBehaviour)EditorGUILayout.EnumPopup("Log Behaviour", _src.logBehaviour);
            _src.drawGizmos = EditorGUILayout.Toggle("Draw Path Gizmos", _src.drawGizmos);
            DOTweenSettings.SettingsLocation prevSettingsLocation = _src.storeSettingsLocation;
            _src.storeSettingsLocation = (DOTweenSettings.SettingsLocation)EditorGUILayout.Popup("Settings Location", (int)_src.storeSettingsLocation, _settingsLocation);
            if (_src.storeSettingsLocation != prevSettingsLocation) {
                if (_src.storeSettingsLocation == DOTweenSettings.SettingsLocation.DemigiantDirectory && EditorUtils.demigiantDir == null) {
                    EditorUtility.DisplayDialog("Change DOTween Settings Location", "Demigiant directory not present (must be the parent of DOTween's directory)", "Ok");
                    if (prevSettingsLocation == DOTweenSettings.SettingsLocation.DemigiantDirectory) {
                        _src.storeSettingsLocation = DOTweenSettings.SettingsLocation.AssetsDirectory;
                        Connect(true);
                    } else _src.storeSettingsLocation = prevSettingsLocation;
                } else Connect(true);
            }
            GUILayout.Space(8);
            GUILayout.Label("DEFAULTS ▼");
            _src.defaultRecyclable = EditorGUILayout.Toggle("Recycle Tweens", _src.defaultRecyclable);
            _src.defaultAutoPlay = (AutoPlay)EditorGUILayout.EnumPopup("AutoPlay", _src.defaultAutoPlay);
            _src.defaultUpdateType = (UpdateType)EditorGUILayout.EnumPopup("Update Type", _src.defaultUpdateType);
            _src.defaultTimeScaleIndependent = EditorGUILayout.Toggle("TimeScale Independent", _src.defaultTimeScaleIndependent);
            _src.defaultEaseType = (Ease)EditorGUILayout.EnumPopup("Ease", _src.defaultEaseType);
            _src.defaultEaseOvershootOrAmplitude = EditorGUILayout.FloatField("Ease Overshoot", _src.defaultEaseOvershootOrAmplitude);
            _src.defaultEasePeriod = EditorGUILayout.FloatField("Ease Period", _src.defaultEasePeriod);
            _src.defaultAutoKill = EditorGUILayout.Toggle("AutoKill", _src.defaultAutoKill);
            _src.defaultLoopType = (LoopType)EditorGUILayout.EnumPopup("Loop Type", _src.defaultLoopType);

            if (GUI.changed) EditorUtility.SetDirty(_src);
        }

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        void Connect(bool forceReconnect = false)
        {
            if (_src != null && !forceReconnect) return;

            LocationData assetsLD = new LocationData(EditorUtils.assetsPath + EditorUtils.pathSlash + "Resources");
            LocationData dotweenLD = new LocationData(EditorUtils.dotweenDir + "Resources");
            bool hasDemigiantDir = EditorUtils.demigiantDir != null;
            LocationData demigiantLD = hasDemigiantDir ? new LocationData(EditorUtils.demigiantDir + "Resources") : new LocationData();

            if (_src == null) {
                // Load eventual existing settings
                _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(assetsLD.adbFilePath, false);
                if (_src == null) _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(dotweenLD.adbFilePath, false);
                if (_src == null && hasDemigiantDir) _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(demigiantLD.adbFilePath, false);
            }
            if (_src == null) {
                // Settings don't exist. Create it in external folder
                if (!Directory.Exists(assetsLD.dir)) AssetDatabase.CreateFolder(assetsLD.adbParentDir, "Resources");
                _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(assetsLD.adbFilePath, true);
            }

            // Move eventual settings from previous location and setup everything correctly
            DOTweenSettings.SettingsLocation settingsLoc = _src.storeSettingsLocation;
            switch (settingsLoc) {
            case DOTweenSettings.SettingsLocation.AssetsDirectory:
                MoveSrc(new[] { dotweenLD, demigiantLD }, assetsLD);
                break;
            case DOTweenSettings.SettingsLocation.DOTweenDirectory:
                MoveSrc(new[] { assetsLD, demigiantLD }, dotweenLD);
                break;
            case DOTweenSettings.SettingsLocation.DemigiantDirectory:
                MoveSrc(new[] { assetsLD, dotweenLD }, demigiantLD);
                break;
            }
        }

        void MoveSrc(LocationData[] from, LocationData to)
        {
            if (!Directory.Exists(to.dir)) AssetDatabase.CreateFolder(to.adbParentDir, "Resources");
            foreach (LocationData ld in from) {
                if (File.Exists(ld.filePath)) {
                    // Move external src file to correct folder
                    AssetDatabase.MoveAsset(ld.adbFilePath, to.adbFilePath);
                    // Delete external settings
                    AssetDatabase.DeleteAsset(ld.adbFilePath);
                    // Check if external Resources folder is empty and in case delete it
                    if (Directory.GetDirectories(ld.dir).Length == 0 && Directory.GetFiles(ld.dir).Length == 0) {
                        AssetDatabase.DeleteAsset(EditorUtils.FullPathToADBPath(ld.dir));
                    }
                }
            }
            _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(to.adbFilePath, true);
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // ||| INTERNAL CLASSES ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        struct LocationData
        {
            public string dir; // without final slash
            public string filePath;
            public string adbFilePath;
            public string adbParentDir; // without final slash

            public LocationData(string srcDir) : this()
            {
                dir = srcDir;
                filePath = dir + EditorUtils.pathSlash + DOTweenSettings.AssetName + ".asset";
                adbFilePath = EditorUtils.FullPathToADBPath(filePath);
                adbParentDir = EditorUtils.FullPathToADBPath(dir.Substring(0, dir.LastIndexOf(EditorUtils.pathSlash)));
            }
        }
    }
}