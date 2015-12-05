using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ShakeInverse : MonoBehaviour
{
	public Transform trans;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.6f);
		Tween t = trans.DOShakePosition(4, 2).SetAutoKill(false);
		t.Complete();
		t.PlayBackwards();
	}
}