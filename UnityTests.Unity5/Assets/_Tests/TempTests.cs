using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;
 
public class TempTests : BrainBase
{
	public Transform target;
	public Ease easeType;
	public float gotoTime;
	public Vector3[] waypoints;

	int count;

	Tween t;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.6f);

		t = target.DOPath(waypoints, 5, PathType.CatmullRom).SetEase(easeType).SetLoops(-1);
	}
}