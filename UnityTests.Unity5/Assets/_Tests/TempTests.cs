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

        Tween t = target.DOMoveX(2, 2);
//        t.OnUpdate(t.Complete);
        t.OnComplete(()=> Debug.Log("COMPLETE"));
        yield return new WaitForSeconds(0.4f);

        t.Complete();
    }
}