using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
    public Transform target;
    public float duration = 8;
    public Ease ease;
    public float amplitude = 8;
    public float period = 1;

    IEnumerator Start()
    {
    	yield return new WaitForSeconds(0.5f);

    	target.DOMoveY(2, duration).SetRelative().SetEase(ease, amplitude, period);
    }
}