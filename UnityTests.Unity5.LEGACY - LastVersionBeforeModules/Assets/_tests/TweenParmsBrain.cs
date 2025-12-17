using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TweenParmsBrain : BrainBase
{
	public Transform[] ts;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);

		TweenParams tp = new TweenParams()
			.SetRelative()
			// .SetSpeedBased()
			.SetEase(Ease.OutQuint)
			.SetLoops(-1, LoopType.Yoyo);

		foreach (Transform t in ts) {
			t.DOMoveY(2, 1).SetAs(tp);
		}
	}
}