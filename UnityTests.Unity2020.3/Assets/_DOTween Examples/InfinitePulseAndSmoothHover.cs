using UnityEngine;
using System.Collections;
using DG.Tweening;

public class InfinitePulseAndSmoothHover : MonoBehaviour
{
	Tween pulseTween;

	void Start()
	{
		// Create the pulse tween and pause it
		pulseTween = this.transform.DOScale(1.3f, 0.5f)
			.SetEase(Ease.InOutQuad)
			.SetLoops(-1, LoopType.Yoyo)
			.Pause();
	}

	void OnMouseEnter()
	{
		// Just play forward
		pulseTween.PlayForward();
	}

	void OnMouseExit()
	{
		// Find the current elapsed duration without considering loops,
		// then goto it and play the tween backwards.
		// I'm gonna use ElapsedDirectionalPercentage instead of Elapsed,
		// because I want Yoyo loops to be considered correctly inverted as they are
		// pulseTween.Goto(pulseTween.ElapsedDirectionalPercentage() * pulseTween.Duration(false));
		// pulseTween.PlayBackwards();

		// That said, I just added a SmoothRewind method so you can use that :P
		pulseTween.SmoothRewind();
	}
}