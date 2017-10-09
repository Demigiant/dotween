using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class IndexOutOfRangeOnKill02 : MonoBehaviour
{
    // Can't replicate the error
    void Start()
    {
        Debug.Log("Start() ► Disabling safe mode");

        DOTween.Init(false, false);

        Tween t = transform.DOMoveX(2, 2);
        t.OnComplete(() => {
            Debug.Log("OnComplete()");
            TweenKiller();
            Destroy(gameObject);
        });
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy()");
        TweenKiller();
    }

    void TweenKiller()
    {
        Debug.Log("TweenKiller()");
        transform.DOKill(true);
    }
}