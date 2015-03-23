using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	void Start()
	{
		Debug.Log("Creating empty sequences");
		for (int i = 0; i < 100; ++i) {
			DOTween.Sequence();
		}
	}
}