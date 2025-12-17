using DG.Tweening;
using UnityEngine;
using System.Collections;

public class ElasticDragAndRelease : MonoBehaviour
{
	public Transform target;
	Vector3 orPos;

	void Start()
	{
		orPos = target.position;
	}

	public void OnClick()
	{
		StartCoroutine(TweenCoroutine());
	}

	IEnumerator TweenCoroutine()
	{
		float range = 2;
		Vector3 rnd = new Vector3(Random.Range(-range, range), Random.Range(-range, range), target.position.z);
		target.position = orPos + rnd;

		yield return new WaitForSeconds(1);

		target.DOMove(orPos, 0.6f).SetEase(Ease.OutElastic, 4, 0);
	}
}