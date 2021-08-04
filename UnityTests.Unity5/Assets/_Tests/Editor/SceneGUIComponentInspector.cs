// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2020/08/11

using System;
using DG.DOTweenEditor;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneGUIComponent))]
public class SceneGUIComponentInspector : UnityEditor.Editor
{
    SceneGUIComponent _src;

    void OnEnable()
    {
        _src = target as SceneGUIComponent;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Custom Inspector with OnSceneGUI implemented");
        DrawDefaultInspector();
    }

    void OnSceneGUI()
    {
        Handles.BeginGUI();
        using (new GUILayout.HorizontalScope()) {
            using (new EditorGUI.DisabledScope(DOTweenEditorPreview.isPreviewing)) {
                if (GUILayout.Button("Begin tweens update loop")) BeginTweensUpdateLoop();
            }
            using (new EditorGUI.DisabledScope(!DOTweenEditorPreview.isPreviewing)) {
                if (GUILayout.Button("Stop tweens update loop")) StopTweensUpdateLoop(false);
                if (GUILayout.Button("Stop tweens update loop and reset targets")) StopTweensUpdateLoop(true);
            }
        }
        using (new GUILayout.HorizontalScope()) {
            if (GUILayout.Button("Create tweens")) CreateTweens(false);
            if (GUILayout.Button("Create tweens FROM")) CreateTweens(true);
            if (GUILayout.Button("Create sequences")) CreateSequences();
        }
        if (DOTweenEditorPreview.isPreviewing) GUILayout.Label("Tweens are being updated");
        Handles.EndGUI();
    }

    void BeginTweensUpdateLoop()
    {
        // Start the editor tweens update loop
        // (will update any tween you passed to PrepareTweenForPreview)
        DOTweenEditorPreview.Start();
    }

    void StopTweensUpdateLoop(bool andResetTargets)
    {
        // Stop all editor tweens
        // (you can pass a TRUE parameter to also reset the tween targets to their original state)
        DOTweenEditorPreview.Stop(andResetTargets);
    }

    void CreateTweens(bool from)
    {
        // Create tweens as usual
        foreach (Transform t in _src.tweenTargets) {
            Tween tween = from ? t.DOMoveX(2, 1).From() : t.DOMoveX(2, 1);
            DOTweenEditorPreview.PrepareTweenForPreview(tween);
        }
    }

    void CreateSequences()
    {
        Sequence s = DOTween.Sequence();
        foreach (Transform t in _src.tweenTargets) {
            s.Join(t.DOMoveX(2, 1));
        }
        DOTweenEditorPreview.PrepareTweenForPreview(s);
    }
}
