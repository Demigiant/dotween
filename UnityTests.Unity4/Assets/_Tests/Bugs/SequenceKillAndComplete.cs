using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SequenceKillAndComplete : BrainBase
{
	public Transform target;

	Sequence sequence;

	IEnumerator Start()
	{
		sequence = DOTween.Sequence();
		sequence.Append(target.DOMoveX(3, 3).SetRelative());
		sequence.Join(target.DOMoveY(3, 3).SetRelative());

		yield return new WaitForSeconds(1.5f);

		sequence.Kill(true);
	}
}