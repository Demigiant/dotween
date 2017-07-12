using UnityEngine;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;

public class NestedSequencesCallbacks : BrainBase
{
	public float[] durations = new[] {
		0.2f, 0.2f, 5.16f, 3f, 0.4f
	};

	void Start()
	{
		Sequence timeline = DOTween.Sequence().OnComplete(()=> Debug.Log("timeline complete"));
		DOGetter<int> emptyGetter = () => 0;
		DOSetter<int> emptySetter = value => {};
		
		int count = durations.Length;
		for (int i = 0; i < count; ++i) {
			int id = i;
			float duration = durations[i];
			timeline.Append(
				DOTween.To(emptyGetter, emptySetter, 0, duration)
					.OnComplete(()=> Debug.Log(string.Format("step {0}/{1} ({2})", id, (count - 1), duration)))
			);
		}
	}
}