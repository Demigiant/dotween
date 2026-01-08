using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DOJumpRigidbody2D : MonoBehaviour
{
	public Rigidbody r3D;
	public Rigidbody2D r2D;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

     	r3D.DOJump(new Vector3(5f, 0, 0), 3f, 2, 2f, false);
		r2D.DOJump(new Vector2(5f, 0), 3f, 2, 2f, false);
		// r2D.transform.DOJump(new Vector2(5f, 0), 3f, 2, 2f, false);
	}
}