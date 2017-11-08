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

        Tween t = target.DOShakeRotation(5f, new Vector3(0f, 20f, 20f), 4, 10f, true);
//        Tween t = target.DOPunchRotation(new Vector3(0f, 0f, 20f), 5, 10);
    }
}