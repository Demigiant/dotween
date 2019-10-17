using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempTests : BrainBase
{
    Tween _tScale, _tMove;

    void Start()
    {
        _tScale = transform.DOScale(0.5f, 1).SetLoops(999);
        _tMove = transform.DOMoveX(2, 1).SetLoops(999);
    }

    void OnDisable()
    {
        _tScale.Complete();
        _tMove.Complete();
    }
}