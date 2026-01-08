using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
	public Transform target;
	float SpecialPulserStage;

	void Start()
	{
		var seq = DOTween.Sequence();
		seq.Append(DOTween.To(() => SpecialPulserStage, i => SpecialPulserStage = i, 0.8f, 0.8f));
		seq.Append(DOTween.To(() => SpecialPulserStage, i => SpecialPulserStage = i, 0, 0.8f));
		seq.SetLoops(-1);
		seq.OnUpdate(SpecialPulserUpdated);
	}

	void SpecialPulserUpdated()
	{
		Debug.Log("CALLED");
	}
}