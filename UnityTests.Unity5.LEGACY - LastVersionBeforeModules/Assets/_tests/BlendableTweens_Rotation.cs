using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlendableTweens_Rotation : BrainBase
{
	public Transform targetRotate, targetLocalRotate;

    public void BlendableRotateX()
    {
    	targetRotate.DOBlendableRotateBy(new Vector3(10, 0, 0), 1);
    	targetLocalRotate.DOBlendableLocalRotateBy(new Vector3(10, 0, 0), 1);
    }

    public void BlendableRotateY()
    {
    	targetRotate.DOBlendableRotateBy(new Vector3(0, 10, 0), 1);
    	targetLocalRotate.DOBlendableLocalRotateBy(new Vector3(0, 10, 0), 1);
    }
}