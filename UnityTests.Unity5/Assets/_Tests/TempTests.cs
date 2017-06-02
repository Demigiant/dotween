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
    public Transform target;

    void Start()
    {
        target.DOShakeScale(2, 0.15f, 10, 90f, true);
    }
}