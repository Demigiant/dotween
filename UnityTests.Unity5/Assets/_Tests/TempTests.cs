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
	public uint test = 1000;
	public uint to = 0;

    void Start () {
        DOTween.To(()=> test, x=> test = x, to, 2.0f).OnUpdate(()=> Debug.Log("1000 to 0 > " + test));
    }
}