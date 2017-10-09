using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TempPro : MonoBehaviour
{
    public Transform target;

    void Start()
    {
//        target.DOScale(2, 0.1f).SetLoops(2, LoopType.Yoyo).SetAutoKill(false).Pause();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(DOTween.TweensByTarget(target).Count);
            target.DOPlay();
        } else if (Input.GetKeyDown(KeyCode.E)) target.GetComponent<DOTweenAnimation>().DOPlay();
    }
}