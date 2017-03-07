using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TempPro : MonoBehaviour
{
	public DOTweenPath target;

	IEnumerator Start()
	{
		Tween t = this.transform.DOMoveX(2, 2).OnRewind(()=> Debug.Log("Rewind 0"));
		yield return new WaitForSeconds(1);
		t.Rewind();
		// target.GetTween().Rewind();
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	public void OnComplete()
	{
		Debug.Log("COMPLETE");
	}

    public void OnRewind()
    {
        Debug.Log("REWIND");
    }
}