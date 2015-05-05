using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public Transform target, follow;

	void Start()
	{
		Tweener tween = target.DOMove(follow.position, 2f)
            .SetEase(Ease.OutExpo);
       
        tween.OnUpdate(() =>
        {
            tween.ChangeEndValue(follow.position, true);
        });
	}
}