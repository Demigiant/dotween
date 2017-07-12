using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Paths : BrainBase
{
	public Transform target;
	public DOTweenPath dotweenPath;

	void Start()
	{
		Vector3[] p = new[] {
			new Vector3(2,2,2),
			new Vector3(2,4,2),
			new Vector3(0,2,2),
		};
		target.DOPath(p, 4);

		// Log length of each DOTweenPath waypoint
		Debug.Log(dotweenPath.path.wpLengths.Length);
		for (int i = 0; i < dotweenPath.path.wpLengths.Length; ++i) {
			Debug.Log(i + " > " + dotweenPath.path.wpLengths[i]);
		}
	}
}