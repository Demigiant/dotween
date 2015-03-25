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
	public Vector3[] waypoints;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.6f);

		Tween t = target.DOPath(waypoints, 5);
		yield return null;
		yield return null;

		t.GotoWaypoint(2);
	}
}