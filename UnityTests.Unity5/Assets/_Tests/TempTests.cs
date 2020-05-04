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
    public Text target;

    IEnumerator Start()
    {
        Tween t = target.DOCounter(800000, 13000, 3).SetEase(Ease.Linear).Pause();
        yield return new WaitForSeconds(1);
        t.Play();
    }
}