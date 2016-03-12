using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FollowTargetUntilReached : MonoBehaviour
{
	public Transform targetToFollow;
	public Transform follower;

	void Start()
	{
		// Create tween and use OnUpdate to follow that target.
		// In this case, when the target is reached the tween will be
		// automatically killed and the following will stop
		Vector3 prevTargetPos = targetToFollow.position;
		Tweener followTween = follower.DOMove(targetToFollow.position, 4);
		followTween.OnUpdate(()=> {
			if (prevTargetPos != targetToFollow.position) {
				prevTargetPos = targetToFollow.position;
				followTween.ChangeEndValue(targetToFollow.position, 2, true).Restart();
			}
		});
		followTween.OnComplete(()=> Debug.Log("Target reached and tween killed"));
	}
}