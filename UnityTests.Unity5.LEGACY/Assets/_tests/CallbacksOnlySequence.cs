using DG.Tweening;
using UnityEngine;
using System.Collections;

public class CallbacksOnlySequence : BrainBase
{
	public Transform target;

	void Start()
	{
		DOTween.Sequence()
			.OnComplete(()=>Debug.Log(target.position.x.ToString("N10")))
			// .SetLoops(3, LoopType.Yoyo)
			// .SetLoops(3)
			.AppendCallback(()=> Debug.Log("<color=#45DCF5>[" + Time.realtimeSinceStartup + "] Callback 0</color>"))
			.AppendInterval(1)
			// .Append(target.DOMoveX(1, 0.05f).SetRelative())
			.AppendCallback(()=> Debug.Log("<color=#45DCF5>[" + Time.realtimeSinceStartup + "] Callback FINAL</color>"));
	}
}