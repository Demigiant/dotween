using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public Transform[] targets;

	void Start()
	{
		for (int i = 0; i < targets.Length; ++i) {
            Transform t = targets[i];
            Tweener tween = t.DOLocalMove(Vector3.zero, 1).SetLoops(4, LoopType.Restart).SetEase(Ease.Linear);
            float time = ((i+1) / (float)targets.Length);
            t.GetComponentInChildren<TextMesh>().text = i + "-" + time;
            tween.Goto(time, false);
        }
	}
}