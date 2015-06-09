using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : MonoBehaviour
{
	public bool straightFixedUpdate;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		Tween t = transform.DORotate(new Vector3(0, 180, 0), 2).OnComplete(()=> Debug.Log("Complete"));
		if (straightFixedUpdate) t.SetUpdate(UpdateType.Fixed);
	}
}