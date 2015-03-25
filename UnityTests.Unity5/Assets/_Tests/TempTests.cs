using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
 
public class TempTests : BrainBase
{
	public Transform target;
	public float delay;
	Vector2 originalPosition;
	Vector2 targetOriginalPosition;
	SpriteRenderer targetOverlay;
	Sequence seq;
	int prevCompletedLoops;

	IEnumerator Start() 
	{
		originalPosition = transform.position;
		DOTween.Init();

		yield return new WaitForSeconds(delay);

		StartAnimationWithTarget(target);

		yield return new WaitForSeconds(4);

		StartCoroutine(StopAnimation());
//		Invoke("StopAnimation", 5);
	}

	public void StartAnimationWithTarget(Transform target)
	{
		Debug.Log("StartAnimationWithTarget");

		// Prepare
		targetOriginalPosition = target.position;
		targetOverlay = target.Find("Sprite").GetComponent<SpriteRenderer>();

		// Execute
		Attack();
	}

	public IEnumerator StopAnimation()
	{
		Debug.Log("Stopping animation");
		Debug.Log("Completed loops: " + seq.CompletedLoops());
		int loopTo = seq.CompletedLoops() + 1;
		// yield return seq.WaitForElapsedLoops(seq.CompletedLoops() + 1);
		yield return seq.WaitForElapsedLoops(loopTo);

		Debug.Log("KILLING");

		seq.Kill(true);
	}

	void Attack()
	{	
		Vector2 direction = (targetOriginalPosition - originalPosition).normalized;

		float attackAnticipationTime = 0.05f;
		float attackMoveForwardTime = 0.08f;

		// Attacking unit movement
		seq = DOTween.Sequence();
		seq.Append(transform.DOBlendableMoveBy(-direction * 0.05f, attackAnticipationTime));
		seq.Append(transform.DOBlendableMoveBy(direction * 0.2f, attackMoveForwardTime));
		seq.Append(transform.DOBlendableMoveBy(-((-direction * 0.05f) + (direction * 0.2f)), 0.3f));

		// Target unit movement
		Sequence def = DOTween.Sequence();
		def.Append(target.DOBlendableMoveBy(direction * 0.2f, 0.05f));
		def.Append(target.DOBlendableMoveBy(-(direction * 0.2f), 0.3f));
		seq.Insert(attackAnticipationTime + attackMoveForwardTime, def);

		// Target unit hit flashing
		targetOverlay.color = new Color(1, 1, 1, 0);
		Sequence flash = DOTween.Sequence();
		flash.Append(targetOverlay.DOFade(0.7f, 0.05f));
		flash.Append(targetOverlay.DOFade(0, 0.5f)).SetEase(Ease.Linear);
		seq.Insert(attackAnticipationTime + attackMoveForwardTime, flash);

		seq.AppendInterval(1.5f);
		seq.SetLoops(-1);
	}

	void OnGUI()
	{
		if (seq != null) {
			if (seq.CompletedLoops() != prevCompletedLoops) Debug.Log("LOOP CHANGE FROM " + prevCompletedLoops + " TO " + seq.CompletedLoops());
			GUILayout.Label("completed loops: " + seq.CompletedLoops() + " (duration: " + seq.Duration(false) + ")");
			prevCompletedLoops = seq.CompletedLoops();
		}
	}
}