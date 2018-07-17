// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/07/16 18:41
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace DG.DOTweenUpgradeManager
{
    /// <summary>
    /// This class and its whole library are deleted the first time DOTween's setup is run after an upgrade (or after a new install)
    /// </summary>
    static class Autorun
    {
        [DidReloadScripts]
        static void DidReloadScripts()
        {
            EditorUtility.DisplayDialog("DOTween",
                "New version of DOTween imported: SETUP REQUIRED." +
                "\n\nSelect \"Setup DOTween...\" in DOTween's Utility Panel to set it up and to add/remove Modules." +
                "\n\nIMPORTANT: if you were upgrading from a DOTween version older than 1.2.000 you will see lots of errors." +
                " Close and reopen your project, then open DOTween's Utility Panel and run the Setup to activate required Modules.",
                "Ok"
            );
        }
    }
}