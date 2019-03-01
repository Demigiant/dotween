using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOCompletePerformance : BrainBase
{
	public int totTweens = 500;
	public Transform target;
	public float floatValue;

	IEnumerator CO_TestDOCompleteBy(bool byTarget, bool tweenFloatValue)
	{
		Debug.Log(Time.realtimeSinceStartup + " :: Create " + totTweens + " tweens on " + (tweenFloatValue ? "float" : "transform"));
		for (int i = 0; i < totTweens; ++i) {
			Tween t = tweenFloatValue
				? (Tween)DOTween.To(()=> floatValue, x=> floatValue = x, 2, 10)
				: target.DOMoveX(2, 10);
			if (!byTarget) t.SetId("myId");
			else if (tweenFloatValue) t.SetTarget(target);
		}
		yield return new WaitForSeconds(2f);

		Debug.Log(Time.realtimeSinceStartup + " :: Complete " + totTweens + " tweens by " + (byTarget ? "target" : "id"));
		float time = Time.realtimeSinceStartup;
		if (byTarget) target.DOComplete();
		else DOTween.Complete("myId");
		float elapsed = Time.realtimeSinceStartup - time;
		Debug.Log(Time.realtimeSinceStartup + " :: Completed " + totTweens + " tweens in " + elapsed + " seconds");
	}

	public void TestDOCompleteBy_Transform(bool byTarget)
	{
		this.StartCoroutine(CO_TestDOCompleteBy(byTarget, false));
	}

	public void TestDOCompleteBy_Float(bool byTarget)
	{
		this.StartCoroutine(CO_TestDOCompleteBy(byTarget, true));
	}
}