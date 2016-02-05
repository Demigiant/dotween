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
    public Transform target;

    void Start()
    {
        Sequence s = DOTween.Sequence()
            .Append(target.DOMoveX(2, 2));
    }
}