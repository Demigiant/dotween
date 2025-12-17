using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SetTweensCapacity : BrainBase
{
    [SerializeField] bool _disableSecondSetCapacity = false;
    [SerializeField] Transform _target;
    
    void Start()
    {
        Debug.Log("NEXT: DOTween.Sequence()");
        Sequence sequence = DOTween.Sequence();
        LogInfo();
        Debug.Log("NEXT: SetTweensCapacity(0, 0)");
        DOTween.SetTweensCapacity(0, 0); // Set the tweens capacity to zero. The sequence becomes unmanaged but is still accessible
        LogInfo();
        if (!_disableSecondSetCapacity)
        {
            Debug.Log("NEXT: SetTweensCapacity(200, 50)");
            DOTween.SetTweensCapacity(200, 50); // If you comment out this line, DOTween.KillAll() will throw an IndexOutOfRangeException. It's another error
            LogInfo();
        }
        Debug.Log("NEXT: DOTween.KillAll()");
        DOTween.KillAll(); // Try to kill all tweens. The sequence is not killed and remains active
        LogInfo();
        Debug.Log("   - sequence active: " + sequence.active);
        Debug.Log("NEXT: sequence.Kill()");
        sequence.Kill(); // Manually kill the unmanaged sequence. You'll see error logs indicating totActiveDefaultTweens, totActiveTweens, and totActiveSequences are less than 0
        LogInfo();
        Debug.Log("   - sequence active: " + sequence.active);
        Debug.Log("NEXT: SetTweensCapacity(0, 0)");
        DOTween.SetTweensCapacity(0, 0);
        LogInfo();
        Debug.Log("NEXT: SetTweensCapacity(-1, -2)");
        DOTween.SetTweensCapacity(-1, -2);
        LogInfo();
        Debug.Log("NEXT: DOTween.Sequence()");
        sequence = DOTween.Sequence();
        LogInfo();
        Debug.Log("NEXT: DOTween.Sequence() with 2 tweens");
        sequence = DOTween.Sequence()
            .Join(_target.DOMoveX(-1, 1))
            .Join(_target.DOScale(2, 1).From());
        LogInfo();
    }

    void LogInfo()
    {
        Debug.Log("   - max: " + DOTween.tweenersCapacity + "/" + DOTween.sequencesCapacity + " - tot: " + DOTween.TotalActiveTweens() + "(" + DOTween.TotalActiveTweeners() + "/" + DOTween.TotalActiveSequences() + ")");
    }
}
