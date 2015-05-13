using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Jump : BrainBase
{
	public Transform target;
	public Vector3 jump = new Vector3(4, 3, 0);
	public int numJumps = 1;
	public float duration = 1;
	public Ease ease = Ease.OutQuad;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		target.DOJump(jump, numJumps, duration).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
	}
}