using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
	public Transform target;

    public void BlendableRotateX()
    {
    	target.DOBlendableRotateBy(new Vector3(10, 0, 0), 1);
    }

    public void BlendableRotateY()
    {
    	target.DOBlendableRotateBy(new Vector3(0, 10, 0), 1);
    }
}