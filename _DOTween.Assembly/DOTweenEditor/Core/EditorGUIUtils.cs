// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/03/12 16:04

using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.Core
{
    public static class EditorGUIUtils
    {
        static bool _stylesSet, _additionalStylesSet;
        public static GUIStyle boldLabelStyle,
                               setupLabelStyle,
                               redLabelStyle,
                               btStyle,
                               btImgStyle,
                               wrapCenterLabelStyle;
        public static GUIStyle handlelabelStyle,
                               handleSelectedLabelStyle,
                               wordWrapLabelStyle,
                               wordWrapItalicLabelStyle,
                               titleStyle,
                               logoIconStyle;
        public static GUIStyle sideBtStyle,
                               sideLogoIconBoldLabelStyle,
                               wordWrapTextArea,
                               popupButton,
                               btIconStyle;

        // Filtered ease types to show desired eases in Inspector panels
        internal static readonly string[] FilteredEaseTypes = new[] {
            "Linear",
            "InSine",
            "OutSine",
            "InOutSine",
            "InQuad",
            "OutQuad",
            "InOutQuad",
            "InCubic",
            "OutCubic",
            "InOutCubic",
            "InQuart",
            "OutQuart",
            "InOutQuart",
            "InQuint",
            "OutQuint",
            "InOutQuint",
            "InExpo",
            "OutExpo",
            "InOutExpo",
            "InCirc",
            "OutCirc",
            "InOutCirc",
            "InElastic",
            "OutElastic",
            "InOutElastic",
            "InBack",
            "OutBack",
            "InOutBack",
            "InBounce",
            "OutBounce",
            "InOutBounce",
            ":: AnimationCurve" // Must be set manually to INTERNAL_Custom
        };

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        // Ease popup with filtered eases
        public static Ease FilteredEasePopup(Ease currEase)
        {
            int stringEaseId = currEase == Ease.INTERNAL_Custom
                ? FilteredEaseTypes.Length - 1
                : Array.IndexOf(FilteredEaseTypes, currEase.ToString());
            if (stringEaseId == -1) stringEaseId = 0;
            stringEaseId = EditorGUILayout.Popup("Ease", stringEaseId, FilteredEaseTypes);
            return stringEaseId == FilteredEaseTypes.Length - 1 ? Ease.INTERNAL_Custom : (Ease)Enum.Parse(typeof(Ease), FilteredEaseTypes[stringEaseId]);
        }

        // A button which works as a toggle
        public static bool ToggleButton(bool toggled, GUIContent content, GUIStyle guiStyle = null, params GUILayoutOption[] options)
        {
            Color orColor = GUI.backgroundColor;
            GUI.backgroundColor = toggled ? Color.green : Color.white;
            bool clicked = guiStyle == null
                ? GUILayout.Button(content, options)
                : GUILayout.Button(content, guiStyle, options);
            if (clicked) {
                toggled = !toggled;
                GUI.changed = true;
            }
            GUI.backgroundColor = orColor;
            return toggled;
        }

        public static void SetGUIStyles(Vector2? footerSize = null)
        {
            if (!_additionalStylesSet && footerSize != null) {
                _additionalStylesSet = true;

                Vector2 footerSizeV = (Vector2)footerSize;
                btImgStyle = new GUIStyle(GUI.skin.button);
                btImgStyle.normal.background = null;
                btImgStyle.imagePosition = ImagePosition.ImageOnly;
                btImgStyle.padding = new RectOffset(0, 0, 0, 0);
                btImgStyle.fixedWidth = footerSizeV.x;
                btImgStyle.fixedHeight = footerSizeV.y;
            }

            if (!_stylesSet) {
                _stylesSet = true;

                boldLabelStyle = new GUIStyle(GUI.skin.label);
                boldLabelStyle.fontStyle = FontStyle.Bold;
                redLabelStyle = new GUIStyle(GUI.skin.label);
                redLabelStyle.normal.textColor = Color.red;
                setupLabelStyle = new GUIStyle(boldLabelStyle);
                setupLabelStyle.alignment = TextAnchor.MiddleCenter;

                wrapCenterLabelStyle = new GUIStyle(GUI.skin.label);
                wrapCenterLabelStyle.wordWrap = true;
                wrapCenterLabelStyle.alignment = TextAnchor.MiddleCenter;

                btStyle = new GUIStyle(GUI.skin.button);
                btStyle.padding = new RectOffset(0, 0, 10, 10);

                //

                titleStyle = new GUIStyle(GUI.skin.label) {
                    fontSize = 12,
                    fontStyle = FontStyle.Bold
                };

                handlelabelStyle = new GUIStyle(GUI.skin.label) {
                    normal = { textColor = Color.white },
                    alignment = TextAnchor.MiddleLeft
                };
                handleSelectedLabelStyle = new GUIStyle(handlelabelStyle) {
                    normal = { textColor = Color.yellow },
                    fontStyle = FontStyle.Bold
                };

                wordWrapLabelStyle = new GUIStyle(GUI.skin.label);
                wordWrapLabelStyle.wordWrap = true;

                wordWrapItalicLabelStyle = new GUIStyle(wordWrapLabelStyle);
                wordWrapItalicLabelStyle.fontStyle = FontStyle.Italic;

                logoIconStyle = new GUIStyle(GUI.skin.box);
                logoIconStyle.active.background = logoIconStyle.normal.background = null;
                logoIconStyle.margin = new RectOffset(0, 0, 4, 4);
                logoIconStyle.padding = new RectOffset(0, 0, 0, 0);

                //

                sideBtStyle = new GUIStyle(GUI.skin.button);
                sideBtStyle.margin.top = 1;
                sideBtStyle.padding = new RectOffset(0, 0, 2, 2);

                sideLogoIconBoldLabelStyle = new GUIStyle(boldLabelStyle);
                sideLogoIconBoldLabelStyle.alignment = TextAnchor.MiddleLeft;
                sideLogoIconBoldLabelStyle.padding.top = 6;

                wordWrapTextArea = new GUIStyle(GUI.skin.textArea);
                wordWrapTextArea.wordWrap = true;

                popupButton = new GUIStyle(EditorStyles.popup);
                popupButton.fixedHeight = 18;
                popupButton.margin.top += 1;

                btIconStyle = new GUIStyle(GUI.skin.button);
                btIconStyle.padding.left -= 2;
                btIconStyle.fixedWidth = 24;
                btIconStyle.stretchWidth = false;
            }
        }
    }
}