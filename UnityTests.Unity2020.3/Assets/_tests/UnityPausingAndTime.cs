using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UnityPausingAndTime : BrainBase
{
    public Transform independentTarget;
    public Transform target;

    IEnumerator Start()
    {
        Debug.Log("real: " + Time.realtimeSinceStartup + " - unscaled: " + Time.unscaledTime + "/" + Time.unscaledDeltaTime);

        yield return new WaitForSeconds(1);

        Debug.Log("real: " + Time.realtimeSinceStartup + " - unscaled: " + Time.unscaledTime + "/" + Time.unscaledDeltaTime);

        float startTime0 = Time.realtimeSinceStartup;
        float startTime1 = Time.unscaledTime;

        independentTarget.DOMoveX(3, 2f).SetUpdate(true).SetEase(Ease.Linear)
            .OnComplete(() => {
                Debug.Log("real: " + Time.realtimeSinceStartup + " ► " + (Time.realtimeSinceStartup - startTime0));
                Debug.Log("unscaled: " + Time.unscaledTime + " ► " + (Time.unscaledTime - startTime1));
            });

        target.DOMoveX(3, 2f).SetEase(Ease.Linear);
    }
}