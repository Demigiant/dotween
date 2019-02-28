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
    Tween myTween;
    void Start()
    {
        myTween = target.DOMove(new Vector3(5, 5, 5), 25).SetAutoKill(false).SetUpdate(true);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            DOTween.timeScale -= 0.1f;
        }
        if (Input.GetKey(KeyCode.K))
        {
            DOTween.timeScale += 0.1f;
        }

        Debug.Log(DOTween.timeScale);
    }
}