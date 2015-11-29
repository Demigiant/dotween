using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
    public RectTransform rectTrans;

    void Start()
    {
    	DOTween.Sequence().SetDelay(2)
    		// .AppendInterval(2)
    		.Append(rectTrans.DOAnchorPosX(100, 1).SetRelative());
    }
}