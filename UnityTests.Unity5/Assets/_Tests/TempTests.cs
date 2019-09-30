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
    Tween t;

    IEnumerator Start()
    {
        target.DOBlendablePunchRotation(new Vector3(120, 120, 120), 1);
        yield return new WaitForSeconds(0.3f);
        Debug.Log(target.eulerAngles);
        target.DOBlendablePunchRotation(new Vector3(200, 200, 200), 1);
    }
}