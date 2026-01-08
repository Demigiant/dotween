using System;
using DG.Tweening;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TweenManager : MonoBehaviour
{
#if UNITY_EDITOR
    #region Editor-only Events
    
    public static event Action<Tween> OnEditorPreviewPrepareTweenCommand;
    public static event Action OnEditorPreviewPlayCommand;
    public static event Action OnEditorPreviewStopCommand;
    static void Dispatch_OnEditorPreviewPrepareTweenCommand(Tween t) { if (OnEditorPreviewPrepareTweenCommand != null) OnEditorPreviewPrepareTweenCommand(t); }
    static void Dispatch_OnEditorPreviewPlayCommand() { if (OnEditorPreviewPlayCommand != null) OnEditorPreviewPlayCommand(); }
    static void Dispatch_OnEditorPreviewStopCommand() { if (OnEditorPreviewStopCommand != null) OnEditorPreviewStopCommand(); }

    #endregion
#endif

    #region Serialized

    [SerializeField] Transform[] targets;

    #endregion

    Tween[] tweens;

    void OnEnable()
    {
        if (targets == null) return;

        tweens = new Tween[targets.Length];
        for (int i = 0; i < targets.Length; ++i) {
            tweens[i] = targets[i].DOMoveX(5, 2);
#if UNITY_EDITOR
            if (!Application.isPlaying) Dispatch_OnEditorPreviewPrepareTweenCommand(tweens[i]);
#endif
        }

#if UNITY_EDITOR
        if (!Application.isPlaying) Dispatch_OnEditorPreviewPlayCommand();
#endif
    }

    void OnDisable()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) Dispatch_OnEditorPreviewStopCommand();
#endif

        // Kill all tweens
        foreach (Tween t in tweens) t.Kill();
        tweens = null;
    }
}