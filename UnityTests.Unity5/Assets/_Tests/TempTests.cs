using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
	public float randomness = 90;
	public bool fadeOut;
	public Transform target;

    public void Shake()
    {
    	DOTween.KillAll(true);
    	target.DOShakePosition(2, 2, 10, randomness, false, fadeOut);
    }
}