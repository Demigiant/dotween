using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PathWaypointReached : BrainBase
{
	public Vector3[] waypoints;
	public float duration = 5;
	public bool relative = true;
	public bool closedPath;
	public PathType pathType;
	public LoopType loopType;
	public int loops = -1;
	public Transform[] targets;

	Tween[] pathTweens;

	void Start()
	{
		pathTweens = new Tween[targets.Length];

		pathTweens[0] = targets[0].DOPath(waypoints, duration, pathType).SetOptions(closedPath);
		pathTweens[0].SetRelative(relative)
			.SetEase(Ease.Linear)
			.SetLoops(loops, loopType)
			.SetAutoKill(false)
			.OnComplete(()=> Debug.Log(targets[0].name + " > complete"))
			.OnWaypointChange(x=> {
				Debug.Log(targets[0].name + " > waypoint reached: " + x + " > " + targets[0].position + " - completed loops: " + pathTweens[0].CompletedLoops());
				pathTweens[0].Pause();
			});
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.Label("Is backwards: " + pathTweens[0].isBackwards);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Play")) DOTween.PlayAll();
		if (GUILayout.Button("Flip")) DOTween.FlipAll();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Goto duration x 2")) DOTween.GotoAll(duration * 2);
		if (GUILayout.Button("Goto duration x 0.5")) DOTween.GotoAll(duration * 0.5f);
		if (GUILayout.Button("Goto WP 0")) pathTweens[0].GotoWaypoint(0);
		if (GUILayout.Button("Goto WP 2")) pathTweens[0].GotoWaypoint(2);
		if (GUILayout.Button("Goto WP 15")) pathTweens[0].GotoWaypoint(15);
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}
}