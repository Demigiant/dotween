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
    public SpriteRenderer[] sprites;
    public Color32 startColor, changeStartColor, changeEndColor;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Start");
        Tweener t0 = sprites[0].DOColor(startColor, 5);
        Tweener t1 = sprites[1].DOColor(startColor, 5);
        Tweener t2 = sprites[2].DOColor(startColor, 5);
        yield return new WaitForSeconds(1);
        Debug.Log("Change");
        t0.ChangeEndValue(changeEndColor, 5f, true);
        t1.ChangeStartValue(changeStartColor, 5f);
        t2.ChangeValues(changeStartColor, changeEndColor, 5f);
    }
}