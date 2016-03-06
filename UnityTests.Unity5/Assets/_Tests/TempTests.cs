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
    public float InitialDelay = 1;
    public float ExpandDelay = 2;
    public float ExpandSpeed = 0.1f;
    public float RetractDelay = 2;
    public float RetractSpeed = 2;

    float time;
 
    Sequence s;
    // Use this for initialization
    void Start ()
    {
        time = Time.realtimeSinceStartup;

        s =  DOTween.Sequence();
        s.Append(target.DOMove(target.position + new Vector3(0, 0.1f, 0),0.1f).SetDelay(InitialDelay));
        s.Append(target.DOMove(target.position + new Vector3(0, 1, 0), ExpandSpeed).SetDelay(ExpandDelay));
        s.Append(target.DOMove(target.position - new Vector3(0, 1.1f, 0), RetractSpeed).SetDelay(RetractDelay));
        s.SetLoops(-1, LoopType.Restart).OnStepComplete(Step);
    }

    void Step()
    {
        Debug.Log(Time.realtimeSinceStartup - time);
        time = Time.realtimeSinceStartup;
    }
}