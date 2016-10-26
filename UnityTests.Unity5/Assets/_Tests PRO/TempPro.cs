using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TempPro : MonoBehaviour
{
	public Transform target;

    void Start ()
    {
        target.DOMoveX(3, 1).OnComplete(TempPro.OnCompleteCallback);
    }

    public static void OnCompleteCallback()
    {
        Debug.Log("Hello I'm public! And Static! Love me!");
    }
}