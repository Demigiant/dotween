using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempTests : MonoBehaviour
{
    public Vector3 from;
    public Vector3 to;
    [Range(0, 1)]
    public float lifetimePercentage;
    public Ease easeType;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Vector3 result = DOVirtual.EasedValue(from, to, lifetimePercentage, easeType);
            Debug.Log(result);
        }
    }
}