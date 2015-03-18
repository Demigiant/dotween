using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using System.Collections;
using UnityEngine;

public class Paths_HOTween : MonoBehaviour
{
	public Axis lockPosition;
	public Axis lockRotation0, lockRotation1;
	public bool is2DPath, is2DSideScroller;
	public Transform[] targets;

	void Start()
	{
		HOTween.showPathGizmos = true;

		Vector3[] path = new[] {
			Vector3.zero,
			new Vector3(0,1,0),
			new Vector3(1,2,0),
			new Vector3(2,1,0),
			new Vector3(2,0,0)
		};

		Axis lockRotation = lockRotation0 | lockRotation1;

		PlugVector3Path plugPath = new PlugVector3Path(path, true).ClosePath().OrientToPath(0.1f, lockRotation).LockPosition(lockPosition);
		if (is2DPath) plugPath.Is2D(is2DSideScroller);
		HOTween.To(targets[0], 3, new TweenParms()
			.Prop("position", plugPath)
			.Ease(EaseType.Linear)
			.Loops(-1)
		).Pause();
		plugPath = new PlugVector3Path(path).ClosePath().LookAt(targets[2]).LockPosition(lockPosition);
		if (is2DPath) plugPath.Is2D(is2DSideScroller);
		HOTween.To(targets[1], 3, new TweenParms()
			.Prop("position", plugPath)
			.Ease(EaseType.Linear)
			.Loops(-1)
		).Pause();

		// Linear VS curved
		plugPath = new PlugVector3Path(path, true, PathType.Curved).ClosePath().LookAt(Vector3.zero).LockPosition(lockPosition);
		if (is2DPath) plugPath.Is2D(is2DSideScroller);
		HOTween.To(targets[2], 3, new TweenParms()
			.Prop("position", plugPath)
			.Ease(EaseType.Linear)
			.Loops(-1)
		).Pause();
		plugPath = new PlugVector3Path(path, true, PathType.Linear).ClosePath().OrientToPath(0.1f, lockRotation).LockPosition(lockPosition);
		if (is2DPath) plugPath.Is2D(is2DSideScroller);
		HOTween.To(targets[3], 3, new TweenParms()
			.Prop("position", plugPath)
			.Ease(EaseType.Linear)
			.Loops(-1)
		).Pause();

		// Linear VS curved top-down
		path = new[] {
			Vector3.zero,
			new Vector3(0,0,1),
			new Vector3(1,0,2),
			new Vector3(2,0,1),
			new Vector3(2,0,0)
		};
		plugPath = new PlugVector3Path(path, true, PathType.Curved).ClosePath().OrientToPath(0.1f, lockRotation).LockPosition(lockPosition);
		if (is2DPath) plugPath.Is2D(is2DSideScroller);
		HOTween.To(targets[4], 3, new TweenParms()
			.Prop("position", plugPath)
			.Ease(EaseType.Linear)
			.Loops(-1)
		).Pause();
		plugPath = new PlugVector3Path(path, true, PathType.Linear).ClosePath().OrientToPath(0.1f, lockRotation).LockPosition(lockPosition);
		if (is2DPath) plugPath.Is2D(is2DSideScroller);
		HOTween.To(targets[5], 3, new TweenParms()
			.Prop("position", plugPath)
			.Ease(EaseType.Linear)
			.Loops(-1)
		).Pause();
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Play")) HOTween.Play();
		if (GUILayout.Button("Pause")) HOTween.Pause();
		if (GUILayout.Button("Kill")) HOTween.Kill();
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}
}