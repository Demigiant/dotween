using DG.Tweening;
using UnityEngine;
using System.Collections;

public class TweenCreationInCoroutine : BrainBase
{
	public Transform[] targets;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		for (int i = 0; i < targets.Length; ++i) {
			// Doesnt work
			Transform t = targets[i];
			// DOTween.To(()=> t.position, x=> t.position = x, new Vector3(0, 2, 0), 1).SetRelative();
			t.DOMove(new Vector3(0, 2, 0), 1).SetRelative();
		}
		// CreateTweens();
	}

	void CreateTweens()
	{
		// Works
		for (int i = 0; i < targets.Length; ++i) {
			Transform t = targets[i];
			DOTween.To(()=> t.position, x=> t.position = x, new Vector3(0, 2, 0), 1).SetRelative();
		}
	}
}