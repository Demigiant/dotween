using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PathEndOffset : MonoBehaviour
{
    public Transform target, toTarget, lookAt;

    void Start()
    {
        target.DOPath(new Vector3[]{ toTarget.position }, 5, PathType.CatmullRom, PathMode.Full3D)
            .SetLookAt(lookAt)
            .SetEase(Ease.InOutCubic);
    }
}