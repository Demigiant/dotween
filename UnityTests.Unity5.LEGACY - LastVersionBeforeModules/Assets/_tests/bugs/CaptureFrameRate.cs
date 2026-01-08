using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CaptureFrameRate : BrainBase
{
	public bool captureFrameRate = true;
	public bool callInit = false;
	public Transform target;

	void Start()
	{
		if (captureFrameRate) Time.captureFramerate = 10; // TOO FAST ANIMATION (1 second)
		// if (captureFrameRate) Application.targetFrameRate = 10; // WORKS
 
 		if (callInit) DOTween.Init(); // Fixes fast animation (why?)

        DOTween.defaultTimeScaleIndependent = true;
        
 		float time = Time.realtimeSinceStartup;
        target.DOLocalMoveY(-450,5).OnComplete(()=> Debug.Log("> " + (Time.realtimeSinceStartup - time)));
	}
}