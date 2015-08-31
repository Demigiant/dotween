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

	void Start()
	{
		float delay = 0.15f;
	    Sequence seq = DOTween.Sequence();
	    seq.Insert(delay, target.DOMoveX(0.5f, 0.5f).OnComplete(() => {
	      Debug.Log("Doesn't work");
	    }));
	    seq.OnComplete(()=> Debug.Log("Works"));

		target.DORotate (new Vector3 (1, 2, 1), 2);
	}
}