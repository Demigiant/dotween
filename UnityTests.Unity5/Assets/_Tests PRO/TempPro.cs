using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TempPro : MonoBehaviour
{
	public DOTweenAnimation anime;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space)) anime.DORestart(true);
	}

	public void OnCreated()
	{
		Debug.Log("Tween created > " + anime.tween);
	}

	public void Output()
	{
		Debug.Log("HERE");
	}
}