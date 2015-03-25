using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public Transform target;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.6f);

        Sequence s0 = DOTween.Sequence().Append(target.DOMoveX(3, 2)).OnComplete(()=> Debug.Log("s0 complete"));
        Sequence s1 = DOTween.Sequence().Append(target.DOMoveY(3, 2)).OnComplete(()=> Debug.Log("s1 complete"));
        Sequence s = DOTween.Sequence().Append(s0).Append(s1).OnComplete(()=> Debug.Log("MAIN COMPLETE"));

        yield return new WaitForSeconds(0.2f);

        s.Complete();
    }
}