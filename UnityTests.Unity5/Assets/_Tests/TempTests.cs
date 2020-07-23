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
    public Rigidbody2D target;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        target.DOPath(new Vector2[] {new Vector2(2, 1), new Vector2(-2, -1)}, 3, PathType.CatmullRom); 
    }
}