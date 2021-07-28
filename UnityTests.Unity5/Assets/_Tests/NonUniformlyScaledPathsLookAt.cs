using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NonUniformlyScaledPathsLookAt : BrainBase
{
    public bool useLocalSpace;
    public Vector3 endP = new Vector3(4, 2, 0);
    public float duration = 3;
    public Transform unscaledTarget, scaledTarget;
    
    Vector3[] startPs;
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.8f);
        CreateLocalPathTween(unscaledTarget);
        CreateLocalPathTween(scaledTarget);
    }
    
    void CreateLocalPathTween(Transform t)
    {
        startPs = new[] {
            unscaledTarget.position,
            scaledTarget.position
        };
        
        Vector3[] ps = new[] {
            Vector3.zero,
            endP
        };
        
        Tween tween = null;
        if (useLocalSpace) {
            tween = t.DOLocalPath(ps, duration, PathType.CatmullRom)
                .SetLookAt(0.01f);
        } else {
            tween = t.DOPath(ps, duration, PathType.CatmullRom)
                .SetLookAt(0.01f);
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