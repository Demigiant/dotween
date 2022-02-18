using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TempTests : MonoBehaviour
{
    public Transform target;
    
    void Start()
    {
        DOTween.Sequence()
            .Append(target.DOMoveX(2, 1).SetSpeedBased().SetLoops(-1));
    }
}