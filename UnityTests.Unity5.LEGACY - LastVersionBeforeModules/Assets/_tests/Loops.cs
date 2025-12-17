using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Loops : BrainBase
{
	public Transform[] targets;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		Vector3 rot = new Vector3(0, 0, 45);
		targets[0].DORotate(rot, 1).SetRelative().SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
		targets[1].DORotate(rot, 1).SetRelative().SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		targets[2].DORotate(rot, 1).SetRelative().SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
	}
}