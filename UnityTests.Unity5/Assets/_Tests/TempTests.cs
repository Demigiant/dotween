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
    public Transform rotTarget;
    public Ease easeType = Ease.Linear;
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        target.DORotateQuaternion(rotTarget.rotation, 2).SetEase(easeType);
    }
}