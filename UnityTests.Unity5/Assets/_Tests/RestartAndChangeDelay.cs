using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RestartAndChangeDelay : BrainBase
{
	public Transform target;

	Tween tween;

	void Start()
	{
		tween = target.DOMoveX(2, 1).SetDelay(2).SetAutoKill(false)
			.OnPlay(()=> Debug.Log("Play"));
	}

	public void Restart(float delay)
	{
		Debug.Log("Restart");
		tween.Restart(true, delay);
	}
	public void Restart()
	{
		Debug.Log("Restart");
		tween.Restart();
	}
}