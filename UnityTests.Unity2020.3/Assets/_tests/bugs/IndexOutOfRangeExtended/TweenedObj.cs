// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2019/10/25

using System;
using DG.Tweening;
using UnityEngine;

public class TweenedObj : MonoBehaviour
{
    Tween t;

    void Start()
    {
        Debug.Log("Create OBJ tween");
        t = this.transform.DOScale(1.5f, 2).SetLoops(-1, LoopType.Yoyo);
    }

    void OnDestroy()
    {
        Debug.Log("Kill from OnDestroy obj " + this.name);
        t.Kill();
    }

    void OnDisable()
    {
        Debug.Log("DOTween.Clear from OnDisable obj " + this.name);
        DOTween.Clear();
    }
}