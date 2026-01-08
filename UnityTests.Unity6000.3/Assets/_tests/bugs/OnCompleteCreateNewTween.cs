using DG.Tweening;
using UnityEngine;
using System.Collections;

public class OnCompleteCreateNewTween : BrainBase
{
	public Transform target;

	void Start()
	{
		MakeTween();
	}

	void MakeTween()
	{
		target.position = Vector3.zero;
		target.DOMoveX(3, 0.1f).SetRelative().OnComplete(MakeTween);
	}
}