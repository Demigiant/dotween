using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
    IEnumerator Start ()
    {
    	yield return new WaitForSeconds(0.5f);

    	Debug.Log(Time.realtimeSinceStartup);
    	DOVirtual.DelayedCall(2, ()=> Debug.Log("Call > " + Time.realtimeSinceStartup))
    		.OnStart(()=> Debug.Log("Start > " + Time.realtimeSinceStartup));

    	DOTween.Sequence()
    		.OnStart(()=> Debug.Log("S Start > " + Time.realtimeSinceStartup))
    		.AppendInterval(2)
    		.OnComplete(()=> Debug.Log("S Complete > " + Time.realtimeSinceStartup));
    }
}