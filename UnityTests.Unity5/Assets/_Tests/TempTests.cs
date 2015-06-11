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
	public Transform trans;
	public Renderer rend;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		DOTween.Sequence()
			.Append(rend.material.DOFade(0, 0.5f))
			.Append(rend.material.DOFade(1, 1f));
	}
}