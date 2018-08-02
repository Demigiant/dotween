// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/07/16 18:41
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using DateTime = System.DateTime;

namespace DG.DOTweenUpgradeManager
{
    /// <summary>
    /// This class and its whole library are deleted the first time DOTween's setup is run after an upgrade (or after a new install).
    /// NOTE: DidReloadScripts doesn't work on first install so it's useless, InitializeOnLoad is the only way
    /// </summary>
    [InitializeOnLoad]
    static class Autorun
    {
        static Autorun()
        {
            EditorApplication.update += OnUpdate;
        }

        public static void OnUpdate()
        {
            if (!UpgradeWindowIsOpen()) {
                ApplyModulesSettings();
                UpgradeWindow.Open();
            }
        }

        static bool UpgradeWindowIsOpen()
        {
            return Resources.FindObjectsOfTypeAll<UpgradeWindow>().Length > 0;
        }

        static void ApplyModulesSettings()
        {
            Type doeditorT = Type.GetType("DG.DOTweenEditor.UI.DOTweenUtilityWindowModules, DOTweenEditor");
            if (doeditorT != null) {
                MethodInfo miOpen = doeditorT.GetMethod("ApplyModulesSettings", BindingFlags.Static | BindingFlags.Public);
                if (miOpen != null) {
                    miOpen.Invoke(null, null);
                }
            }
        }
    }
}