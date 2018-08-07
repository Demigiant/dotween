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
    public Transform target0;
    public Transform target1;

    void OnEnable()
    {
        this.StartCoroutine(CreateTweens());
    }

    IEnumerator CreateTweens()
    {
        Tween t0 = target0.DOBlendableLocalMoveBy(new Vector3(1, 2, 1), 2f);
        Tween t1 = target1.DOBlendableLocalMoveBy(new Vector3(1, -2, 1), 2f);

        yield return new WaitForSeconds(1);

        t0.Kill();
        t1.Goto(1.5f);
    }
}