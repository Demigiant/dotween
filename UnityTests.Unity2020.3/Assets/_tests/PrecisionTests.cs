using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PrecisionTests : BrainBase
{
    public int intVal = 0;
    public int toIntVal = 2147483647;
    public double doubleVal = 0;
    public double toDoubleVal = 999999999999999;
    public float duration = 1;
    
    void Start()
    {
        DOTween.To(() => intVal, x => intVal = x, toIntVal, 1)
            .OnComplete(() => Debug.Log("intVal: " + intVal));
        DOTween.To(() => doubleVal, x => doubleVal = x, toDoubleVal, 1)
            .OnComplete(() => Debug.Log("doubleVal: " + doubleVal));
    }
}
