using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class TempTests : BrainBase
{
    public Transform target;
    public float shakeStrength = 0.5f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        Debug.Log(target.position.x.ToString("N6"));
        target.DOShakePosition(2, shakeStrength, 10, 90, false, true).OnComplete(()=> Debug.Log(target.position.x.ToString("N6")));
    }
}