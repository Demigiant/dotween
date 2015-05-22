// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/30 11:59
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.IO;
using System.Reflection;
using DG.DOTweenEditor.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    /// <summary>
    /// Not used as menu item anymore, but as a utiity function
    /// </summary>
    class DOTweenSetupMenuItem
    {
//        [MenuItem("Tools/" + _Title)]
//        static void StartSetup() { Setup(); }

        const string _Title = "DOTween Setup";

        /// <summary>
        /// Setups DOTween
        /// </summary>
        /// <param name="partiallySilent">If TRUE, no warning window appears in case there is no need for setup</param>
        public static void Setup(bool partiallySilent = false)
        {
            bool setupRequired = EditorUtils.DOTweenSetupRequired();
            if (setupRequired) {
                string msg = "Based on your Unity version (" + Application.unityVersion + ") and eventual plugins, DOTween will now import additional tween elements, if available.";
                if (!EditorUtility.DisplayDialog(_Title, msg, "Ok", "Cancel")) return;
            } else {
                if (!partiallySilent) {
                    string msg = "This project has already been setup for your version of DOTween.\nUpdate to a new DOTween version before running the setup again.";
                    EditorUtility.DisplayDialog(_Title, msg, "Ok");
                }
                return;
            }

            string addonsDir = EditorUtils.dotweenDir;
            string proAddonsDir = EditorUtils.dotweenProDir;
            string projectPath = Application.dataPath + EditorUtils.pathSlash;

            EditorUtility.DisplayProgressBar(_Title, "Importing additional DOTween elements based on your Unity version and eventual plugins...", 0.25f);

            int totImported = 0;
            // Unity version-based files
            string[] vs = Application.unityVersion.Split("."[0]);
            int majorVersion = Convert.ToInt32(vs[0]);
            int minorVersion = Convert.ToInt32(vs[1]);
            if (majorVersion < 4) {
                SetupComplete(addonsDir, proAddonsDir, totImported);
                return;
            }
            if (majorVersion == 4) {
                if (minorVersion < 3) {
                    SetupComplete(addonsDir, proAddonsDir, totImported);
                    return;
                }
                totImported += ImportAddons("43", addonsDir);
                if (minorVersion >= 6) totImported += ImportAddons("46", addonsDir);
            } else {
                // 5.x
                totImported += ImportAddons("43", addonsDir);
                totImported += ImportAddons("46", addonsDir);
                totImported += ImportAddons("50", addonsDir);
            }
            // Additional plugin files
            // Pro plugins
            if (EditorUtils.hasPro) {
                if (Directory.GetDirectories(EditorUtils.projectPath, "TK2DROOT", SearchOption.AllDirectories).Length > 0) {
                    // PRO > 2DToolkit shortcuts
                    totImported += ImportAddons("Tk2d", proAddonsDir);
                }
                if (Directory.GetDirectories(EditorUtils.projectPath, "TextMesh Pro", SearchOption.AllDirectories).Length > 0) {
                    // PRO > TextMeshPro shortcuts
                    totImported += ImportAddons("TextMeshPro", proAddonsDir);
                }
            }

            SetupComplete(addonsDir, proAddonsDir, totImported);
        }

        static void SetupComplete(string addonsDir, string proAddonsDir, int totImported)
        {

            // Delete all remaining addon files
            string[] leftoverAddonFiles = Directory.GetFiles(addonsDir, "*.addon");
            if (leftoverAddonFiles.Length > 0) {
                EditorUtility.DisplayProgressBar(_Title, "Removing " + leftoverAddonFiles.Length + " unused additional files...", 0.5f);
                foreach (string leftoverAddonFile in leftoverAddonFiles) File.Delete(leftoverAddonFile);
            }
            if (EditorUtils.hasPro) {
                leftoverAddonFiles = Directory.GetFiles(proAddonsDir, "*.addon");
                if (leftoverAddonFiles.Length > 0) {
                    EditorUtility.DisplayProgressBar(_Title, "Removing " + leftoverAddonFiles.Length + " unused additional files...", 0.5f);
                    foreach (string leftoverAddonFile in leftoverAddonFiles) File.Delete(leftoverAddonFile);
                }
            }
            // Delete all remaining addon meta files
            leftoverAddonFiles = Directory.GetFiles(addonsDir, "*.addon.meta");
            if (leftoverAddonFiles.Length > 0) {
                EditorUtility.DisplayProgressBar(_Title, "Removing " + leftoverAddonFiles.Length + " unused additional meta files...", 0.75f);
                foreach (string leftoverAddonFile in leftoverAddonFiles) File.Delete(leftoverAddonFile);
            }
            if (EditorUtils.hasPro) {
                leftoverAddonFiles = Directory.GetFiles(proAddonsDir, "*.addon.meta");
                if (leftoverAddonFiles.Length > 0) {
                    EditorUtility.DisplayProgressBar(_Title, "Removing " + leftoverAddonFiles.Length + " unused additional meta files...", 0.75f);
                    foreach (string leftoverAddonFile in leftoverAddonFiles) File.Delete(leftoverAddonFile);
                }
            }

            EditorUtility.DisplayProgressBar(_Title, "Refreshing...", 0.9f);
            AssetDatabase.Refresh();

            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog(_Title, "DOTween setup is now complete." + (totImported == 0 ? "" : "\n" + totImported + " additional libraries were imported or updated."), "Ok");
        }

        // Removes relative .addon extension thus activating files
        static int ImportAddons(string version, string addonsDir)
        {
            bool imported = false;
            string[] filenames = new[] {
                "DOTween" + version + ".dll",
                "DOTween" + version + ".xml",
                "DOTween" + version + ".dll.mdb",
                "DOTween" + version + ".cs"
            };

            foreach (string filename in filenames) {
                string addonFilepath = addonsDir + filename + ".addon";
                string finalFilepath = addonsDir + filename;
                if (File.Exists(addonFilepath)) {
                    // Delete eventual existing final file
                    if (File.Exists(finalFilepath)) File.Delete(finalFilepath);
                    // Rename addon file to final
                    File.Move(addonFilepath, finalFilepath);
                    imported = true;
                }
            }

            return imported ? 1 : 0;
        }
    }
}