// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/07/16 18:41
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
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
        static readonly string _Id = "DOTweenUpgradeManager";

        static Autorun()
        {
            _Id = Application.dataPath + _Id;

            bool refresh;
            DateTime now = DateTime.UtcNow;
            DateTime lastTime;
            if (!EditorPrefs.HasKey(_Id) || !DateTime.TryParse(EditorPrefs.GetString(_Id), out lastTime)) {
                refresh = true;
            } else {
                refresh = (now - lastTime).TotalSeconds > 60;
            }
            if (refresh) {
                EditorPrefs.SetString(_Id, now.ToString());
                EditorUtility.DisplayDialog("DOTween",
                    "DOTWEEN SETUP REQUIRED: new version imported." +
                    "\n\nSelect \"Setup DOTween...\" in DOTween's Utility Panel to set it up and add/remove Modules." +
                    "\n\n::::: IMPORTANT :::::" +
                    "\nIf you're upgrading from a DOTween version older than 1.2.000 you will see lots of errors." +
                    "\n1) Close and reopen the project (if you haven't already done so)" +
                    "\n2) Open DOTween's Utility Panel and run the Setup to activate required Modules",
                    "Ok"
                );
            }
        }
    }
}