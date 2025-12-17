using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TempMonoBehaviour : MonoBehaviour
{
	// IEnumerator Start()
 //    {
 //    	yield return new WaitForSeconds(1);

 //    	Debug.Log("Start");
 //    	transform.DOMoveX(2, 3).OnComplete(Goco);

 //    	yield return new WaitForSeconds(1);

 //    	Debug.Log("Deactivate");
 //    	this.gameObject.SetActive(false);
 //    }

	public void Goco()
    {
    	Debug.Log("Start Coroutine");
    	StartCoroutine(SomeCoroutine());
    }

    IEnumerator SomeCoroutine()
    {
    	Debug.Log("CO start");
    	yield return new WaitForSeconds(1);
    	Debug.Log("CO end");
    }
}