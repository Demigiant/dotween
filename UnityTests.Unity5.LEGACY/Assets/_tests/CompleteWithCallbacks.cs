using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CompleteWithCallbacks : MonoBehaviour
{
	public Transform target;

	IEnumerator Start()
	{
		Sequence s = DOTween.Sequence();
		s.AppendCallback(()=> Debug.Log("Start callback"))
			.Append(target.DOMoveX(3, 1))
			.AppendCallback(()=> Debug.Log("Mid callback"))
			.Append(target.DOMoveY(3, 1))
			.AppendCallback(()=> Debug.Log("End callback"));
		
		yield return new WaitForSeconds(0.5f);

		Debug.Log("WAITED");
		s.Complete(true);
	}
}