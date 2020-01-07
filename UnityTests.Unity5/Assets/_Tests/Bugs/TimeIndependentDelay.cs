using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TimeIndependentDelay : BrainBase
{
    public float startupDelay = 1;
    public bool timeScaleIndependent = true;
    public float tweensDelayStep = 0.15f;
    public Transform[] targets;

    IEnumerator Start()
    {
        if (startupDelay > 0) yield return new WaitForSeconds(startupDelay);
        Log("STARTUP", "00ff00");

        for (int i = 0; i < targets.Length; ++i) {
            float delay = i * tweensDelayStep;
            targets[i].DOMoveX(1, 1).SetDelay(delay).SetUpdate(timeScaleIndependent).SetEase(Ease.Linear)
                .OnStart(() => Log(string.Format("{0}: tween start after delay of {1}\"", i, delay)));
        }
    }

    void Log(string message, string color = null)
    {
        if (color != null) message = string.Format("<color=#{0}>{1}</color>", color, message);
        message = string.Format("{0} | {1} | {2} ::: {3}", Time.realtimeSinceStartup, Time.deltaTime, Time.unscaledDeltaTime, message);
        Debug.Log(message);
    }
}