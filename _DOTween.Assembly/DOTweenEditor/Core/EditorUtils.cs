// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/12/24 13:50

using System;
using System.Collections;
using System.Reflection;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.Core
{
    public static class EditorUtils
    {
        public static string projectPath { get; private set; } // Without final slash
        public static string assetsPath { get; private set; } // Without final slash
        public static bool hasPro { get { if (!_hasCheckedForPro) CheckForPro(); return _hasPro; } }
        public static string proVersion { get { if (!_hasCheckedForPro) CheckForPro(); return _proVersion; } }
        // Editor path from Assets (not included) with final slash, in AssetDatabase format (/)
        public static string editorADBDir { get { if (string.IsNullOrEmpty(_editorADBDir)) StoreEditorADBDir(); return _editorADBDir; } }
        // With final slash (system based) - might be NULL in case users are not using a parent Demigiant folder
        public static string demigiantDir { get { if (string.IsNullOrEmpty(_demigiantDir)) StoreDOTweenDirs(); return _demigiantDir; } }
        // With final slash (system based)
        public static string dotweenDir { get { if (string.IsNullOrEmpty(_dotweenDir)) StoreDOTweenDirs(); return _dotweenDir; } }
        // With final slash (system based)
        public static string dotweenProDir { get { if (string.IsNullOrEmpty(_dotweenProDir)) StoreDOTweenDirs(); return _dotweenProDir; } }
        public static bool isOSXEditor { get; private set; }
        public static string pathSlash { get; private set; } // for full paths
        public static string pathSlashToReplace { get; private set; } // for full paths

        static bool _hasPro;
        static string _proVersion;
        static bool _hasCheckedForPro;
        static string _editorADBDir;
        static string _demigiantDir; // with final slash
        static string _dotweenDir; // with final slash
        static string _dotweenProDir; // with final slash

        static EditorUtils()
        {
            isOSXEditor = Application.platform == RuntimePlatform.OSXEditor;
            bool useWindowsSlashes = Application.platform == RuntimePlatform.WindowsEditor;
            pathSlash = useWindowsSlashes ? "\\" : "/";
            pathSlashToReplace = useWindowsSlashes ? "/" : "\\";

            projectPath = Application.dataPath;
            projectPath = projectPath.Substring(0, projectPath.LastIndexOf("/"));
            projectPath = projectPath.Replace(pathSlashToReplace, pathSlash);

            assetsPath = projectPath + pathSlash + "Assets";
        }

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        public static void DelayedCall(float delay, Action callback)
        {
            new DelayedCall(delay, callback);
        }

        /// <summary>
        /// Checks that the given editor texture use the correct import settings,
        /// and applies them if they're incorrect.
        /// </summary>
        public static void SetEditorTexture(Texture2D texture, FilterMode filterMode = FilterMode.Point, int maxTextureSize = 32)
        {
            if (texture.wrapMode == TextureWrapMode.Clamp) return;

            string path = AssetDatabase.GetAssetPath(texture);
            TextureImporter tImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            tImporter.textureType = TextureImporterType.GUI;
            tImporter.npotScale = TextureImporterNPOTScale.None;
            tImporter.filterMode = filterMode;
            tImporter.wrapMode = TextureWrapMode.Clamp;
            tImporter.maxTextureSize = maxTextureSize;
            tImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
            AssetDatabase.ImportAsset(path);
        }

        /// <summary>
        /// Returns TRUE if addons setup is required.
        /// </summary>
        public static bool DOTweenSetupRequired()
        {
            if (!Directory.Exists(dotweenDir)) return false; // Can happen if we were deleting DOTween
            return Directory.GetFiles(dotweenDir, "*.addon").Length > 0 || hasPro && Directory.GetFiles(dotweenProDir, "*.addon").Length > 0;
        }

        /// <summary>
        /// Returns TRUE if the file/directory at the given path exists.
        /// </summary>
        /// <param name="adbPath">Path, relative to Unity's project folder</param>
        /// <returns></returns>
        public static bool AssetExists(string adbPath)
        {
            string fullPath = ADBPathToFullPath(adbPath);
            return File.Exists(fullPath) || Directory.Exists(fullPath);
        }

        /// <summary>
        /// Converts the given project-relative path to a full path,
        /// with backward (\) slashes).
        /// </summary>
        public static string ADBPathToFullPath(string adbPath)
        {
            adbPath = adbPath.Replace(pathSlashToReplace, pathSlash);
            return projectPath + pathSlash + adbPath;
        }

        /// <summary>
        /// Converts the given full path to a path usable with AssetDatabase methods
        /// (relative to Unity's project folder, and with the correct Unity forward (/) slashes).
        /// </summary>
        public static string FullPathToADBPath(string fullPath)
        {
            string adbPath = fullPath.Substring(projectPath.Length + 1);
            return adbPath.Replace("\\", "/");
        }

        /// <summary>
        /// Connects to a <see cref="ScriptableObject"/> asset.
        /// If the asset already exists at the given path, loads it and returns it.
        /// Otherwise, either returns NULL or automatically creates it before loading and returning it
        /// (depending on the given parameters).
        /// </summary>
        /// <typeparam name="T">Asset type</typeparam>
        /// <param name="adbFilePath">File path (relative to Unity's project folder)</param>
        /// <param name="createIfMissing">If TRUE and the requested asset doesn't exist, forces its creation</param>
        public static T ConnectToSourceAsset<T>(string adbFilePath, bool createIfMissing = false) where T : ScriptableObject
        {
            if (!AssetExists(adbFilePath)) {
                if (createIfMissing) CreateScriptableAsset<T>(adbFilePath);
                else return null;
            }
            T source = (T)AssetDatabase.LoadAssetAtPath(adbFilePath, typeof(T));
            if (source == null) {
                // Source changed (or editor file was moved from outside of Unity): overwrite it
                CreateScriptableAsset<T>(adbFilePath);
                source = (T)AssetDatabase.LoadAssetAtPath(adbFilePath, typeof(T));
            }
            return source;
        }

        /// <summary>
        /// Full path for the given loaded assembly, assembly file included
        /// </summary>
        public static string GetAssemblyFilePath(Assembly assembly)
        {

            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            if (path.Substring(path.Length - 3) == "dll") return path;

//            string codeBase = assembly.CodeBase;
//            UriBuilder uri = new UriBuilder(codeBase);
//            string path = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
//            string lastChar = path.Substring(path.Length - 4);
//            if (lastChar == "dll") return path;

            // Invalid path, use Location
            return Path.GetFullPath(assembly.Location);
        }

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        static void CheckForPro()
        {
            _hasCheckedForPro = true;
            try {
                Assembly additionalAssembly = Assembly.Load("DOTweenPro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                _proVersion = additionalAssembly.GetType("DG.Tweening.DOTweenPro").GetField("Version", BindingFlags.Static | BindingFlags.Public).GetValue(null) as string;
                _hasPro = true;
            } catch {
                // No DOTweenPro present
                _hasPro = false;
                _proVersion = "-";
            }
        }

        // AssetDatabase formatted path to DOTween's Editor folder
        static void StoreEditorADBDir()
        {
//            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
//            UriBuilder uri = new UriBuilder(codeBase);
//            string fullPath = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            string fullPath = Path.GetDirectoryName(GetAssemblyFilePath(Assembly.GetExecutingAssembly()));
            string adbPath = fullPath.Substring(Application.dataPath.Length + 1);
            _editorADBDir = adbPath.Replace("\\", "/") + "/";
        }

        static void StoreDOTweenDirs()
        {
//            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
//            UriBuilder uri = new UriBuilder(codeBase);
//            _dotweenDir = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            _dotweenDir = Path.GetDirectoryName(GetAssemblyFilePath(Assembly.GetExecutingAssembly()));
            string pathSeparator = _dotweenDir.IndexOf("/") != -1 ? "/" : "\\";
            _dotweenDir = _dotweenDir.Substring(0, _dotweenDir.LastIndexOf(pathSeparator) + 1);

            _dotweenProDir = _dotweenDir.Substring(0, _dotweenDir.LastIndexOf(pathSeparator));
            _dotweenProDir = _dotweenProDir.Substring(0, _dotweenProDir.LastIndexOf(pathSeparator) + 1) + "DOTweenPro" + pathSeparator;

            _demigiantDir = _dotweenDir.Substring(0, _dotweenDir.LastIndexOf(pathSeparator));
            _demigiantDir = _demigiantDir.Substring(0, _demigiantDir.LastIndexOf(pathSeparator) + 1);
            if (_demigiantDir.Substring(_demigiantDir.Length - 10, 9) != "Demigiant") _demigiantDir = null;

            _dotweenDir = _dotweenDir.Replace(pathSlashToReplace, pathSlash);
            _dotweenProDir = _dotweenProDir.Replace(pathSlashToReplace, pathSlash);
            if (_demigiantDir != null) _demigiantDir = _demigiantDir.Replace(pathSlashToReplace, pathSlash);
        }

        static void CreateScriptableAsset<T>(string adbFilePath) where T : ScriptableObject
        {
            T data = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(data, adbFilePath);
        }
    }
}