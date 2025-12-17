using UnityEngine;
using System.Collections;
using DG.Tweening;

public class LocalAxisTweens : BrainBase
{
	public Transform[] targets;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.6f);
		
		foreach (Transform t in targets) t.DOLocalMoveX(2, 2);
	}
}