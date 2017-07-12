using UnityEngine;
using System.Collections;
using DG.Tweening;

public class NegativeTimeScale : MonoBehaviour
{
	public Transform target;

	void Start()
	{
		Tween t = target.DOMoveX(7, 4).SetRelative().SetLoops(-1, LoopType.Yoyo);
		t.OnUpdate(()=> t.timeScale = 1);
	}
}