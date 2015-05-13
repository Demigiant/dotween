using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public Transform target;

	void Start()
	{
		for (int i = 0; i < 300; ++i) {
			DOTween.Sequence().AppendInterval(3).Append(target.DOMoveX(1, 1));
		}
	}
}