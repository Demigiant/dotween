using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public Transform target;
	Vector3 orPos;

	void Start()
	{
		orPos = target.position;
	}

	void OnGUI()
	{
		if (GUILayout.Button("Default")) CreateTween();
		if (GUILayout.Button("0")) CreateTween(0);
		if (GUILayout.Button("1")) CreateTween(1);
		if (GUILayout.Button("2")) CreateTween(2);
		if (GUILayout.Button("3")) CreateTween(3);
	}

	void CreateTween(float amplitude = -1)
	{
		DOTween.KillAll();

		target.position = orPos;
		if (amplitude > 0) target.DOMoveX(3, 2).SetEase(Ease.OutElastic, amplitude);
		else target.DOMoveX(3, 2).SetEase(Ease.OutElastic);
	}
}