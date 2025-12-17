using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DebugMode : BrainBase
{
    public Transform target;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Destroy target then try to create tween");
        Destroy(target.gameObject);
        target.DOMoveX(2, 2);
    }
}
