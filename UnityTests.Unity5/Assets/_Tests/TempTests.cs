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
    public Transform target;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        target.DOLocalRotate(new Vector3(0, 0, 360), 2, RotateMode.FastBeyond360).SetRelative().SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        // target.DOLocalRotate(new Vector3(0, 0, 360), 2, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
}