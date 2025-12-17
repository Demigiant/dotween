using DG.Tweening;
using UnityEngine;

public class TweenLookAt : BrainBase
{
	public AxisConstraint axisConstraint;
	public Transform target, lookAtTarget;

	Quaternion targetOrRot;
	Vector3 lookAtPos;

	void Start()
	{
		targetOrRot = target.rotation;
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("LookAt")) {
			// target.DOLookAt(lookAtTarget.position, 0.15f, axisConstraint, Vector3.up);
			
			lookAtPos = lookAtTarget.position;
			lookAtPos = target.InverseTransformPoint(lookAtPos);
			// lookAtPos.y = 0;
			lookAtPos = target.TransformPoint(lookAtPos);
			target.LookAt(lookAtPos, target.up);
		}
		if (GUILayout.Button("Reset")) target.rotation = targetOrRot;

		DGUtils.EndGUI();
	}

	void OnDrawGizmos()
	{
		// Gizmos.DrawLine(target.position, lookAtTarget.position);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(target.position, lookAtPos);
	}
}