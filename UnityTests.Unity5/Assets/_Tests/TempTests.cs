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
    public Transform target0, target1;

    IEnumerator Start()
    {
        while (true) {
            yield return null;
            target0.DOMoveX(1, 0.1f);
//            target1.DOMoveX(1, 0.1f);
        }
    }
}