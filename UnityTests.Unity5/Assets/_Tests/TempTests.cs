using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : MonoBehaviour
{
    public Text tf;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        DOTween.ToAlpha(() => tf.color, x => tf.color = x, 0, 1);
    }
}