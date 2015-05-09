using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TempPro : MonoBehaviour
{
	public Transform target;
	public Vector3[] path;

	void Start()
	{
		target.DOPath(path, 4);
	}
}