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

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.6f);

		Tweener t = target.DOLocalMove(new Vector3(4, 4, 0), 3);

		yield return new WaitForSeconds(0.1f);

		t.ChangeEndValue(new Vector3(0, 8, 0));
	}
}