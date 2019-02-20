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
    public Transform[] targets;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < targets.Length; ++i) {
            targets[i].DOMove(new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f), 0), 20)
                .SetId(i % 2 == 0 ? "a" : "b");
        }
        yield return new WaitForSeconds(1);
        List<Tween> ts = DOTween.TweensById("a");
        foreach (Tween t in ts) {
            t.Pause();
        }
    }
}