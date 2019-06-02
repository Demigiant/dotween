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
    public bool useFrom = true;
    public Transform target;
    
    IEnumerator Start()
    {
        if (!useFrom) {
            target.DOMoveX(2, 2);
        } else {
            Vector3 currentPosition = target.position;
            target.DOMoveX(2, 2).From().SetAutoKill(false);//.OnRewind(() => OnResetTransformFrom(transformToAnimate,currentPosition));
        }

        yield return new WaitForSeconds(3f);

        target.DORewind();
    }
}