using DG.Tweening;
using System.Collections;
using UnityEngine;

public class VirtualTweens : BrainBase
{
	float sampleFloat;
	Vector3 vector = Vector3.zero;
	float startupTime;
	int delayedCalls;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);

		// DOVirtual.Float(0, 1, 3, UpdateCallback);

		// DOVirtual.DelayedCall(2, ()=> Debug.Log("<color=#00ff00>" + Time.realtimeSinceStartup + " > Wait call complete</color>"));

		startupTime = Time.realtimeSinceStartup;
		DOTween.Sequence()
			.Append(DOVirtual.DelayedCall(0.2f, RepeatCallback).SetLoops(10))
			.SetEase(Ease.OutQuint);
		// DOTween.Sequence()
		// 	.Append(
		// 		DOVirtual.DelayedCall(0.2f, RepeatCallback).SetLoops(10)
		// 	);
	}

	void UpdateCallback(float val)
	{
		vector.x = DOVirtual.EasedValue(15, 100, val, Ease.InQuad);
		vector.y = DOVirtual.EasedValue(15, 100, val, Ease.OutQuad);
		Debug.Log(vector);
	}

	void RepeatCallback()
	{
		delayedCalls++;
		Debug.Log("<color=#00FF00>" + (Time.realtimeSinceStartup - startupTime) + " > DELAYED CALL " + delayedCalls + "</color>");
	}
}