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
	public RectTransform target;

    IEnumerator Start ()
    {
    	yield return new WaitForSeconds(0.5f);

    	target.DOPivot(Vector2.zero, 2);
    }
}