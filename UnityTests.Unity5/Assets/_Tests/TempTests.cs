using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class TempTests : BrainBase
{
	public Transform target;
	public Ease ease = Ease.Linear;

	void OnGUI()
	{
		if (GUILayout.Button("SHAKE")) {
			target.DOKill();
			target.DOShakePosition(4, 1, 3).SetEase(ease);
		}
	}
}