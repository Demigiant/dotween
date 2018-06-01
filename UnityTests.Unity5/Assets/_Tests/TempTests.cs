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
        yield return new WaitForSeconds(1);

        Sequence s = DOTween.Sequence()
            .OnStart(() => Debug.Log("START"))
            .Append(target.DOMoveX(2, 1).OnStart(() => Debug.Log("Append X")))
            .SetDelay(1)
            .Join(target.DOMoveY(2, 1).OnStart(() => Debug.Log("Join y")))
            .SetDelay(2)
            .OnComplete(()=> Debug.Log("COMPLETE"));
    }
}