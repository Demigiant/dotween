using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
    public Transform target;
	public Ease ease;

    // Use this for initialization
    IEnumerator Start ()
    {
        yield return new WaitForSeconds(1);
        Tween t = target.DOMoveX(60000, 200).SetEase(ease);
        t.Goto(198, true);
        // t.OnUpdate(()=> Debug.Log(target.position.x));
    }
}