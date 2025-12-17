using DG.Tweening;
using UnityEngine;
using System.Collections;

public class AppendLookAt : MonoBehaviour
{
	public Transform target, lookAtTarget, moveToTarget;

	void Start()
	{
		DOTween.Sequence()
            .Append(target.DOMove(moveToTarget.position,3,false))
            .Append(target.DOLookAt(lookAtTarget.position,3));

		// DOTween.Sequence()
		// 	.Append(target.DORotate(new Vector3(0, 180, 0), 1).SetRelative())
		// 	.Append(target.DOMoveX(-3, 1).SetRelative())
		// 	.Append(target.DORotate(new Vector3(0, 45, 0), 1).SetRelative());
	}
}