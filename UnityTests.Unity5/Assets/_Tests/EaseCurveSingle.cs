using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class EaseCurveSingle : MonoBehaviour
{
	public Transform[] targets;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		targets[0].DOMoveX(6, 1.5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Restart);
		targets[1].DOMoveX(6, 1.5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
	}
}