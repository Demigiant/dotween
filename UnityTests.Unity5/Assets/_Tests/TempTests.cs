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

    IEnumerator Start()
    {
    	yield return new WaitForSeconds(0.5f);

    	target.DOMoveY(2, 5).SetRelative().SetEase(Ease.InOutFlash, 8, 1);
    }
}