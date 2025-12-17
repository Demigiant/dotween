using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IndexOutOfRangeExtended : BrainBase
{
    public Transform[] targets;
    public Button btReload;

    readonly List<Tween> tweens = new List<Tween>();

    protected override void Awake()
    {
        base.Awake();

        btReload.onClick.AddListener(()=> SceneManager.LoadScene(SceneManager.GetActiveScene().name));

        Debug.Log("Create AWAKE tweens");
        for (int i = 0; i < targets.Length; i++) {
            Transform t = targets[i];
            int index = i;
            tweens.Add(
                t.DOMoveY(2, 3).SetLoops(-1, LoopType.Yoyo)
                    .OnComplete(() => DOTween.Clear())
                    .OnKill(()=> Debug.Log("Kill AWAKE tween " + index))
            );
        }
    }

    void Start()
    {
        Debug.Log("Create START tweens");
        for (int i = 0; i < targets.Length; i++) {
            Transform t = targets[i];
            int index = i;
            tweens.Add(
                t.DOMoveX(2, 3).SetLoops(-1, LoopType.Yoyo)
                    .OnComplete(() => DOTween.Clear())
                    .OnKill(()=> Debug.Log("Kill START tween " + index))
            );
        }
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F5)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDisable()
    {
        Debug.Log("OnDisable Brain");
        for (int i = 0; i < tweens.Count; i++) {
            Tween tween = tweens[i];
            tween.Kill();
            Debug.Log("Kill tween from list at index " + i);
        }
        tweens.Clear();
        DOTween.Clear();
    }

    void OnDestroy()
    {
//        foreach (Tween tween in tweens) tween.Kill();
//        tweens.Clear();
    }
}