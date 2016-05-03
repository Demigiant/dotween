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

    IEnumerator Start ()
    {
    	yield return new WaitForSeconds(0.5f);

    	DOTween.Sequence()
    		.Append(target.DOMoveX(1, 1))
    		.Append(target.DOMoveY(1, 1))
    		.SetLoops(-1, LoopType.Incremental);
    }
}