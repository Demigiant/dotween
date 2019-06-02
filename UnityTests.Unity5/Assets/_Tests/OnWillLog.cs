using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OnWillLog : BrainBase
{
    public bool allowLogs = false;
    public Transform target;

    IEnumerator Start()
    {
        DOTween.onWillLog = OnWillLogCallback;
        yield return new WaitForSeconds(0.5f);

        target.DOMoveX(3, 2);
        yield return new WaitForSeconds(0.5f);
        Destroy(target.gameObject);
    }

    bool OnWillLogCallback(LogType logType, object message)
    {
        Debug.Log("LOG CAUGHT > " + logType + " > " + message);
        return allowLogs;
    }
}