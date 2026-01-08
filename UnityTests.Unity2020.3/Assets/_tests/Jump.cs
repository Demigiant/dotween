using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Jump : BrainBase
{
	public Transform target;
	public Vector3 jump = new Vector3(4, 3, 0);
	public float jumpHeight = 3;
	public int numJumps = 1;
	public float duration = 1;
	public Ease ease = Ease.OutQuad;
	public int loops = -1;
	public bool isRelative;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		Tween t = target.DOJump(jump, jumpHeight, numJumps, duration).SetEase(ease).SetLoops(loops, LoopType.Yoyo);
		if (isRelative) t.SetRelative();
	}
}