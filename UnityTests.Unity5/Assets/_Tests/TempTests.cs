using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
	public Transform target;
	Tween tween;

	void Start()
	{
		tween = target.DOMoveX(2, 1).SetAutoKill(false).OnPlay(()=> Debug.Log("Play"));
	}

	void OnGUI()
	{
		if (GUILayout.Button("Restart")) tween.Restart();
		if (GUILayout.Button("Flip")) tween.Flip();
		if (GUILayout.Button("TogglePause")) tween.TogglePause();
	}
}