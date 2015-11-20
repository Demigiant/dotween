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
        DOTween.Init();
        PlayTween();
    }

    Sequence mySequence;

    Tween anchorMinTween = null;
    Tween anchorMaxTween = null;
    Tween anchorPositionTween = null;
    Tween scaleTween = null;

    void PlayTween()
    {
        mySequence = DOTween.Sequence();

        anchorMaxTween = DOTween.To(() => rectTrans.anchorMax, v => rectTrans.anchorMax = v, new Vector2(0.5f, 0.5f), 1f)
           // .Pause()
           .SetDelay(2f)
           .OnPlay(()=> Debug.Log(Time.realtimeSinceStartup + " Start MAX"))
            ;

        anchorMinTween = DOTween.To(() => rectTrans.anchorMin, v => rectTrans.anchorMin = v, new Vector2(0.5f, 0.5f), 1f)
           // .Pause()
           .SetDelay(2f)
           .OnPlay(() => Debug.Log(Time.realtimeSinceStartup + " Start MIN"))
            ;

        anchorPositionTween = DOTween.To(() => rectTrans.anchoredPosition, v => rectTrans.anchoredPosition = v, Vector2.zero, 1f)
            //.Pause()
            .SetDelay(2f)
            .OnPlay(() => Debug.Log(Time.realtimeSinceStartup + " Start POS"))
            ;

        scaleTween = DOTween.To(() => rectTrans.localScale, v => rectTrans.localScale = v, Vector3.zero, 1f)
            //.Pause()
            .SetDelay(2f)
            .OnPlay(() => Debug.Log(Time.realtimeSinceStartup + " Start SCALE"))
            ;

        mySequence
            .AppendInterval(5f)
            .Append(anchorMaxTween)
            .Join(anchorMinTween)
            .Join(anchorPositionTween)
            .AppendInterval(2f)
            .Append(scaleTween)
            //.SetDelay(2f)
            //.Play()
            .Pause()
            .OnPlay(()=> Debug.Log(Time.realtimeSinceStartup + " >>> PLAY"))
            ;
    }
}