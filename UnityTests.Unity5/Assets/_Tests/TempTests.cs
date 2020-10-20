using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TempTests : MonoBehaviour
{
    public bool completeImmediately = true;
    public bool completeWCallbacks = false;
    public Transform target;

    void Start()
    {
        Debug.Log("Creating CALLER");
        Sequence caller = DOTween.Sequence();
        caller.AppendCallback(() => Test());
        caller.Play();
    }

    void Test()
    {
        Debug.Log("TEST called");
        Tween t = target.DOMoveX(2, 1).OnComplete(() => Debug.Log("Tween complete"));
        Sequence s = DOTween.Sequence().SetAutoKill(false)
            .AppendCallback(() => Debug.Log("Sequence internal callback 0"))
            .Append(target.DOMoveY(2, 1).OnComplete(() => Debug.Log("Nested tween complete")))
            .AppendCallback(() => Debug.Log("Sequence internal callback 1"))
            .OnRewind(() => Debug.Log("Sequence rewound"))
            .OnPlay(() => Debug.Log("Sequence onPlay"))
            .OnComplete(() => Debug.Log("Sequence complete"));

        if (completeImmediately) {
            if (completeWCallbacks) {
                t.Complete(true);
                s.Complete(true);
            } else {
                t.Complete();
                s.Complete();
            }
        }
    }
}