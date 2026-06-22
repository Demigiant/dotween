using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
	public RectTransform target;

	Tween startTween;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);
		Debug.Log("START");
		target.DOShakeAnchorPos(1f, 5f, 100, 90f, true, false)
			.OnStart(() => Debug.Log("START"))
			.OnComplete(() => Debug.Log("COMPLETE"));
	}
}