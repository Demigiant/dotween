using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;
 
public class TempTests : BrainBase
{
	public LoopType loopType = LoopType.Restart;
	public Transform target;

	void Start()
	{
		Sequence s = DOTween.Sequence();
		s.AppendCallback(()=> Debug.Log(">>>>> Start Callback"))
			.Append(target.DOMoveX(2, 1))
			.Append(target.DOMoveY(2, 1))
			.SetLoops(2, loopType)
			.OnStepComplete(()=> Debug.Log("Step > " + s.CompletedLoops()));
	}
}