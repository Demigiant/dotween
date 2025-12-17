using DG.Tweening;
using UnityEngine;
using System.Collections;
using System;

public class SequenceEase : MonoBehaviour
{
	public Transform target;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);

		const float duration = 0.3f;
		DOTween.Sequence()
			.Append(target.DOMoveX(5, duration).SetEase(Ease.Linear))
			.Append(target.DOMoveX(-5, duration).SetEase(Ease.Linear))
			.Append(target.DOMoveX(5, duration).SetEase(Ease.Linear))
			.Append(target.DOMoveX(-5, duration).SetEase(Ease.Linear))
			.Append(target.DOMoveX(5, duration).SetEase(Ease.Linear))
			.Append(target.DOMoveX(-5, duration).SetEase(Ease.Linear));
	}
}