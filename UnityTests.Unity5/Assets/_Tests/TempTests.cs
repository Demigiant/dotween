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
    public Rigidbody target;
    public float duration = 1f;

    void Start()
    {
        target.transform.DOJump(new Vector3(2.5f, 1.5f, -4.5f), 2, 1, duration).SetDelay(1)
            .OnComplete(()=> Debug.Log(target.position));
    }
}