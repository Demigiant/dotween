using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Rigidbodies : BrainBase
{
	public Transform[] targets;
	public Rigidbody2D r2D;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		DOTween.Sequence()
			.Append(targets[0].rigidbody.DORotate(new Vector3(0, 0, 90), 1, RotateMode.WorldAxisAdd))
			.Append(targets[0].rigidbody.DORotate(new Vector3(90, 0, 0), 1, RotateMode.LocalAxisAdd))
			.Append(targets[0].rigidbody.DORotate(new Vector3(0, 810, 0), 1, RotateMode.LocalAxisAdd));
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("Rotate 2D 360")) r2D.DORotate(360, 1);
		if (GUILayout.Button("Rotate 2D 360 Relative")) r2D.DORotate(360, 1).SetRelative();

		DGUtils.EndGUI();
	}
}