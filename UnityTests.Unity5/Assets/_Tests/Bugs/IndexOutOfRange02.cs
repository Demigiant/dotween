using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IndexOutOfRange02 : BrainBase
{
	public Transform[] targets;

	IEnumerator Start()
	{
		DOTween.Init (true, false, LogBehaviour.ErrorsOnly).SetCapacity (1000, 100);
		DOTween.defaultAutoPlay = AutoPlay.None;
		DOTween.defaultAutoKill = false;
		yield return new WaitForSeconds(1);

		foreach (Transform t in targets) t.DOMoveX(10, 10).Play();
		yield return new WaitForSeconds(2);

		DOTween.Clear(true);
	}

	void OnDestroy()
	{
		DOTween.Clear(true);
	}
}