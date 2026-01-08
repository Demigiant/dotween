using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SequenceGCAllocations : BrainBase
{
	public Transform target;

	void Start()
	{
		// DOTween.Sequence().Append(target.DOMoveX(2, 5));
		target.DOMoveX(2, 5);
	}
}