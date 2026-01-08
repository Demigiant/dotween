using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DynamicTweens : MonoBehaviour
{
    public Transform doDynLookAtChild;
    public Transform doDynLookAt;
    public Transform doLookAt;
    public Transform lookAtTarget;
	
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        
        doDynLookAtChild.DODynamicLookAt(lookAtTarget.position, 5)
            .SetEase(Ease.Linear);
        doDynLookAt.DODynamicLookAt(lookAtTarget.position, 5)
            .SetEase(Ease.Linear);
        doLookAt.DOLookAt(lookAtTarget.position, 5)
            .SetEase(Ease.Linear);
    }
}