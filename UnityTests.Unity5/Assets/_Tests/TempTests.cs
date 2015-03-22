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
	public Transform target;

	void Start()
	{
		DOTween.To(()=>new Vector3(0,2,0), x=>target.position = x, new Vector3(2,4,2), 2);
	}
}