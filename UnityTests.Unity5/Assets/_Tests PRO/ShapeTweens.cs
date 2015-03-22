using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ShapeTweens : BrainBase
{
	public float duration = 1;
	public int loops = 1;
	public LoopType loopType = LoopType.Yoyo;
	public Ease ease = Ease.Linear;
	public SpiralMode spiralMode;
	public float frequency = 4;
	public float speed = 1;
	public float depth = 0;
	public Vector3 direction = Vector3.up;
	public bool snapping;
	public Transform[] targets;

	void Start()
	{
		targets[0].DOSpiral(duration, direction, spiralMode, speed, frequency, depth, snapping)
			.SetEase(ease)
			.SetLoops(loops, loopType)
			.SetAutoKill(false)
			.Pause();
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("TogglePause")) DOTween.TogglePauseAll();
		if (GUILayout.Button("Restart")) DOTween.RestartAll();
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}
}