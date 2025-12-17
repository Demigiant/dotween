using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IndexOutOfRange06 : BrainBase
{
    GameObject _go;
    Sequence _sequence;
    
    void Start()
    {
        _go = new GameObject("TEST");
        AnimateGO();
        _sequence.Complete(true);
    }
    
    void AnimateGO()
    {
        if (_sequence == null) {
            _sequence = DOTween.Sequence();
        } else {
            _sequence.onComplete = null;
            _sequence.Kill();
        }
        _sequence.Append(_go.transform.DOMoveX(1, 1));
        _sequence.OnComplete(AnimateGO);
    }
}