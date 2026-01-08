using DG.DOTweenEditor;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class EditorPreviewer
{
    static EditorPreviewer()
    {
        TweenManager.OnEditorPreviewPrepareTweenCommand += PrepareTween;
        TweenManager.OnEditorPreviewPlayCommand += Play;
        TweenManager.OnEditorPreviewStopCommand += Stop;
    }

    static void PrepareTween(Tween t)
    {
        Debug.Log("EditorPreviewer ► AddTween");
        DOTweenEditorPreview.PrepareTweenForPreview(t);
    }

    static void Play()
    {
        Debug.Log("EditorPreviewer ► Play");
        DOTweenEditorPreview.Start();
    }

    static void Stop()
    {
        Debug.Log("EditorPreviewer ► Stop and reset");
        DOTweenEditorPreview.Stop(true);
    }
}