using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class UGUI : MonoBehaviour
{
	public Image dotweenLogo, circleOutline;
	public Text text, relativeText, scrambledText;
	public Slider slider;

	void Start()
	{
		// All tweens are created in a paused state (by chaining to them a final Pause()),
		// so that the UI Play button can activate them when pressed.
		// Also, the ones that don't loop infinitely have the AutoKill property set to FALSE,
		// so they won't be destroyed when complete and can be resued by the RESTART button

		// Animate the fade out of DOTween's logo
		dotweenLogo.DOFade(0, 1.5f).SetAutoKill(false).Pause();

		// Animate the circle outline's color and fillAmount
		circleOutline.DOColor(RandomColor(), 1.5f).SetEase(Ease.Linear).Pause();
		circleOutline.DOFillAmount(0, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo)
			.OnStepComplete(()=> {
				circleOutline.fillClockwise = !circleOutline.fillClockwise;
				circleOutline.DOColor(RandomColor(), 1.5f).SetEase(Ease.Linear);
			})
			.Pause();

		// Animate the first text...
		text.DOText("This text will replace the existing one", 2).SetEase(Ease.Linear).SetAutoKill(false).Pause();
		// Animate the second (relative) text...
		relativeText.DOText(" - This text will be added to the existing one", 2).SetRelative().SetEase(Ease.Linear).SetAutoKill(false).Pause();
		// Animate the third (scrambled) text...
		scrambledText.DOText("This text will appear from scrambled chars", 2, true, ScrambleMode.All).SetEase(Ease.Linear).SetAutoKill(false).Pause();

		// Animate the slider
		slider.DOValue(1, 1.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).Pause();
	}

	// Called by PLAY button OnClick event. Starts all tweens
	public void StartTweens()
	{
		DOTween.PlayAll();
	}

	// Called by RESTART button OnClick event. Restarts all tweens
	public void RestartTweens()
	{
		DOTween.RestartAll();
	}

	// Returns a random color
	Color RandomColor()
	{
		return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
	}
}