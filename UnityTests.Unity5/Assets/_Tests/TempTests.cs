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
    public bool doQuaternion = false;

    IEnumerator Start()
    {
        target.eulerAngles = new Vector3(95, 0, 0);
        yield return new WaitForSeconds(1);
        if (doQuaternion) target.DORotateQuaternion(Quaternion.Euler(new Vector3(100, 0, 0)), 1);
        else target.DORotate(new Vector3(100, 0, 0), 1);
    }
}