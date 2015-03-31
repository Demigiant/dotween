using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public Transform target;

    void Start()
    {
        target.DOMoveX(2,1)
            .OnUpdate(()=> Debug.Log(DOTween.IsTweening(target)))
            .OnComplete(()=> Debug.Log(DOTween.IsTweening(target)));
    }
}