using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
    public int poolId;
    public Transform target;
    private Transform myTransform;
    private Color baseColor;
    void Awake () {
        myTransform = transform;
    }

    public void Show (string txt) {
        myTransform.SetAsLastSibling ();
        this.StartCoroutine(RemoveIn());
    }

    IEnumerator RemoveIn () {
        yield return new WaitForSeconds(4);
        Hide ();
    }

    private void Hide ()
    {
        target.DOMoveX(2, 1).OnComplete (Remove);
    }

    private void Remove ()
    {
        target.gameObject.SetActive(false);
//        Destroy(target.gameObject);
    }

    void OnDisable ()
    {
        target.DOKill();
    }
}