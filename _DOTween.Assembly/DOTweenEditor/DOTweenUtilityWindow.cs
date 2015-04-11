// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/12/24 13:37

using System.IO;
using DG.DOTweenEditor.Core;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    public class UtilityWindowProcessor : AssetPostprocessor
    {
        static bool _setupDialogRequested; // Used to prevent OnPostProcessAllAssets firing twice (because of a Unity bug/feature)

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (_setupDialogRequested) return;

            string[] dotweenEntries = System.Array.FindAll(importedAssets, name => name.Contains("DOTween") && !name.EndsWith(".meta") && !name.EndsWith(".jpg") && !name.EndsWith(".png"));
            bool dotweenImported = dotweenEntries.Length > 0;
            if (dotweenImported) {
                bool openSetupDialog = EditorUtils.DOTweenSetupRequired()
                    && (EditorPrefs.GetString(Application.dataPath + DOTweenUtilityWindow.Id) != Application.dataPath + DOTween.Version
                    || EditorPrefs.GetString(Application.dataPath + DOTweenUtilityWindow.IdPro) != Application.dataPath + EditorUtils.proVersion);
                if (openSetupDialog) {
                    _setupDialogRequested = true;
                    EditorPrefs.SetString(Application.dataPath + DOTweenUtilityWindow.Id, Application.dataPath + DOTween.Version);
                    EditorPrefs.SetString(Application.dataPath + DOTweenUtilityWindow.IdPro, Application.dataPath + EditorUtils.proVersion);
                    EditorUtility.DisplayDialog("DOTween", "DOTween needs to be setup.\n\nSelect \"Tools > DOTween Utility Panel\" and press \"Setup DOTween...\" in the panel that opens.", "Ok");
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
        [MenuItem("Tools/" + _Title)]
        static void ShowWindow() { Open(); }
		
        const string _Title = "DOTween Utility Panel";
        static readonly Vector2 _WinSize = new Vector2(300,365);
        public const string Id = "DOTweenVersion";
        public const string IdPro = "DOTweenProVersion";
        static readonly float _HalfBtSize = _WinSize.x * 0.5f - 6;

        DOTweenSettings _src;
        Texture2D _headerImg, _footerImg;
        Vector2 _headerSize, _footerSize;
        string _innerTitle;
        bool _setupRequired;

        int _selectedTab;
        string[] _tabLabels = new[] { "Setup", "Preferences" };
        string[] _settingsLocation = new[] {"Assets > Resources", "DOTween > Resources"};

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
            _innerTitle = "DOTween v" + DOTween.Version + (DOTween.isDebugBuild ? " [Debug build]" : " [Release build]");
            if (EditorUtils.hasPro) _innerTitle += "\nDOTweenPro v" + EditorUtils.proVersion;
            else _innerTitle += "\nDOTweenPro not installed";

            if (_headerImg == null) {
                _headerImg = Resources.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + "Imgs/Header.jpg", typeof(Texture2D)) as Texture2D;
                EditorUtils.SetEditorTexture(_headerImg, FilterMode.Bilinear, 512);
                _headerSize.x = _WinSize.x;
                _headerSize.y = (int)((_WinSize.x * _headerImg.height) / _headerImg.width);
                _footerImg = Resources.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + (EditorGUIUtility.isProSkin ? "Imgs/Footer.png" : "Imgs/Footer_dark.png"), typeof(Texture2D)) as Texture2D;
                EditorUtils.SetEditorTexture(_footerImg, FilterMode.Bilinear, 256);
                _footerSize.x = _WinSize.x;
                _footerSize.y = (int)((_WinSize.x * _footerImg.height) / _footerImg.width);
            }

            _setupRequired = EditorUtils.DOTweenSetupRequired();
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
                DOTweenSetupMenuItem.Setup();
                _setupRequired = EditorUtils.DOTweenSetupRequired();
            }

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
                _src.logBehaviour = LogBehaviour.ErrorsOnly;
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
            _src.showUnityEditorReport = EditorGUILayout.Toggle("Editor Report", _src.showUnityEditorReport);
            _src.logBehaviour = (LogBehaviour)EditorGUILayout.EnumPopup("Log Behaviour", _src.logBehaviour);
            DOTweenSettings.SettingsLocation prevSettingsLocation = _src.storeSettingsLocation;
            _src.storeSettingsLocation = (DOTweenSettings.SettingsLocation)EditorGUILayout.Popup("Settings Location", (int)_src.storeSettingsLocation, _settingsLocation);
            if (_src.storeSettingsLocation != prevSettingsLocation) Connect(true);
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

            string externalSrcDir = EditorUtils.assetsPath + EditorUtils.pathSlash + "Resources";
            string externalSrcFilePath = externalSrcDir + EditorUtils.pathSlash + DOTweenSettings.AssetName + ".asset";
            string externalAdbSrcFilePath = EditorUtils.FullPathToADBPath(externalSrcFilePath);
            string internalSrcDir = EditorUtils.dotweenDir + "Resources";
            string internalSrcFilePath = internalSrcDir + EditorUtils.pathSlash + DOTweenSettings.AssetName + ".asset";
            string internalAdbSrcFilePath = EditorUtils.FullPathToADBPath(internalSrcFilePath);

            if (_src == null) {
                // Load eventual existing settings
                _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(externalAdbSrcFilePath, false);
                if (_src == null) _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(internalAdbSrcFilePath, false);
            }
            if (_src == null) {
                // Settings don't exist. Create it in external folder
                if (!Directory.Exists(externalSrcDir)) AssetDatabase.CreateFolder("Assets", "Resources");
                _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(externalAdbSrcFilePath, true);
            }

            // Move eventual settings from previous location and setup everything correctly
            DOTweenSettings.SettingsLocation settingsLoc = _src.storeSettingsLocation;
            switch (settingsLoc) {
            case DOTweenSettings.SettingsLocation.AssetsDirectory:
                if (!Directory.Exists(externalSrcDir)) AssetDatabase.CreateFolder("Assets", "Resources");
                if (File.Exists(internalSrcFilePath)) {
                    // Move internal src file to correct folder
                    AssetDatabase.MoveAsset(internalAdbSrcFilePath, externalAdbSrcFilePath);
                    // Delete internal Resources folder
                    AssetDatabase.DeleteAsset(EditorUtils.FullPathToADBPath(internalSrcDir));
                }
                _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(externalAdbSrcFilePath, true);
                break;
            case DOTweenSettings.SettingsLocation.DOTweenDirectory:
                if (!Directory.Exists(internalSrcDir)) AssetDatabase.CreateFolder(EditorUtils.FullPathToADBPath(EditorUtils.dotweenDir.Substring(0, EditorUtils.dotweenDir.LastIndexOf(EditorUtils.pathSlash))), "Resources");
                if (File.Exists(externalSrcFilePath)) {
                    // Move external src file to correct folder
                    AssetDatabase.MoveAsset(externalAdbSrcFilePath, internalAdbSrcFilePath);
                    // Delete external settings
                    AssetDatabase.DeleteAsset(externalAdbSrcFilePath);
                    // Check if external Resources folder is empty and in case delete it
                    if (Directory.GetDirectories(externalSrcDir).Length == 0 && Directory.GetFiles(externalSrcDir).Length == 0) {
                        AssetDatabase.DeleteAsset(EditorUtils.FullPathToADBPath(externalSrcDir));
                    }
                }
                _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(internalAdbSrcFilePath, true);
                break;
            }

//            if (_src == null) {
//                string srcDir = EditorUtils.assetsPath + EditorUtils.pathSlash + "Resources";
//                if (!Directory.Exists(srcDir)) AssetDatabase.CreateFolder("Assets", "Resources");
//                string adbSrcFilePath = EditorUtils.FullPathToADBPath(srcDir + EditorUtils.pathSlash + DOTweenSettings.AssetName + ".asset");
//
//                // Legacy: check if there are settings saved in previous mode and eventually move them
//                string altSrcDir = EditorUtils.dotweenDir + "Resources";
//                string legacySrcFilePath = altSrcDir + EditorUtils.pathSlash + DOTweenSettings.AssetName + ".asset";
//                if (File.Exists(legacySrcFilePath)) {
//                    // Move legacy src file to correct folder
//                    AssetDatabase.MoveAsset(EditorUtils.FullPathToADBPath(legacySrcFilePath), adbSrcFilePath);
//                    // Delete legacy Resources folder
//                    AssetDatabase.DeleteAsset(EditorUtils.FullPathToADBPath(altSrcDir));
//                }
//
//                _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(adbSrcFilePath, true);
//            }
        }
    }
}