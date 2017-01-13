using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
	public Transform target;

    IEnumerator Start()
    {
    	Tween t = target.DORotate(new Vector3(0, 0, 90), 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear)
    		.SetAutoKill(false).Pause();
    	t.SetUpdate(true);
    	t.OnUpdate(()=> Debug.Log("UPDATE " + t.Elapsed() + "/" + t.IsPlaying()));
    	yield return new WaitForSeconds(0.5f);

    	t.Complete();
    	// t.Rewind();
    	t.PlayBackwards();
    }
}