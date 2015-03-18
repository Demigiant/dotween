using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Paths : BrainBase
{
	public Ease ease = Ease.Linear;
	public AxisConstraint lockPosition;
	public AxisConstraint lockRotation0, lockRotation1;
	public LoopType loopType = LoopType.Yoyo;
	public PathMode pathMode;
	public int pathResolution = 10;
	public bool closePaths;
	public Vector3 forward = Vector3.forward;
	public Color[] pathsColors = new Color[2];
	public Transform[] targets;

	Tween controller;

	void Start()
	{
		return;

		Vector3[] path = new[] {
			new Vector3(0,1,0),
			new Vector3(1,2,0),
			new Vector3(2,1,0),
			new Vector3(2,0,0)
		};

		TweenParams tp = new TweenParams()
			.SetEase(ease)
			.SetLoops(-1, loopType);

		AxisConstraint lockRotation = lockRotation0 | lockRotation1;

		// Relative VS non relative
		controller = targets[0].DOPath(path, 3, PathType.CatmullRom, pathMode, pathResolution, pathsColors[0])
			.SetOptions(closePaths, lockPosition, lockRotation)
			.SetLookAt(0.1f, forward)
			.SetAs(tp)
			.SetRelative()
			.Pause();
		targets[1].DOPath(path, 3, PathType.CatmullRom, pathMode, pathResolution, pathsColors[1])
			.SetOptions(closePaths, lockPosition, lockRotation)
			.SetLookAt(targets[2], forward)
			.SetAs(tp)
			.Pause();

		// Linear VS curved
		targets[2].DOPath(path, 3, PathType.CatmullRom, pathMode, pathResolution, pathsColors[0])
			.SetOptions(closePaths, lockPosition, lockRotation)
			.SetLookAt(new Vector3(3, 0, 0), forward)
			.SetAs(tp)
			.SetRelative()
			.Pause();
		targets[3].DOPath(path, 3, PathType.Linear, pathMode, pathResolution, pathsColors[1])
			.SetOptions(closePaths, lockPosition, lockRotation)
			.SetLookAt(0.1f, forward)
			.SetAs(tp)
			.SetRelative()
			.Pause();

		// Linear VS curved no lookAt
		targets[4].DOPath(path, 3, PathType.CatmullRom, pathMode, pathResolution, pathsColors[0])
			.SetOptions(closePaths, lockPosition, lockRotation)
			.SetAs(tp)
			.SetRelative()
			.Pause();
		targets[5].DOPath(path, 3, PathType.Linear, pathMode, pathResolution, pathsColors[1])
			.SetOptions(closePaths, lockPosition, lockRotation)
			.SetAs(tp)
			.SetRelative()
			.Pause();

		// Linear VS curved top-down
		path = new[] {
			new Vector3(0,0,1),
			new Vector3(1,0,2),
			new Vector3(2,0,1),
			new Vector3(2,0,0)
		};
		targets[6].DOPath(path, 3, PathType.CatmullRom, pathMode, pathResolution, pathsColors[0])
			.SetOptions(closePaths, lockPosition, lockRotation)
			.SetLookAt(0.1f, forward)
			.SetAs(tp)
			.SetRelative()
			.Pause();
		targets[7].DOPath(path, 3, PathType.Linear, pathMode, pathResolution, pathsColors[1])
			.SetOptions(closePaths, lockPosition, lockRotation)
			.SetLookAt(0.1f, forward)
			.SetAs(tp)
			.SetRelative()
			.Pause();

		// Log lengths
		controller.ForceInit();
		Debug.Log("Controller path length: " + controller.PathLength());
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		DGUtils.GUIScrubber(controller);

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("TogglePause")) DOTween.TogglePauseAll();
		if (GUILayout.Button("Goto 1.5")) DOTween.GotoAll(1.5f);
		if (GUILayout.Button("Kill")) DOTween.KillAll();
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}
}