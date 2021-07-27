using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempTests : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        target.DOMoveX(2, 1).SetLoops(-1, LoopType.Yoyo);
    }
}