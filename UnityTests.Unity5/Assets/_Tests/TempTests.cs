using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
    public Transform target;
    Sequence seq;

    private void Awake()
    {
//        DOTween.Init();
//
//        seq = DOTween.Sequence()
//            .OnComplete(()=> Debug.Log("COMPLETE"));
//        seq.AppendInterval(1);
//        seq.Append(target.DOMoveX(5, 1).OnComplete(()=> Debug.Log("Tween 0 complete")));
//        seq.AppendCallback(() => {
//            Debug.Log("Callback fired");
//            target.DOMoveX(-5, 1);
//        });

        seq = DOTween.Sequence();
        seq.AppendInterval(2)
            .Append(target.DOMoveX(100, 1))
            .AppendCallback(() => { target.DOMoveX(-50, 2); });
    }
}