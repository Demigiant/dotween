using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
    private int killCounter = 0;

    private void Awake()
    {
        DOTween.Init(true, true, LogBehaviour.Verbose);
        DOTween.SetTweensCapacity(200, 125);
    }

    private void Start()
    {
        const float Delay = 2;
        DOTween.Sequence().AppendInterval(Delay).OnKill(OnKill);
        DOTween.Sequence().AppendInterval(Delay).OnKill(OnKill);
    }

    private void OnKill()
    {
        if (++killCounter == 2)
            StartCoroutine(Coroutine());
    }

    private IEnumerator Coroutine()
    {
        Sequence sequence = DOTween.Sequence().AppendInterval(2).OnKill(() => { });

        yield return new WaitForSeconds(1);

        Debug.Log("sequence.Kill()");
        sequence.Kill(); // IndexOutOfRangeException
    }
}