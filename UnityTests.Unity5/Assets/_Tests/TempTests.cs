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
        yield return new WaitForSeconds(0.8f);

        Sequence s = DOTween.Sequence();
//        s.Append(target.DOMoveX(2, 1));
        s.AppendCallback(() => {
                Debug.Log("Gonna pause here");
                s.Pause();
            })
            .AppendCallback(() => Debug.Log("Will move Y"))
            .Append(target.DOMoveY(2, 1));
    }
}