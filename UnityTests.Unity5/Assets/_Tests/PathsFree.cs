using DG.Tweening;
using UnityEngine;
using System.Collections;

public class PathsFree : BrainBase
{
	public Transform target;
	public Ease ease = Ease.Linear;
	public PathType pathType;
	public bool closePath;
	public Vector3[] waypoints;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		target.DOPath(waypoints, 3, pathType).SetOptions(closePath).SetEase(ease);
	}
}