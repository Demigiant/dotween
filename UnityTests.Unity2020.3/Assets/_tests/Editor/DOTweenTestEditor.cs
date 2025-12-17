// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2020/08/11

using DG.DOTweenEditor;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

class DOTweenTestEditor : EditorWindow
{
    [MenuItem("DOTween Testing/" + _Title)]
    static void ShowWindow() { GetWindow(typeof(DOTweenTestEditor), false, _Title); }

    const string _Title = "DOTween Test Editor";
    Transform _target;



    void OnGUI()
    {
        _target = (Transform)EditorGUILayout.ObjectField("Tween Target", _target, typeof(Transform), true);
        if (GUILayout.Button("Create and Play Editor Tween")) CreateEditorTween();
        if (GUILayout.Button("Stop All Editor Tweens (and reset targets)")) StopEditorTweens();
    }

    void CreateEditorTween()
    {
        // Create tween as usual
        Tween t = _target.DOMoveX(2, 1);
        // Prepare the tween for editor preview
        DOTweenEditorPreview.PrepareTweenForPreview(t);
        // Start the editor tweens update loop
        // (will update any tween you passed to PrepareTweenForPreview)
        DOTweenEditorPreview.Start();
    }

    void StopEditorTweens()
    {
        // Stop all editor tweens
        // (you can pass a TRUE parameter to also reset the tween targets to their original state)
        DOTweenEditorPreview.Stop(true);
    }
}
