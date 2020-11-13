using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TempTests : MonoBehaviour
{
    public Transform target;

    public float effectDuration = 0.3f;
    public Vector3 shakeStrength = new Vector3(10f, 0, 0);
    public int shakeVibrato = 20;
    public float shakeRandomness = 0;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) DoShake();
    }
    
    public void DoShake() {
        target.DOShakePosition(effectDuration, shakeStrength, shakeVibrato, shakeRandomness).SetLoops(1, LoopType.Restart);
    }
}