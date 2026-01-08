using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EmptySequences : BrainBase
{
    public Transform target;
    public bool nullifyTarget = false;
    public bool createSequenceA, createSequenceB, createTweenT;
    public bool appendAToParent, appendBToParent, appendTToParent;
    public int loopParent = 1;
    public LoopType loopType = LoopType.Restart;

    IEnumerator Start()
    {
        if (nullifyTarget) target = null;

        yield return new WaitForSeconds(0.75f);

        Sequence a = null;
        Sequence b = null;
        Tween t = null;

        if (createSequenceA) {
            a = DOTween.Sequence()
                .AppendCallback(() => Debug.Log("A callback 1/2"))
                .AppendInterval(1f)
                .AppendCallback(() => Debug.Log("A callback 2/2 (1 sec delay)"))
                .InsertCallback(0, () => Debug.Log("A callback 0 A"))
                .InsertCallback(0, () => Debug.Log("A callback 0 B"))
                .OnStart(() => Debug.Log("A OnStart"))
                .OnStepComplete(() => Debug.Log("A OnStepComplete"))
                .OnComplete(() => Debug.Log("A OnComplete"))
                .OnKill(() => Debug.Log("A OnKill"));
        }
        if (createSequenceB) {
            b = DOTween.Sequence()
                .Append(target.DOMoveX(3, 0))
                .OnStart(() => Debug.Log("B OnStart"))
                .OnStepComplete(() => Debug.Log("B OnStepComplete"))
                .OnComplete(() => Debug.Log("B OnComplete"))
                .OnKill(() => Debug.Log("B OnKill"));
        }
        if (createTweenT) {
            t = target.DOMoveY(3, 0)
                .OnStart(() => Debug.Log("T OnStart"))
                .OnStepComplete(() => Debug.Log("T OnStepComplete"))
                .OnComplete(() => Debug.Log("T OnComplete"))
                .OnKill(() => Debug.Log("T OnKill"));
        }
        if (appendAToParent || appendBToParent || appendTToParent) {
            Sequence parent = DOTween.Sequence();
            if (a != null && appendAToParent) parent.Append(a);
            if (b != null && appendBToParent) parent.Append(b);
            if (t != null && appendTToParent) parent.Append(t);
            if (loopParent > 1 || loopParent == -1) parent.SetLoops(loopParent, loopType);
            parent
//                .AppendInterval(1f)
                .AppendCallback(() => Debug.Log("PARENT callback 1/3 (1 sec delay)"))
//                .AppendInterval(1f)
                .AppendCallback(() => Debug.Log("PARENT callback 2/3 (1 sec delay after prev)"))
//                .AppendInterval(0.0001f)
                .AppendCallback(() => Debug.Log("PARENT callback 3/3 (0.0001 sec delay after prev, right at end of Sequence)"))
                .OnStart(() => Debug.Log("PARENT OnStart"))
                .OnStepComplete(() => Debug.Log("PARENT OnStepComplete"))
                .OnComplete(() => Debug.Log("PARENT OnComplete"))
                .OnKill(() => Debug.Log("PARENT OnKill"));
        }
    }
}