using DG.Tweening;
using UnityEngine;
using System.Collections;

public class PathsFree : BrainBase
{
	public Transform target;
	public bool useRigidbody;
	public bool useLocalPosition;
	public float duration = 3;
	public Ease ease = Ease.Linear;
	public PathType pathType;
	public bool closePath;
	public Vector3[] waypoints;

	void Start()
	{
		Tween t;
		if (useLocalPosition) {
			t = useRigidbody
				? target.GetComponent<Rigidbody>().DOLocalPath(waypoints, duration, pathType).SetOptions(closePath).SetLookAt(0.001f)
				: target.DOLocalPath(waypoints, duration, pathType).SetOptions(closePath).SetLookAt(0.001f);
		} else {
			t = useRigidbody
				? target.GetComponent<Rigidbody>().DOPath(waypoints, duration, pathType).SetOptions(closePath).SetLookAt(0.001f)
				: target.DOPath(waypoints, duration, pathType).SetOptions(closePath).SetLookAt(0.001f);
		}
		t.SetEase(ease)
			.OnWaypointChange(x=> Debug.Log("CHANGE > " + x + " - " + target.position));
		if (useRigidbody && !target.GetComponent<Rigidbody>().isKinematic) {
			t.OnPlay(()=> target.GetComponent<Rigidbody>().isKinematic = true);
			t.OnComplete(()=> target.GetComponent<Rigidbody>().isKinematic = false);
		}
	}
}