using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PortraitToLandscapeAtRuntime : BrainBase
{
	public RectTransform target;
	Tween tween;

	void Start()
	{
		CreateRTTween();
		tween.Pause();
	}

	void OnGUI()
	{
		if (GUILayout.Button("Rewind")) DOTween.RewindAll();
		if (GUILayout.Button("Restart")) tween.Restart();
		if (GUILayout.Button("RT Recreate")) CreateRTTween();
		if (GUILayout.Button("T Recreate")) CreateTween();
	}

	void CreateRTTween()
	{
		tween = target.DOAnchorPos(new Vector2(0, 50), 2).SetRelative().SetAutoKill(false);
	}

	void CreateTween()
	{
		tween = ((Transform)target).DOMove(new Vector2(0, 50), 2).SetRelative().SetAutoKill(false);
	}
}