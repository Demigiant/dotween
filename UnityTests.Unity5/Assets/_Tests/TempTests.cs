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
    public Transform target;

    IEnumerator Start()
    {
        Tween t = target.DOMoveX(2, 1).OnPlay(()=>Debug.Log("PLAY")).SetAutoKill(false);
        t.SetDelay(1);

        yield return new WaitForSeconds(2.5f);

        t.Rewind();
        t.Play();
    }
}