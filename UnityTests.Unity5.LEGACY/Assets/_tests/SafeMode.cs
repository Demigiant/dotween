using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SafeMode : BrainBase
{
	public Transform target0, target1;
	public GameObject nullGO;

	IEnumerator Start()
	{
		DOTween.Init();
		DOTween.useSafeMode = true;

		yield return new WaitForSeconds(0.7f);

		// Tween that will fail on callback
		target0.DOMoveX(4, 1).SetRelative().OnComplete(()=> Debug.Log(nullGO.transform));
		// Tween whose target will be destroyed at half tween
		target1.DOMoveX(4, 1).SetRelative();

		yield return new WaitForSeconds(0.5f);
		Destroy(target1.gameObject);
	}
}