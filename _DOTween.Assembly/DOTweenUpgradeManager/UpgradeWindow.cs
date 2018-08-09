// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/08/02 12:29
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenUpgradeManager
{
    internal class UpgradeWindow : EditorWindow
    {
        const string _Title = "New Version of DOTween Imported";
        static readonly Vector2 _WinSize = new Vector2(400,280);

        const string _DescrTitle0 = "DOTWEEN SETUP REQUIRED";
        const string _DescrContent0 = "Select <color=#ffc47a><b>\"Setup DOTween...\"</b></color> in <b>DOTween's Utility Panel</b> to set it up and add/remove Modules.";
        const string _DescrTitle1 = "IMPORTANT IN CASE OF UPGRADE";
        const string _DescrContent1 = "If you're upgrading from a DOTween version older than <b>1.2.000</b> or <b>Pro older than 1.0.000</b>" +
                                      " (<color=#ffc47a><i>before the introduction of DOTween Modules</i></color>)" +
                                      " you will see lots of errors. <b>Follow these instructions</b> to fix them:";
        const string _DescrContent2 = "\n<color=#94de59><b>1)</b></color> <color=#ffc47a><b>Close and reopen the project</b></color>" +
                                      " (if you haven't already done so)" +
                                      "\n<color=#94de59><b>2)</b></color> Open DOTween's Utility Panel" +
                                      " and <color=#ffc47a><b>run the Setup</b></color> to activate required Modules";

        #region Unity and GUI Methods

        public static void Open()
        {
            EditorWindow window = EditorWindow.GetWindow<UpgradeWindow>(true, _Title, true);
            window.minSize = _WinSize;
            window.maxSize = _WinSize;
            window.ShowUtility();
        }

        void OnGUI()
        {
            Styles.Init();

            Rect area = new Rect(0, 0, position.width, position.height);

            // Background
            GUI.color = new Color(0.18f, 0.18f, 0.18f);
            GUI.DrawTexture(area, Texture2D.whiteTexture);
            GUI.color = Color.white;

            GUILayout.Space(4);
            GUILayout.Label(_DescrTitle0, Styles.descrTitle);
            GUILayout.Label(_DescrContent0, Styles.descrLabel);
            GUILayout.Space(12);
            GUILayout.Label(_DescrTitle1, Styles.descrTitle);
            GUILayout.Label(_DescrContent1, Styles.descrLabel);
            GUILayout.Space(-15);
            GUILayout.Label(_DescrContent2, Styles.descrLabel);

            // Buttons
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Open DOTween Utility Panel", GUILayout.Height(30))) {
                Type doeditorT = Type.GetType("DG.DOTweenEditor.UI.DOTweenUtilityWindow, DOTweenEditor");
                if (doeditorT != null) {
                    MethodInfo miOpen = doeditorT.GetMethod("Open", BindingFlags.Static | BindingFlags.Public);
                    if (miOpen != null) {
                        miOpen.Invoke(null, null);
                    }
                }
                EditorApplication.update -= Autorun.OnUpdate;
                this.Close();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        #endregion

        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        static class Styles
        {
            static bool _initialized;
            public static GUIStyle descrTitle, descrLabel;

            public static void Init()
            {
                if (_initialized) return;

                _initialized = true;

                descrTitle = new GUIStyle(GUI.skin.label);
                descrTitle.richText = true;
                descrTitle.fontSize = 18;
                SetTextColor(descrTitle, new Color(0.58f, 0.87f, 0.35f));

                descrLabel = new GUIStyle(GUI.skin.label);
                descrLabel.fontSize = 12;
                descrLabel.wordWrap = descrLabel.richText = true;
                SetTextColor(descrLabel, new Color(0.93f, 0.93f, 0.93f));
            }

            static void SetTextColor(GUIStyle style, Color color)
            {
                style.normal.textColor = style.active.textColor = style.focused.textColor = style.hover.textColor
                = style.onNormal.textColor = style.onActive.textColor = style.onFocused.textColor = style.onHover.textColor = color;
            }
        }
    }
}