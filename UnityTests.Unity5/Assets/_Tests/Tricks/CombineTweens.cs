using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CombineTweens : MonoBehaviour
{
	public Transform target;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.6f);

	    target.DOBlendableMoveBy(new Vector3(3, 3, 0), 3);
        target.DOBlendableMoveBy(new Vector3(-3, 0, 0), 1.5f).SetLoops(2, LoopType.Yoyo);
	}
}