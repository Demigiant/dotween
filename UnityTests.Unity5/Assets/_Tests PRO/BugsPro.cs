using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BugsPro : BrainBase
{
	void Start()
	{
		List<Tween> tweens = DOTween.TweensById("SomeId");
		Debug.Log(tweens.Count);
		foreach (Tween t in tweens) t.Play();
	}
}