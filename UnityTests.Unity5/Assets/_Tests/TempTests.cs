using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempTests : BrainBase
{
    public Transform target;
    public float y = 1;
    public AnimationCurve ease;
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.8f);

        target.DOMoveY(y, 2).SetEase(ease);
    }
}