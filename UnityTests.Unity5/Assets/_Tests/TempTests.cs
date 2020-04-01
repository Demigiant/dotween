using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class TempTests : BrainBase
{
    public Transform target;

    void Start()
    {
        var prefab = Instantiate(target, transform);
        prefab.transform.DOScale(2.0f, 1.0f).SetDelay(2.0f);
        StartCoroutine(Wait(1.0f, () => Destroy(prefab.gameObject)));
    }

    IEnumerator Wait(float time, UnityAction callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy");
        target.DOMoveX(2, 1);
    }
}