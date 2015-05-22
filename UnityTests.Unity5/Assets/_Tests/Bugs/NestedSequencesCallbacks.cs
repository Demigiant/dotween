using UnityEngine;
using System.Collections;
using DG.Tweening;

public class NestedSequencesCallbacks : BrainBase
{
	public Transform target;
	int count;

	void Start()
	{
		int loops = 10;
		DOTween.Sequence()
			// .Append(DOTween.Sequence()
			// 	.InsertCallback(0.0000001f, ()=> Debug.Log("Nested Sequence callback"))
			// 	// .AppendInterval(0.5f)
			// 	.SetLoops(10)
			// )
			.Append(
				target.DOMoveX(3, 0.0000001f)
					.SetLoops(loops)
					.OnStepComplete(()=> {
						count++;
						Debug.Log("Nested Tween callback " + count + "/" + loops);
					})
			)
			.OnComplete(()=> Debug.Log("Sequence complete"));
	}
}