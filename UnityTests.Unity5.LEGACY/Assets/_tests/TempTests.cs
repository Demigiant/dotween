using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
	public Transform target;
	public Vector3 someScale = new Vector3(2, 2, 2);
	public Vector3 finalScale = new Vector3(4, 4, 4);
	
	Sequence _mainSeq;
	Tween _tween;

	protected override void Awake()
	{
		base.Awake();
		
		CreateAnimation();
	}

	protected override void Update()
	{
		base.Update();
		
		if (Input.GetKeyDown(KeyCode.LeftArrow)) PrepareAnimationForNewRun();
		else if (Input.GetKeyDown(KeyCode.Space)) _mainSeq.Play();
	}

	// This is triggered before every animation.
	// On the first run it shouldn't do anything, since it's already rewinded.
	void PrepareAnimationForNewRun()
	{
		Debug.Log("Rewinding... (Sequence is at: " + _mainSeq.fullPosition + ")");
		
		_mainSeq.Rewind();
		
		Debug.Log("Rewinded");
	}

	// This is a repeatable sequence that can be rewinded many times.
	void CreateAnimation()
	{
		Debug.Log("Create Animation...");
		
		_mainSeq = DOTween.Sequence().SetAutoKill(false).Pause().SetId("SEQUENCE").OnRewind(() => {
			Debug.Log("> OnRewind (should not be printed on the first run)");
		});

		_mainSeq.AppendCallback(() => Debug.Log("CALLBACK at 0"));
		
		_mainSeq.Append(target.DOMoveX(2, 1).SetId("Tween A")
			.OnUpdate(() => Debug.Log("UPDATE A"))
			.OnRewind(() => Debug.Log("REWIND A"))
		);
		
		_mainSeq.Append(target.DOMoveX(-2, 1).SetId("Tween B")
			.OnUpdate(() => Debug.Log("UPDATE B"))
			.OnRewind(() => Debug.Log("REWIND B"))
		);

		// // Note: Duration must be higher than 0f or it doesn't work.
		// // _mainSeq.Append(DOVirtual.Vector3(Vector3.zero, someScale, 0.01f, v => {
		// // 	Debug.Log("> Callback 1"); // <-- BUG IS HERE: This should not be logged on first Rewind()?
		// // 	target.localScale = v;
		// // }));
		// _mainSeq.AppendCallback(() => Debug.Log("> Callback 1"));
		//
		// _mainSeq.Append(DOVirtual.Vector3(someScale, finalScale, 0.5f, v => {
		// 	Debug.Log("> Callback 2"); // <-- Interestingly this is not triggered, so it's inconsistent.
		// 	target.localScale = v;
		// }));
  //       
		// // // I also created a third scale tween in the same way (not included here for brevity).
		// // // The 3rd log message is also not triggered (like the second).
		// // _mainSeq.Append(thirdTween);
		
		Debug.Log("Animation Created");
	}
}