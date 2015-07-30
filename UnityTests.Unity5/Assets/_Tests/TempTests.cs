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
		DOTween.Sequence()
			.Append(target.DOMoveX(2, 2))
			.Join(target.DOMoveY(2, 2))
			.Append(target.DOScale(2, 2))
			.Join(target.DORotate(new Vector3(0, 0, 180), 2));
	}
}