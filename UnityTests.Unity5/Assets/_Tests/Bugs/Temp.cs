using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public RotateMode rotMode;
	public Transform target;

    IEnumerator Start()
    {
    	yield return new WaitForSeconds(0.6f);
    	target.DORotate(new Vector3(0,0,20f), 3f, rotMode).SetRelative();
    }
}