// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/03/12 16:04

using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.UI
{
    public static class EditorGUIUtils
    {
        static bool _stylesSet, _additionalStylesSet;
        public static GUIStyle boldLabelStyle,
                               setupLabelStyle,
                               redLabelStyle,
                               btBigStyle,
                               btSetup,
                               btImgStyle,
                               wrapCenterLabelStyle;
        public static GUIStyle handlelabelStyle,
                               handleSelectedLabelStyle,
                               wordWrapLabelStyle,
                               wordWrapRichTextLabelStyle,
                               wordWrapItalicLabelStyle,
                               titleStyle,
                               logoIconStyle;
        public static GUIStyle sideBtStyle,
                               sideLogoIconBoldLabelStyle,
                               wordWrapTextArea,
                               popupButton,
                               btIconStyle;
        public static GUIStyle infoboxStyle;

        public static Texture2D logo
        {
            get
            {
                if (_logo == null) {
                    _logo = AssetDatabase.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + "Imgs/DOTweenIcon.png", typeof(Texture2D)) as Texture2D;
                    EditorUtils.SetEditorTexture(_logo, FilterMode.Bilinear, 128);
                }
                return _logo;
            }
        }
        static Texture2D _logo;

        // Filtered ease types to show desired eases in Inspector panels
        public static readonly string[] FilteredEaseTypes = new[] {
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
            // Extra custom
            "Flash", "InFlash", "OutFlash", "InOutFlash",
            // Curve
            ":: AnimationCurve" // Must be set manually to INTERNAL_Custom
        };

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        // Ease popup with filtered eases
        public static Ease FilteredEasePopup(string label, Ease currEase, GUIStyle style = null)
        {
            if (style == null) style = EditorStyles.popup;
            Rect area = EditorGUILayout.GetControlRect(label != null, 18, style);
            return FilteredEasePopup(area, label, currEase, style);
//            int stringEaseId = currEase == Ease.INTERNAL_Custom
//                ? FilteredEaseTypes.Length - 1
//                : Array.IndexOf(FilteredEaseTypes, currEase.ToString());
//            if (stringEaseId == -1) stringEaseId = 0;
//            stringEaseId = label == null
//                ? EditorGUILayout.Popup(stringEaseId, FilteredEaseTypes, style == null ? EditorStyles.popup : style)
//                : EditorGUILayout.Popup(label, stringEaseId, FilteredEaseTypes, style == null ? EditorStyles.popup : style);
//            return stringEaseId == FilteredEaseTypes.Length - 1
//                ? Ease.INTERNAL_Custom
//                : (Ease)Enum.Parse(typeof(Ease), FilteredEaseTypes[stringEaseId]);
        }
        // Ease popup with filtered eases
        public static Ease FilteredEasePopup(Rect rect, string label, Ease currEase, GUIStyle style = null)
        {
            int stringEaseId = currEase == Ease.INTERNAL_Custom
                ? FilteredEaseTypes.Length - 1
                : Array.IndexOf(FilteredEaseTypes, currEase.ToString());
            if (stringEaseId == -1) stringEaseId = 0;
            stringEaseId = label == null
                ? EditorGUI.Popup(rect, stringEaseId, FilteredEaseTypes, style == null ? EditorStyles.popup : style)
                : EditorGUI.Popup(rect, label, stringEaseId, FilteredEaseTypes, style == null ? EditorStyles.popup : style);
            return stringEaseId == FilteredEaseTypes.Length - 1
                ? Ease.INTERNAL_Custom
                : (Ease)Enum.Parse(typeof(Ease), FilteredEaseTypes[stringEaseId]);
        }

        public static void InspectorLogo()
        {
            GUILayout.Box(logo, logoIconStyle);
        }

        // A button which works as a toggle
        public static bool ToggleButton(bool toggled, GUIContent content, bool alert = false, GUIStyle guiStyle = null, params GUILayoutOption[] options)
        {
            Color orColor = UnityEngine.GUI.backgroundColor;
            UnityEngine.GUI.backgroundColor = toggled ? alert ? Color.red : Color.green : Color.white;
            bool clicked = guiStyle == null
                ? GUILayout.Button(content, options)
                : GUILayout.Button(content, guiStyle, options);
            if (clicked) {
                toggled = !toggled;
                UnityEngine.GUI.changed = true;
            }
            UnityEngine.GUI.backgroundColor = orColor;
            return toggled;
        }

        public static void SetGUIStyles(Vector2? footerSize = null)
        {
            if (!_additionalStylesSet && footerSize != null) {
                _additionalStylesSet = true;

                Vector2 footerSizeV = (Vector2)footerSize;
                btImgStyle = new GUIStyle(UnityEngine.GUI.skin.button);
                btImgStyle.normal.background = null;
                btImgStyle.imagePosition = ImagePosition.ImageOnly;
                btImgStyle.padding = new RectOffset(0, 0, 0, 0);
//                btImgStyle.fixedWidth = footerSizeV.x;
                btImgStyle.fixedHeight = footerSizeV.y;
            }

            if (!_stylesSet) {
                _stylesSet = true;

                boldLabelStyle = new GUIStyle(UnityEngine.GUI.skin.label);
                boldLabelStyle.fontStyle = FontStyle.Bold;
                redLabelStyle = new GUIStyle(UnityEngine.GUI.skin.label);
                redLabelStyle.normal.textColor = Color.red;
                setupLabelStyle = new GUIStyle(boldLabelStyle);
                setupLabelStyle.alignment = TextAnchor.MiddleCenter;

                wrapCenterLabelStyle = new GUIStyle(UnityEngine.GUI.skin.label);
                wrapCenterLabelStyle.wordWrap = true;
                wrapCenterLabelStyle.alignment = TextAnchor.MiddleCenter;

                btBigStyle = new GUIStyle(UnityEngine.GUI.skin.button);
                btBigStyle.padding = new RectOffset(0, 0, 10, 10);

                btSetup = new GUIStyle(btBigStyle);
                btSetup.padding = new RectOffset(10, 10, 6, 6);
                btSetup.wordWrap = true;
                btSetup.richText = true;

                //

                titleStyle = new GUIStyle(UnityEngine.GUI.skin.label) {
                    fontSize = 12,
                    fontStyle = FontStyle.Bold
                };

                handlelabelStyle = new GUIStyle(UnityEngine.GUI.skin.label) {
                    normal = { textColor = Color.white },
                    alignment = TextAnchor.MiddleLeft
                };
                handleSelectedLabelStyle = new GUIStyle(handlelabelStyle) {
                    normal = { textColor = Color.yellow },
                    fontStyle = FontStyle.Bold
                };

                wordWrapLabelStyle = new GUIStyle(UnityEngine.GUI.skin.label);
                wordWrapLabelStyle.wordWrap = true;

                wordWrapRichTextLabelStyle = new GUIStyle(UnityEngine.GUI.skin.label);
                wordWrapRichTextLabelStyle.wordWrap = true;
                wordWrapRichTextLabelStyle.richText = true;

                wordWrapItalicLabelStyle = new GUIStyle(wordWrapLabelStyle);
                wordWrapItalicLabelStyle.fontStyle = FontStyle.Italic;

                logoIconStyle = new GUIStyle(UnityEngine.GUI.skin.box);
                logoIconStyle.active.background = logoIconStyle.normal.background = null;
                logoIconStyle.margin = new RectOffset(0, 0, 0, 0);
                logoIconStyle.padding = new RectOffset(0, 0, 0, 0);

                //

                sideBtStyle = new GUIStyle(UnityEngine.GUI.skin.button);
                sideBtStyle.margin.top = 1;
                sideBtStyle.padding = new RectOffset(0, 0, 2, 2);

                sideLogoIconBoldLabelStyle = new GUIStyle(boldLabelStyle);
                sideLogoIconBoldLabelStyle.alignment = TextAnchor.MiddleLeft;
                sideLogoIconBoldLabelStyle.padding.top = 2;

                wordWrapTextArea = new GUIStyle(UnityEngine.GUI.skin.textArea);
                wordWrapTextArea.wordWrap = true;

                popupButton = new GUIStyle(EditorStyles.popup);
                popupButton.fixedHeight = 18;
                popupButton.margin.top += 1;

                btIconStyle = new GUIStyle(UnityEngine.GUI.skin.button);
                btIconStyle.padding.left -= 2;
                btIconStyle.fixedWidth = 24;
                btIconStyle.stretchWidth = false;

                //

                infoboxStyle = new GUIStyle(GUI.skin.box) {
                    alignment = TextAnchor.UpperLeft,
                    richText = true,
                    wordWrap = true,
                    padding = new RectOffset(5, 5, 5, 6),
                    normal = { textColor = Color.white, background = Texture2D.whiteTexture }
                };
            }
        }
    }
}