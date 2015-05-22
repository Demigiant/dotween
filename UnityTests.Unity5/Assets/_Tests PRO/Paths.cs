using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Paths : BrainBase
{
	public Transform target;

	void Start()
	{
		Vector3[] p = new[] {
			new Vector3(2,2,2),
			new Vector3(2,4,2),
			new Vector3(0,2,2),
		};
		target.DOPath(p, 4);
	}
}