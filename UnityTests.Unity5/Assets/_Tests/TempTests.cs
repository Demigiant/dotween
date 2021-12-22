using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TempTests : MonoBehaviour
{
    public Transform target;
    
    IEnumerator Start()
    {
        TweenParams p = new TweenParams().SetId(12);
        yield return new WaitForSeconds(0.5f);
        target.DOMoveX(2, 2).SetAs(p);
        yield return new WaitForSeconds(1);
        DOTween.Complete(12);
    }
}