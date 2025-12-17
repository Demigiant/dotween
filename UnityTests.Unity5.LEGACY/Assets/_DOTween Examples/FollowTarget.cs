using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FollowTarget : MonoBehaviour
{
	public Transform targetToFollow;
	public Transform follower;

	Vector3 prevTargetPos;
	Tweener followTween;

	void Start()
	{
		prevTargetPos = targetToFollow.position;
		followTween = follower.DOMoveX(targetToFollow.position.x, 2).SetAutoKill(false);
	}

	void Update()
	{
		if (prevTargetPos != targetToFollow.position) {
			prevTargetPos = targetToFollow.position;
			followTween.ChangeEndValue(targetToFollow.position, 2, true).Restart();
		}
	}
}