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

    void Start()
    {
        int tot = 10;
        while (tot > 0) {
            target.DOMoveX(1, 5f).SetId("TW" + tot).SetLoops(-1, LoopType.Yoyo);
            DOTween.Sequence().Append(target.DOMoveY(2, 3f).SetId("INN" + tot)).SetLoops(-1, LoopType.Yoyo).SetId("SEQ" + tot);
            tot--;
        }
    }
}