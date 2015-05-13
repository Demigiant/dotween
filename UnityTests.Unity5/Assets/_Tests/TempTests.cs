using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class TempTests : BrainBase
{
	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.8f);

		Debug.Log(Time.realtimeSinceStartup + " Create tween");

		DOTween.Sequence()
            .SetId(123)
            .PrependInterval(3)
            .OnComplete(() => Debug.Log(Time.realtimeSinceStartup + " First callback!"));

        DOTween.Kill(123, true);
	}
}