using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class TempTests : BrainBase
{
    public Transform target;
    Tween myTween;
    bool aBool;

    void Start()
    {
    	DOTween.Init();
    	this.StartCoroutine(COUpdate());
    }

    IEnumerator COUpdate()
    {
    	yield return null;
    	yield return null;
    	target.DOMoveX(2, 7);
    	yield return null;
    	// for (int i = 0; i < 1000; ++i) target.DOMoveX(2, 7).OnComplete(ACallback);
    	// for (int i = 0; i < 1000; ++i) target.DOMoveX(2, 7).OnComplete(()=> aBool = true);
    	target.DOMoveX(2, 7).OnComplete(()=> aBool = true);
    	// myTween.Kill();
    	// myTween = target.DOMoveX(2, 1);
    	// myTween.OnComplete(ACallback);
    }

    void ACallback()
    {
    	Debug.Log("DO SOMETHING");
    }
}