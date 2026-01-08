using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IndexOutOfRangeOnKill : BrainBase
{
	IEnumerator Start()
    {
        transform.DOScale(Vector3.zero, 1000).OnKill(() => transform.gameObject.SetActive(false));
        yield return new WaitForSeconds(1.5f);
        transform.DOKill();
        Debug.Log("Start() completed");
    }


    void OnDisable()
    {
    	Debug.Log("OnDisable()");
        transform.DOKill();
    }
}