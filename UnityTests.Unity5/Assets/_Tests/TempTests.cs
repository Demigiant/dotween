using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class TempTests : BrainBase
{
	void OnMouseDown()
	{
		this.GetComponent<DOTweenAnimation>().DOPlay();
	}
}