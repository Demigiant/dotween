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

    IEnumerator Start () {
        yield return new WaitForSeconds(0.5f);
        Tween t = target.DOMoveX(4, 2).SetRelative().SetAutoKill(false);
        yield return new WaitForSeconds(0.5f);
        t.PlayBackwards();
        yield return new WaitForSeconds(1f);
        t.Restart();
    }
}