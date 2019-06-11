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
    public Transform target;
    
    IEnumerator Start()
    {
        Sequence s = DOTween.Sequence().SetAutoKill(false)
            .Join(target.DOMoveX(2, 1))
            .Append(target.DOMoveY(2, 1));
        yield return s.WaitForCompletion(true);

        s.PlayBackwards();
        s.PlayForward();
    }
}