using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public float lastStepDuration = 0.4f; // 0.4 doesn't work, 0.2 does

	void Start()
	{
		DOTween.Init();
		 
		Sequence timeline = DOTween.Sequence();
		DOGetter<int> emptyGetter = () => 0;
		DOSetter<int> emptySetter = value => { };
		 
		timeline.Append(DOTween.To(emptyGetter, emptySetter, 0, 0.2f).OnComplete(() => Debug.LogWarning("step1")));
		timeline.Append(DOTween.To(emptyGetter, emptySetter, 0, 0.2f).OnComplete(() => Debug.LogWarning("step2")));
		timeline.Append(DOTween.To(emptyGetter, emptySetter, 0, 0.4f).OnComplete(() => Debug.LogWarning("step3")));
		timeline.Append(DOTween.To(emptyGetter, emptySetter, 0, 0.2f).OnComplete(() => Debug.LogWarning("step4")));
		timeline.Append(DOTween.To(emptyGetter, emptySetter, 0, lastStepDuration).SetId("last").OnComplete(() => Debug.LogWarning("step5"))).OnComplete(() => Debug.LogWarning("timeline"));
	}
}