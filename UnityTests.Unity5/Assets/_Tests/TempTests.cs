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

        DOTween.Sequence()
            .Append(target.DOLocalJump(new Vector3(0, -2, 0), 2, 1, 1f))
            .AppendInterval(1)
            .Append(target.DOLocalJump(new Vector3(0, -2, 0), 2, 1, 1f));

        targetAlt.DOLocalJump(new Vector3(0, -2, 0), 2, 1, 1f)
            .OnComplete(() => targetAlt.DOLocalJump(new Vector3(0, -2, 0), 2, 1, 1f));
    }
}