// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2017/10/28 20:39
// License Copyright (c) Daniele Giardini

using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _Tests
{
    public class TempTests_Extras : MonoBehaviour
    {
        public Transform target;

        IEnumerator Start()
        {
            target.DOMoveX(2, 2).SetAutoKill(false);
            target.DOMoveY(2, 2).SetAutoKill(false);

            yield return new WaitForSeconds(1);

            Debug.Log("Are all playing forward: " + DOTweenExtras.IsPlayForwardByTarget(target));
            Debug.Log("Are all playing backwards: " + DOTweenExtras.IsPlayBackwardsByTarget(target));
            Debug.Log("Are all paused: " + DOTweenExtras.IsPausedByTarget(target));

            yield return new WaitForSeconds(1.2f);

            Debug.Log("Are all playing forward: " + DOTweenExtras.IsPlayForwardByTarget(target));
            Debug.Log("Are all playing backwards: " + DOTweenExtras.IsPlayBackwardsByTarget(target));
            Debug.Log("Are all paused: " + DOTweenExtras.IsPausedByTarget(target));
        }
    }
}