using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TempPro : MonoBehaviour
{
	public Rigidbody2D target;

	void Start()
	{
		target.transform.DOScale(2, 1).SetLoops(-1);
	}

	void FixedUpdate()
	{
		target.position += new Vector2(0.03f, 0);
	}
}