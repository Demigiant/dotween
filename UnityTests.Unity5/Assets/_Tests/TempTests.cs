using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class TempTests : BrainBase
{
    Stopwatch sw;
    Tween t;

    void Start()
    {
        sw = new Stopwatch();
        sw.Start();
        t = DOVirtual.DelayedCall(0.1f, TestDeleteMe).SetLoops(-1);
    }

    void TestDeleteMe()
    {
        Debug.Log("Test " + sw.ElapsedMilliseconds);
//        t.Kill();
    }
}