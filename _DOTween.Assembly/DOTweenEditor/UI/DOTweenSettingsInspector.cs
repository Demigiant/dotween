// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/02/05 19:50

using DG.Tweening.Core;
using UnityEditor;

namespace DG.DOTweenEditor.UI
{
    [CustomEditor(typeof(DOTweenSettings))]
    public class DOTweenSettingsInspector : Editor
    {
        DOTweenSettings _src;

        // ===================================================================================
        // MONOBEHAVIOUR METHODS -------------------------------------------------------------

        void OnEnable()
        {
            _src = target as DOTweenSettings;
        }

        override public void OnInspectorGUI()
        {
            UnityEngine.GUI.enabled = false;

            DrawDefaultInspector();

            UnityEngine.GUI.enabled = true;
        }
    }
}