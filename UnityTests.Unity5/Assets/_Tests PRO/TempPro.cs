using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TempPro : MonoBehaviour
{
	public DOTweenPath path;

    IEnumerator Start()
    {
    	yield return new WaitForSeconds(0.5f);

    	Debug.Log("PLAY");
    	Debug.Log(path.GetTween().id);
    	DOTween.Play("myId");
    }
}