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
        Tween t = target.DOMoveX(2, 4).Pause();
        yield return new WaitForSeconds(1);

        Debug.Log(t.Duration());
        Debug.Log(t.ElapsedPercentage());

    }
}