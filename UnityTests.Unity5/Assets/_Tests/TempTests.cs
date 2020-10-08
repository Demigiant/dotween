using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TempTests : BrainBase
{
    public Transform target;
    public Transform targetAlt;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        // DOTween.Sequence()
        //     .Append(target.DOLocalJump(new Vector3(0, -2, 0), 2, 1, 1f))
        //     .AppendInterval(1)
        //     .Append(target.DOLocalJump(new Vector3(0, -2, 0), 2, 1, 1f));
        //
        // targetAlt.DOLocalJump(new Vector3(0, -2, 0), 2, 1, 1f)
        //     .OnComplete(() => targetAlt.DOLocalJump(new Vector3(0, -2, 0), 2, 1, 1f));

        Tween a = target.DOJump(new Vector3(2, 0, 0), 2, 1, 2f).SetAutoKill(true).Pause().SetId("Goto");
        Tween b = targetAlt.DOJump(new Vector3(2, 0, 0), 2, 1, 2f).SetAutoKill(true).SetId("Play");
        
        while (a.IsActive() && !a.IsComplete()) {
            yield return null;
            a.Goto(a.position + Time.deltaTime);
        }
    }
}