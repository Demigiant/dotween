using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
	public Button bt;
	
	AudioSource _aaa;
 
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		Debug.Log("aaa null in Start? " + (_aaa == null));
		
		bt.onClick.AddListener(OnClick);
	}
 
	void OnClick()
	{
		Tween t = transform.DORotate(new Vector3(0, 0, 50f), 2f);
		t.onComplete = delegate () 
		{
			Debug.Log("aaa null on complete? " + (_aaa == null));
			_aaa.Play();
			Debug.Log("Won't reach here.");
		};
	}
}