using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IndexOutOfRange03 : MonoBehaviour
{
	public GameObject prefab;

	IEnumerator Start()
	{
		DOTween.Init(true);

		for (int i = 0; i < 100; ++i) {
			GameObject go = Instantiate(prefab);
			go.transform.DOMoveX(2, 4);
		}

		yield return new WaitForSeconds(2);

		Debug.Log("Complete all");
		DOTween.CompleteAll();
	}
}