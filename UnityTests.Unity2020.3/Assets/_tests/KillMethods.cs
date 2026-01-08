using UnityEngine;
using System.Collections;
using DG.Tweening;

public class KillMethods : BrainBase
{
	public Transform[] targets;

	void Start()
	{
		for (int i = 0; i < targets.Length; ++i) {
			Tween t = targets[i].DOMoveX(3, 1).SetRelative().SetLoops(-1, LoopType.Yoyo);
			if (i < 2) t.SetId("ID 0-1");
			if (i == 2) t.SetId("ID 2");
		}
	}

	void OnGUI()
	{
		if (GUILayout.Button("KillAll except ID 0-1")) DOTween.KillAll(false, "ID 0-1");
		if (GUILayout.Button("KillAll except ID 2")) DOTween.KillAll(false, "ID 2");
		if (GUILayout.Button("KillAll except wrong ID")) DOTween.KillAll(false, "Wrong");
		if (GUILayout.Button("KillAll except target 3-4")) DOTween.KillAll(false, targets[3], targets[4]);
		if (GUILayout.Button("KillAll")) DOTween.KillAll();
	}
}