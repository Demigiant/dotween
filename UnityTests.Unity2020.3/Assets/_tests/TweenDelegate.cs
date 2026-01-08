using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TweenDelegate : BrainBase
{
	public Transform[] targets;
	public Tween[] tweens;

	void Start()
	{
		tweens = new Tween[targets.Length];
		for (int i = 0; i < targets.Length; ++i) {
			tweens[i] = targets[i].DOMoveY(5, 2).SetRelative().SetLoops(-1, LoopType.Yoyo);
		}
	}

	void OnGUI()
	{
		if (GUILayout.Button("Assign delegates")) {
			for (int i = 0; i < tweens.Length - 1; ++i) DelegateTester.TogglePause += tweens[i].TogglePause;
		}
		if (GUILayout.Button("TogglePause via Delegate")) if (DelegateTester.TogglePause != null) DelegateTester.TogglePause();
		if (GUILayout.Button("Clear delegates")) {
			 // DelegateTester.Clear();
			DelegateTester.TogglePause = null;
		}
	}
}

public static class DelegateTester
{
	public delegate void TweenActionDelegate();
	public static TweenActionDelegate TogglePause;

	public static void Clear()
	{
		TogglePause = null;
	}
}