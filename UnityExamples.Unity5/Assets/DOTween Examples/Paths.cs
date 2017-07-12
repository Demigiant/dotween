using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Paths : MonoBehaviour
{
	public Transform target;
	public PathType pathType = PathType.CatmullRom;
	public Vector3[] waypoints = new[] {
		new Vector3(4, 2, 6),
		new Vector3(8, 6, 14),
		new Vector3(4, 6, 14),
		new Vector3(0, 6, 6),
		new Vector3(-3, 0, 0)
	};

	void Start()
	{
		// Create a path tween using the given pathType, Linear or CatmullRom (curved).
		// Use SetOptions to close the path
		// and SetLookAt to make the target orient to the path itself
		Tween t = target.DOPath(waypoints, 4, pathType)
			.SetOptions(true)
			.SetLookAt(0.001f);
		// Then set the ease to Linear and use infinite loops
		t.SetEase(Ease.Linear).SetLoops(-1);
	}
}