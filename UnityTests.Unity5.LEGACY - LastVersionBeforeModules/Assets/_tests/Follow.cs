using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Follow : BrainBase
{
	public Transform target, follow;
	public float someInt;

	void Start()
	{
		Tweener tween = target.DOMove(follow.position, 2f)
            .SetEase(Ease.OutExpo);
       
        tween.OnUpdate(() =>
        {
        	// someInt = 0;
        	// someInt = follow.position.x + tween.ElapsedPercentage();
            // tween.ChangeEndValue(follow.position, true);
            SomeFunction<Vector3>(follow.position);
        });
	}

	void SomeFunction<T>(T obj)
	{
		someInt = 1;
	}
}