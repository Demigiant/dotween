// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2019/03/05 12:37
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.IO;
using DG.DOTweenEditor.UI;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    internal static class ASMDEFManager
    {
        public enum ASMDEFType
        {
            Modules,
            DOTweenPro
        }

        enum ChangeType
        {
            Deleted,
            Created,
            Overwritten
        }

        public static bool hasModulesASMDEF { get; private set; }
        public static bool hasProASMDEF { get; private set; }

        const string _ModulesId = "DOTween.Modules";
        const string _ProId = "DOTweenPro.Scripts";
        const string _ModulesASMDEFFile = _ModulesId + ".asmdef";
        const string _ProASMDEFFile = _ProId + ".asmdef";

        const string _RefTextMeshPro = "Unity.TextMeshPro";

        static ASMDEFManager()
        {
            Refresh();
        }

        #region Public Methods

        public static void Refresh()
        {
            hasModulesASMDEF = File.Exists(EditorUtils.dotweenModulesDir + _ModulesASMDEFFile);
            hasProASMDEF = File.Exists(EditorUtils.dotweenProDir + _ProASMDEFFile);
        }

        public static void RefreshExistingASMDEFFiles()
        {
            Refresh();

            if (!hasModulesASMDEF) {
                if (hasProASMDEF) RemoveASMDEF(ASMDEFType.DOTweenPro);
                return;
            }
            if (EditorUtils.hasPro && !hasProASMDEF) {
                CreateASMDEF(ASMDEFType.DOTweenPro);
                return;
            }

            // Pro ASMDEF present: check that it contains correct elements
            DOTweenSettings src = DOTweenUtilityWindow.GetDOTweenSettings();
            if (src == null) return;

            bool hasTextMeshProRef = false;
            using (StreamReader sr = new StreamReader(EditorUtils.dotweenProDir + _ProASMDEFFile)) {
                string s;
                while ((s = sr.ReadLine()) != null) {
                    if (!s.Contains(_RefTextMeshPro)) continue;
                    hasTextMeshProRef = true;
                    break;
                }
            }
            bool recreate = hasTextMeshProRef != src.modules.textMeshProEnabled;
            if (recreate) CreateASMDEF(ASMDEFType.DOTweenPro, true);
        }

        public static void CreateAllASMDEF()
        {
            CreateASMDEF(ASMDEFType.Modules);
            CreateASMDEF(ASMDEFType.DOTweenPro);
        }

        public static void RemoveAllASMDEF()
        {
            RemoveASMDEF(ASMDEFType.Modules);
            RemoveASMDEF(ASMDEFType.DOTweenPro);
        }

        #endregion

        #region Methods

        static void LogASMDEFChange(ASMDEFType asmdefType, ChangeType changeType)
        {
            Debug.Log(string.Format(
                "<b>DOTween ASMDEF file <color=#{0}>{1}</color></b> ► {2}",
                changeType == ChangeType.Deleted ? "ff0000" : changeType == ChangeType.Created ? "00ff00" : "ff6600",
                changeType == ChangeType.Deleted ? "removed" : changeType == ChangeType.Created ? "created" : "changed",
                asmdefType == ASMDEFType.Modules ? "DOTween/Modules/" + _ModulesASMDEFFile : "DOTweenPro/" + _ProASMDEFFile
            ));
        }

        static void CreateASMDEF(ASMDEFType type, bool forceOverwrite = false)
        {
            Refresh();
            bool alreadyPresent = false;
            string asmdefId = null;
            string asmdefFile = null;
            string asmdefDir = null; // with final OS slash
            switch (type) {
            case ASMDEFType.Modules:
                alreadyPresent = hasModulesASMDEF;
                asmdefId = _ModulesId;
                asmdefFile = _ModulesASMDEFFile;
                asmdefDir = EditorUtils.dotweenModulesDir;
                break;
            case ASMDEFType.DOTweenPro:
                alreadyPresent = hasProASMDEF;
                asmdefId = _ProId;
                asmdefFile = _ProASMDEFFile;
                asmdefDir = EditorUtils.dotweenProDir;
                break;
            }
            if (alreadyPresent && !forceOverwrite) {
                EditorUtility.DisplayDialog("Create ASMDEF", asmdefFile + " already exists", "Ok");
                return;
            }
            if (!Directory.Exists(asmdefDir)) {
                EditorUtility.DisplayDialog(
                    "Create ASMDEF",
                    string.Format("Directory not found\n({0})", asmdefDir),
                    "Ok"
                );
                return;
            }

            string asmdefFilePath = asmdefDir + asmdefFile;
            using (StreamWriter sw = File.CreateText(asmdefFilePath)) {
                sw.WriteLine("{");
                switch (type) {
                case ASMDEFType.Modules:
                    sw.WriteLine("\t\"name\": \"{0}\"", asmdefId);
                    break;
                case ASMDEFType.DOTweenPro:
                    sw.WriteLine("\t\"name\": \"{0}\",", asmdefId);
                    sw.WriteLine("\t\"references\": [");
                    DOTweenSettings src = DOTweenUtilityWindow.GetDOTweenSettings();
                    if (src != null) {
                        if (src.modules.textMeshProEnabled) sw.WriteLine("\t\t\"{0}\",", _RefTextMeshPro);
                    }
                    sw.WriteLine("\t\t\"{0}\"", _ModulesId);
                    sw.WriteLine("\t]");
                    break;
                }
                sw.WriteLine("}");
            }
            string adbFilePath = EditorUtils.FullPathToADBPath(asmdefFilePath);
            AssetDatabase.ImportAsset(adbFilePath, ImportAssetOptions.ForceUpdate);
            Refresh();
            LogASMDEFChange(type, alreadyPresent ? ChangeType.Overwritten : ChangeType.Created);
        }

        static void RemoveASMDEF(ASMDEFType type)
        {
            bool alreadyPresent = false;
            string asmdefFile = null;
            string asmdefDir = null; // with final OS slash
            switch (type) {
            case ASMDEFType.Modules:
                alreadyPresent = hasModulesASMDEF;
                asmdefDir = EditorUtils.dotweenModulesDir;
                asmdefFile = _ModulesASMDEFFile;
                break;
            case ASMDEFType.DOTweenPro:
                alreadyPresent = hasProASMDEF;
                asmdefFile = _ProASMDEFFile;
                asmdefDir = EditorUtils.dotweenProDir;
                break;
            }

            Refresh();
            if (!alreadyPresent) {
                EditorUtility.DisplayDialog("Remove ASMDEF", asmdefFile + " not present", "Ok");
                return;
            }

            string asmdefFilePath = asmdefDir + asmdefFile;
            AssetDatabase.DeleteAsset(EditorUtils.FullPathToADBPath(asmdefFilePath));
            Refresh();
            LogASMDEFChange(type, ChangeType.Deleted);
        }

        #endregion
    }
}