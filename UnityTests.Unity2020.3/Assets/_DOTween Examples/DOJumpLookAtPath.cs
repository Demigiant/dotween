using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DOJumpLookAtPath : MonoBehaviour
{
    public Transform target;
    public float duration;
    public float jumpPower;
    public Vector3 jumpEnd;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.8f);

        float midP = duration * 0.25f; // approximate mid point when using an OutQuad ease
        Vector3 jumpPeak = jumpEnd - target.position + new Vector3(0, jumpPower, 0);
        Sequence s = DOTween.Sequence()
            .Join(target.DOJump(jumpEnd, jumpPower, 1, duration))
            .Join(target.DOLookAt(jumpPeak, midP * 0.05f)) // Make this faster so it looks almost immediately
            .Insert(midP, target.DOLookAt(jumpEnd, duration - midP, AxisConstraint.Z));
    }
}