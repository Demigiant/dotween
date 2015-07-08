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
	public float delay = 0;

	void Start()
	{
		Tween t = target.DOJump(new Vector3(4, 3, 0), 2, 1, 1).SetRelative();
		if (delay > 0) t.SetDelay(delay);
		t.OnStart(()=>Debug.Log("Start"));
		t.OnComplete(()=>Debug.Log("Complete"));


		// Tween t = DOTween.Sequence().Append(target.DOMoveX(2, 1).SetRelative());
		// if (delay > 0) t.SetDelay(delay);
		// t.OnStart(()=>Debug.Log("Start"));
		// t.OnComplete(()=>Debug.Log("Complete"));
	}
}