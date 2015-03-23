using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MixedTweens : BrainBase
{
	public Transform target;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.6f);

	    target.DOBlendableMoveBy(new Vector3(3, 3, 0), 3).SetAutoKill(false);
        target.DOBlendableMoveBy(new Vector3(-3, 0, 0), 1.5f).SetLoops(2, LoopType.Yoyo).SetAutoKill(false);
	}

	void OnGUI()
	{
		if (GUILayout.Button("Toggle Pause")) DOTween.TogglePauseAll();
		if (GUILayout.Button("Restart")) DOTween.RestartAll();
		if (GUILayout.Button("Flip")) DOTween.FlipAll();
	}
}