using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
	void Start()
	{
		Sequence TestSeq = DOTween.Sequence();

		TestSeq.Pause();

		TestSeq.Append(Tween1());
		TestSeq.AppendCallback(Log1);
		TestSeq.Append(Tween2());
		TestSeq.AppendCallback(Log2);
		TestSeq.Append(Tween3());
		TestSeq.AppendCallback(Log3);

		TestSeq.Play();
	}


	private Tween Tween1()
	{
		Debug.Log("TWEEN 1");
		return transform.DOMove(new Vector3(10f, 0f, 0f), 0.5f).Pause();
	}

	private Tween Tween2()
	{
		Debug.Log("TWEEN 2");
		return transform.DOMove(new Vector3(0f, 10f, 0f), 0.5f).Pause();
	}

	private Tween Tween3()
	{
		Debug.Log("TWEEN 3");
		return transform.DOMove(new Vector3(0f, 0f, 10f), 0.5f).Pause();
	}

	private void Log1()
	{
		Debug.Log("tween 1 completed");
	}

	private void Log2()
	{
		Debug.Log("tween 2 completed");
	}

	private void Log3()
	{
		Debug.Log("tween 3 completed");
	}
}