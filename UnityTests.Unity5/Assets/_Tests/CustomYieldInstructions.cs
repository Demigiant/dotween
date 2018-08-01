using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CustomYieldInstructions : BrainBase
{
    public Transform target;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        Debug.Log("01");
        yield return target.DOMoveX(2, 1).WaitForCompletion(true);
        Debug.Log("02");
    }
}