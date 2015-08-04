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

	IEnumerator Start()
	{
		Vector3[] wps = new[] {
			new Vector3(-9.9f, -4.9f, 15.0f), new Vector3(-9.9f, -2.4f, 10.0f), new Vector3(-9.9f, 2.7f, 10.0f), new Vector3(-9.9f, 5.2f, 15.0f)
		};
		Tween t = target.DOPath(wps, 4).SetEase(Ease.Linear).Pause();

		yield return new WaitForSeconds(1);
		t.Goto(1);
		Debug.Log("1 " + target.position);

		yield return new WaitForSeconds(2);
		t.Goto(2);
		Debug.Log("2 " + target.position);
	}
}