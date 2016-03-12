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
    public Color32 color;

    void Start ()
    {
        DOTween.To(()=> color, x=> color = x, Color.red, 2)
        	.OnUpdate(()=> Debug.Log(color));
    }
}