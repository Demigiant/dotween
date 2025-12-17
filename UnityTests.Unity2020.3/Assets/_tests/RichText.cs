using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class RichText : BrainBase
{
	public float duration = 6;
	public bool speedBased = true;
	public ScrambleMode scrambleMode;
	public Text txtRichA, txtA;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.6f);

		TweenParams tp = new TweenParams().SetEase(Ease.Linear);
		tp.SetSpeedBased(speedBased);
		txtRichA.DOText("This is a <color=#ff0000>colored <color=#00ff00>text</color></color> and normal text. And this is a minor sign: <", duration, true, scrambleMode).SetAs(tp);
		txtA.DOText("This is a colored text and normal text", duration, true, scrambleMode).SetAs(tp);
	}
}