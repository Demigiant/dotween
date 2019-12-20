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
        yield return new WaitForSeconds(1);
        Sequence s = DOTween.Sequence().Append(target.DOMoveX(2, 1)).SetLoops(-1);
        yield return new WaitForSeconds(1f);
        s.Restart(true, 1);
    }
}