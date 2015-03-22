using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class UIMisc : BrainBase
{
	public Image[] imgs;
	public Text text, textScramble;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);

		// Radial
		Sequence s = DOTween.Sequence();
		foreach (Image i in imgs) {
			Image img = i;
			img.fillAmount = 0;
			s.Insert(0, img.DOFillAmount(1, 1).SetEase(Ease.Linear))
				.Join(img.DOFade(0, 1).From().SetEase(Ease.Linear))
				.SetLoops(-1, LoopType.Yoyo);
		}
		s.OnStepComplete(()=> {
			foreach (Image img in imgs) img.fillClockwise = !img.fillClockwise;
		});

		// Text
		DOTween.Sequence()
			.AppendInterval(0.5f)
			.Append(text.DOText("", 2).From().SetEase(Ease.Linear))
			.AppendInterval(0.5f)
			.SetLoops(-1, LoopType.Yoyo);
		DOTween.Sequence()
			.AppendInterval(0.5f)
			.Append(textScramble.DOText("", 2, true).From().SetEase(Ease.Linear))
			.AppendInterval(0.5f)
			.SetLoops(-1, LoopType.Yoyo);
	}
}