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
    float initNum = 0;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        DOTween.To(()=>initNum,(x)=>initNum=x,400000000,3f).OnUpdate( ()=> Debug.Log(initNum));
    }
}