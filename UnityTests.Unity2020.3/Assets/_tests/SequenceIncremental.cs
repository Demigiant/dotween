using DG.Tweening;
using UnityEngine;
using System.Collections;

public class SequenceIncremental : BrainBase
{
	public Transform target;

	void Start()
	{
		DOTween.Sequence()
			.Append(target.DORotate(new Vector3(0, 45, 0), 1)
				.SetLoops(2, LoopType.Incremental)
			)
			.AppendInterval(0.5f)
			.SetLoops(3, LoopType.Incremental);
	}
}