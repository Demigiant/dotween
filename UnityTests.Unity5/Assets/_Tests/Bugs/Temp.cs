using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public RotateMode rotMode;
	public Transform target;
	Sequence s;

    IEnumerator Start()
    {
    	yield return new WaitForSeconds(0.6f);
    	s = DOTween.Sequence();
    	s.Insert(0.5f, target.DORotate(new Vector3(0,0,90f), 1f, rotMode).SetRelative());
    	s.Insert(1.5f, target.DORotate(new Vector3(0,90,0f), 1f, rotMode).SetRelative());
    	s.Insert(0.8f, target.DOMoveX(4, 1).SetRelative());
    	s.Insert(2f, target.DOMoveY(4, 1).SetRelative());
    	s.Pause();
    }

    void OnGUI()
    {
    	if (GUILayout.Button("Play")) s.Play();
    	if (GUILayout.Button("Goto 2.5")) s.Goto(2.5f);
    }
}