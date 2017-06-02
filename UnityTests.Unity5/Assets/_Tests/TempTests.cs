using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class TempTests : BrainBase
{
    public RectTransform target;

    void Start()
    {
        DOTween.Init();
    }

    public void Shake(float duration)
    {
        target.DOShakeScale(duration, 0.15f, 10, 90f, true)
			.SetEase(Ease.InOutBack)
			.Play();
    }
}