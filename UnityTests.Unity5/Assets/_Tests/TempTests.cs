using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempTests : MonoBehaviour
{
    public Vector3 startingPosition, endingPosition;
    public float p = 1;
    public Transform target;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.75f);
        target.position = startingPosition;
        Vector3[] pathVectors = new Vector3[3];
        pathVectors[0] = endingPosition;
        pathVectors[1] = startingPosition + (Vector3.up * p);
        pathVectors[2] = endingPosition + (Vector3.up * -p);
        target.DOPath(pathVectors, 0.4f, PathType.CubicBezier, PathMode.Full3D, 6, null)
            .OnStart(()=> Debug.Log("Start"))
            .OnComplete(()=> Debug.Log("Complete"));
    }
}