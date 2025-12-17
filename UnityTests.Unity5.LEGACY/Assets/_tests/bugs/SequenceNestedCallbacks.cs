using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SequenceNestedCallbacks : BrainBase
{
	public Transform target;

	void Start()
	{
		Sequence sMain = DOTween.Sequence()
			.OnComplete(()=> Debug.Log("MAIN > Complete"));
		sMain.AppendInterval(0.001f)
			.Append(target.DOMoveX(2, 0.0010001f).SetRelative().OnComplete(()=> Debug.Log("S0 > Complete")));
		// sMain.Append(target.DOMoveY(2, 1).SetRelative().OnComplete(()=> Debug.Log("S1 > Complete")))
		// 	.Append(target.DOMoveZ(2, 1).SetRelative().OnComplete(()=> Debug.Log("S2 > Complete")))
		// 	.Join(target.DORotate(new Vector3(0, 90, 0), 1).SetRelative().OnComplete(()=> Debug.Log("S3 > Complete")));
	}
}