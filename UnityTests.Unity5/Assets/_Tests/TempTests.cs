using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
	public Transform target;

	Sequence sequence;

    void Start()
    {
    	sequence = DOTween.Sequence();
		sequence.Append(target.DORotate(new Vector3(0,45f,0), 1f).SetEase(Ease.InOutCubic));
		sequence.AppendInterval(2f);
		sequence.SetLoops(-1, LoopType.Incremental);
		sequence.SetRelative(true);
    }

    public void Pause()
    {
    	sequence.Pause();
    }

    public void Kil()
    {
    	sequence.Kill();
    }
}