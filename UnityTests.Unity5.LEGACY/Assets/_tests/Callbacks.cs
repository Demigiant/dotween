using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Callbacks : BrainBase
{
	public Transform target;

	Tween tween;

	IEnumerator Start()
	{
		tween = target.DOMoveX(5, 5);
		yield return new WaitForSeconds(1f);

		Debug.Log("Set OnUpdate");
		tween.OnUpdate(Update0);
		tween.onUpdate += Update1;
		yield return new WaitForSeconds(1);

		Debug.Log("Clear OnUpdate0");
		tween.onUpdate -= Update0;
		yield return new WaitForSeconds(1);

		Debug.Log("Clear OnUpdate");
		tween.OnUpdate(null);
	}

	void Update0()
	{
		Debug.Log("UPDATE 0");
	}

	void Update1()
	{
		Debug.Log("   UPDATE 1");
	}
}