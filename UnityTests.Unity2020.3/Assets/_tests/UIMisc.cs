using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class UIMisc : BrainBase
{
	public CanvasGroup cGroup;
	public Image[] imgs;
	public Text text, textScramble;
	public RectTransform circleOutT, circleInT;
	public RectTransform moveT;
	public RectTransform shakeT;

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

		// RectTransform
		// Rotate
		circleOutT.DORotate(new Vector3(0,0,360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
		circleInT.DORotate(new Vector3(0,0,-360), 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
		// Move
		// moveT.DOMoveX(50, 1, true).SetRelative().SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
		moveT.DOAnchorPos3D(new Vector2(50, 0), 1, true).SetRelative().SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);

		// Shake
		shakeT.DOShakeAnchorPos(2, new Vector2(100, 10));

		// Text
		DOTween.Sequence()
			.AppendInterval(0.5f)
			.Append(text.DOText("", 2).From().SetEase(Ease.Linear))
			.AppendInterval(0.5f)
			.SetLoops(-1, LoopType.Yoyo);
		DOTween.Sequence()
			.AppendInterval(0.5f)
			.Append(textScramble.DOText("", 2, true, ScrambleMode.Lowercase).From().SetEase(Ease.Linear))
			.AppendInterval(0.5f)
			.SetLoops(-1, LoopType.Yoyo);
	}

	void OnGUI()
	{
		if (GUILayout.Button("Fade CanvasGroup")) cGroup.DOFade(0, 1);
	}
}