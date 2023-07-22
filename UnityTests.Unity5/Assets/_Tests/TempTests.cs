using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TempTests : BrainBase
{
	public Rigidbody2D rigibody2d;
    
	void Start()
	{
		Tweener t = rigibody2d.DOMove(new Vector2(2, 0), 2);
		bool valueChanged = false;
		t.OnUpdate(() =>
		{
			if (!valueChanged && rigibody2d.position.x > 1) {
				Debug.Log("Changing value");
				valueChanged = true;
				t.ChangeEndValue(new Vector2(-2, 0), 2, true);
			}
		});
	}
}