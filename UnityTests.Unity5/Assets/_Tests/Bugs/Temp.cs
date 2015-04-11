using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public Transform target;

    IEnumerator Start()
    {
    	target.DOMoveX(2, 3).OnComplete(()=> {
    		Debug.Log("call");
    		target.GetComponent<TempMonoBehaviour>().Goco();
    		Debug.Log("after call");
		});

    	yield return new WaitForSeconds(1);

    	target.gameObject.SetActive(false);
    }
}