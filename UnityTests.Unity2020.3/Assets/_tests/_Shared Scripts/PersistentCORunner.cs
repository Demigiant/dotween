// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2026/02/04

using System.Collections;
using UnityEngine;

public class PersistentCORunner : MonoBehaviour
{
    static MonoBehaviour I;

    void OnDestroy()
    {
        I = null;
        this.StopAllCoroutines();
    }

    static void Init()
    {
        if (I != null) return;

        GameObject go = new GameObject("PersistentCORunner");
        I = go.AddComponent<PersistentCORunner>();
        DontDestroyOnLoad(go);
    }
    
    public new static void StartCoroutine(IEnumerator coroutine)
    {
        Init();
        I.StartCoroutine(coroutine);
    }
}