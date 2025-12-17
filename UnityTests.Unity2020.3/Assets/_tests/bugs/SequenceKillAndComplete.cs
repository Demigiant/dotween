using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SequenceKillAndComplete : BrainBase
{
	public Transform target;

	Sequence sequence;

	IEnumerator Start()
	{
		sequence = DOTween.Sequence().OnComplete(()=> Debug.Log("SEQUENCE COMPLETE"));
		sequence.Append(target.DOMoveX(3, 3).SetRelative().OnComplete(()=> Debug.Log("Tween A Complete")));
		sequence.Join(target.DOMoveY(3, 3).SetRelative().OnComplete(()=> Debug.Log("Tween B Complete")));

		yield return new WaitForSeconds(1f);

		Debug.Log("<color=#00FF00>COMPLETE</color>");
		sequence.Kill(true);
	}
}