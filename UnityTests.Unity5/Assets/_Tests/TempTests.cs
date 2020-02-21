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
    public Transform target;
    string[] empty;

    IEnumerator Start()
    {
        target.DOMoveX(3, 8).OnUpdate(
            ()=> Debug.Log(empty.Length)
        );
        yield return new WaitForSeconds(1);
        Destroy(target.gameObject);
    }
}