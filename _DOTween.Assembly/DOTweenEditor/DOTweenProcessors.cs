// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/07/16 18:07
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System.IO;
using DG.DOTweenEditor.UI;
using DG.Tweening;
using DG.Tweening.Core;
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
            // DOTween is being deleted: deal with it
            // Remove EditorPrefs
            EditorPrefs.DeleteKey(Application.dataPath + DOTweenUtilityWindow.Id);
            EditorPrefs.DeleteKey(Application.dataPath + DOTweenUtilityWindow.IdPro);
            // Remove scripting define symbols
            DOTweenDefines.RemoveAllDefines();
            Debug.Log("::: DOTween deleted and DOTWEEN defines removed");
//            //
//            EditorUtility.DisplayDialog("DOTween Deleted",
//                "DOTween was deleted and all of its scripting define symbols removed." +
//                "\n\nThis might show an error depending on your previous setup." +
//                " If this happens, please close and reopen Unity or reimport DOTween.",
//                "Ok"
//            );
            return AssetDeleteResult.DidNotDelete;
        }


    }

    public class UtilityWindowPostProcessor : AssetPostprocessor
    {
        static bool _setupDialogRequested; // Used to prevent OnPostProcessAllAssets firing twice (because of a Unity bug/feature)

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (_setupDialogRequested) return;

            string dotweenFile = System.Array.Find(
                importedAssets, name => name.Contains("DOTween") && !name.EndsWith(".meta") && !name.EndsWith(".jpg") && !name.EndsWith(".png")
            );
            bool dotweenImported = dotweenFile != null;
            if (dotweenImported) {
                // DOTween or Pro or Timeline imported
                if (!EditorUtils.HasGlobalDefine(DOTweenDefines.GlobalDefine_DOTween)) EditorUtils.AddGlobalDefine(DOTweenDefines.GlobalDefine_DOTween);
                // Reapply modules and ASMDEF
                EditorUtils.DelayedCall(0.1f, ()=> {
                    // Debug.Log("Apply Modules Settings after DOTween reimport (" + dotweenFile + ")");
                    DOTweenUtilityWindowModules.ApplyModulesSettings();
                    ASMDEFManager.ApplyASMDEFSettings();
                    // ASMDEFManager.RefreshExistingASMDEFFiles();
                });
            }

//             string dotweenProFile = System.Array.Find(
//                 importedAssets, name => name.Contains("DOTweenPro") && !name.EndsWith(".meta") && !name.EndsWith(".jpg") && !name.EndsWith(".png")
//             );
//             bool dotweenProImported = dotweenProFile != null;
//             if (dotweenProImported) {
//                 // Refresh ASMDEF
//                 EditorUtils.DelayedCall(0.1f, ()=> {
// //                    Debug.Log("Refresh ASMDEF after DOTweenPro reimport (" + dotweenProFile + ")");
//                     ASMDEFManager.RefreshExistingASMDEFFiles();
//                 });
//             }
//
//             string dotweenTimelineFile = System.Array.Find(
//                 importedAssets, name => name.Contains("DOTweenTimeline") && !name.EndsWith(".meta") && !name.EndsWith(".jpg") && !name.EndsWith(".png")
//             );
//             bool dotweenTimelineImported = dotweenTimelineFile != null;
//             if (dotweenTimelineImported) {
//                 // Reapply modules
//                 EditorUtils.DelayedCall(0.1f, ()=> {
// //                    Debug.Log("Apply Modules Settings after DOTweenTimeline reimport (" + dotweenTimelineFile + ")");
//                     DOTweenUtilityWindowModules.ApplyModulesSettings();
//                 });
//             }
        }
    }
}