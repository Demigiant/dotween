using DG.Tweening;
using System.Collections;
using UnityEngine;

public class VirtualTweens : BrainBase
{
	Vector3 vector = Vector3.zero;

	void Start()
	{
		DOVirtual.Float(0, 1, 3, UpdateCallback);

		DOVirtual.DelayedCall(2, ()=> Debug.Log("<color=#00ff00>" + Time.realtimeSinceStartup + " > Wait call complete</color>"));
	}

	void UpdateCallback(float val)
	{
		vector.x = DOVirtual.EasedValue(15, 100, val, Ease.InQuad);
		vector.y = DOVirtual.EasedValue(15, 100, val, Ease.OutQuad);
		Debug.Log(vector);
	}
}