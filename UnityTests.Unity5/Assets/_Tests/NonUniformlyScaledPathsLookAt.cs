using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NonUniformlyScaledPathsLookAt : BrainBase
{
    public bool useLocalSpace;
    public bool tweenUnscaledTarget, tweenScaledTarget;
    public bool setTransfFwUp;
    public float duration = 3;
    public PathMode pathMode = PathMode.Full3D;
    public Vector3 endP = new Vector3(4, 2, 0);
    public Transform unscaledTarget, scaledTarget;
    
    Vector3[] startPs;
    
    IEnumerator Start()
    {
        startPs = new[] {
            unscaledTarget.position,
            scaledTarget.position
        };
        yield return new WaitForSeconds(0.8f);
        if (tweenUnscaledTarget) CreateLocalPathTween(unscaledTarget);
        if (tweenScaledTarget) CreateLocalPathTween(scaledTarget);
    }
    
    void CreateLocalPathTween(Transform t)
    {
        Tween tween = null;
        Vector3[] ps = new[] {
            Vector3.zero,
            endP
        };
        if (useLocalSpace) {
            tween = setTransfFwUp
                ? t.DOLocalPath(ps, duration, PathType.CatmullRom, pathMode)
                    .SetLookAt(0, t.forward, t.up)
                : t.DOLocalPath(ps, duration, PathType.CatmullRom, pathMode)
                .SetLookAt(0);
        } else {
            tween = setTransfFwUp
                ? t.DOPath(ps, duration, PathType.CatmullRom, pathMode)
                .SetLookAt(0, t.forward, t.up)
                : t.DOPath(ps, duration, PathType.CatmullRom, pathMode)
                .SetLookAt(0);
        }
        tween.SetEase(Ease.Linear).SetLoops(-1);
    }
    
    void OnDrawGizmos()
    {
        if (startPs == null) return;
        foreach(Vector3 p in startPs) Gizmos.DrawSphere(p, 0.25f);
        if (useLocalSpace) {
            Gizmos.DrawSphere(unscaledTarget.parent.TransformPoint(endP), 0.25f);
            Gizmos.DrawSphere(scaledTarget.parent.TransformPoint(endP), 0.25f);
        } else {
            Gizmos.DrawSphere(endP, 0.25f);
            Gizmos.DrawSphere(endP, 0.25f);
        }
    }
}