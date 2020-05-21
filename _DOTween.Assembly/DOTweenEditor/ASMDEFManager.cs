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
            DOTweenPro,
            DOTweenProEditor
        }

        enum ChangeType
        {
            Deleted,
            Created,
            Overwritten
        }

        public static bool hasModulesASMDEF { get; private set; }
        public static bool hasProASMDEF { get; private set; }
        public static bool hasProEditorASMDEF { get; private set; }


        const string _ModulesId = "DOTween.Modules";
        const string _ProId = "DOTweenPro.Scripts";
        const string _ProEditorId = "DOTweenPro.EditorScripts";
        const string _ModulesASMDEFFile = _ModulesId + ".asmdef";
        const string _ProASMDEFFile = _ProId + ".asmdef";
        const string _ProEditorASMDEFFile = _ProEditorId + ".asmdef";

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
            hasProEditorASMDEF = File.Exists(EditorUtils.dotweenProEditorDir + _ProEditorASMDEFFile);
        }

        public static void RefreshExistingASMDEFFiles()
        {
            Refresh();

            if (!hasModulesASMDEF) {
                if (hasProASMDEF) RemoveASMDEF(ASMDEFType.DOTweenPro);
                if (hasProEditorASMDEF) RemoveASMDEF(ASMDEFType.DOTweenProEditor);
                return;
            }

            if (!EditorUtils.hasPro) return;

            if (!hasProASMDEF) CreateASMDEF(ASMDEFType.DOTweenPro);
            if (!hasProEditorASMDEF) CreateASMDEF(ASMDEFType.DOTweenProEditor);

            // Pro ASMDEF present: check that they contain correct elements
            DOTweenSettings src = DOTweenUtilityWindow.GetDOTweenSettings();
            if (src == null) return;

            ValidateProASMDEFReferences(src, ASMDEFType.DOTweenPro, EditorUtils.dotweenProDir + _ProASMDEFFile);
            ValidateProASMDEFReferences(src, ASMDEFType.DOTweenProEditor, EditorUtils.dotweenProEditorDir + _ProEditorASMDEFFile);
        }

        public static void CreateAllASMDEF()
        {
            CreateASMDEF(ASMDEFType.Modules);
            if (!EditorUtils.hasPro) return;
            CreateASMDEF(ASMDEFType.DOTweenPro);
            CreateASMDEF(ASMDEFType.DOTweenProEditor);
        }

        public static void RemoveAllASMDEF()
        {
            RemoveASMDEF(ASMDEFType.Modules);
            RemoveASMDEF(ASMDEFType.DOTweenPro);
            RemoveASMDEF(ASMDEFType.DOTweenProEditor);
        }

        #endregion

        #region Methods

        static void ValidateProASMDEFReferences(DOTweenSettings src, ASMDEFType asmdefType, string asmdefFilepath)
        {
            bool hasTextMeshProRef = false;
            using (StreamReader sr = new StreamReader(asmdefFilepath)) {
                string s;
                while ((s = sr.ReadLine()) != null) {
                    if (!s.Contains(_RefTextMeshPro)) continue;
                    hasTextMeshProRef = true;
                    break;
                }
            }
            bool recreate = hasTextMeshProRef != src.modules.textMeshProEnabled;
            if (recreate) CreateASMDEF(asmdefType, true);
        }

        static void LogASMDEFChange(ASMDEFType asmdefType, ChangeType changeType)
        {
            string asmdefTypeStr = "";
            switch (asmdefType) {
            case ASMDEFType.Modules:
                asmdefTypeStr = "DOTween/Modules/" + _ModulesASMDEFFile;
                break;
            case ASMDEFType.DOTweenPro:
                asmdefTypeStr = "DOTweenPro/" + _ProASMDEFFile;
                break;
            case ASMDEFType.DOTweenProEditor:
                asmdefTypeStr = "DOTweenPro/Editor/" + _ProEditorASMDEFFile;
                break;
            }
            Debug.Log(string.Format(
                "<b>DOTween ASMDEF file <color=#{0}>{1}</color></b> ► {2}",
                changeType == ChangeType.Deleted ? "ff0000" : changeType == ChangeType.Created ? "00ff00" : "ff6600",
                changeType == ChangeType.Deleted ? "removed" : changeType == ChangeType.Created ? "created" : "changed",
                asmdefTypeStr
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
            case ASMDEFType.DOTweenProEditor:
                alreadyPresent = hasProEditorASMDEF;
                asmdefId = _ProEditorId;
                asmdefFile = _ProEditorASMDEFFile;
                asmdefDir = EditorUtils.dotweenProEditorDir;
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
                case ASMDEFType.DOTweenProEditor:
                    sw.WriteLine("\t\"name\": \"{0}\",", asmdefId);
                    sw.WriteLine("\t\"references\": [");
                    DOTweenSettings src = DOTweenUtilityWindow.GetDOTweenSettings();
                    if (src != null) {
                        if (src.modules.textMeshProEnabled) sw.WriteLine("\t\t\"{0}\",", _RefTextMeshPro);
                    }
                    if (type == ASMDEFType.DOTweenProEditor) {
                        sw.WriteLine("\t\t\"{0}\",", _ModulesId);
                        sw.WriteLine("\t\t\"{0}\"", _ProId);
                        sw.WriteLine("\t],");
                        sw.WriteLine("\t\"includePlatforms\": [");
                        sw.WriteLine("\t\t\"Editor\"");
                        sw.WriteLine("\t],");
                        sw.WriteLine("\t\"autoReferenced\": false");
                    } else {
                        sw.WriteLine("\t\t\"{0}\"", _ModulesId);
                        sw.WriteLine("\t]");
                    }
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
            case ASMDEFType.DOTweenProEditor:
                alreadyPresent = hasProEditorASMDEF;
                asmdefFile = _ProEditorASMDEFFile;
                asmdefDir = EditorUtils.dotweenProEditorDir;
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