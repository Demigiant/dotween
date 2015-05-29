using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
	public Transform targetA, targetB;

	public DOGetter<Vector3> getterA, getterB;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		getterA = ()=> targetA.position;
		getterB = ()=> targetB.position;

		Debug.Log(getterA == getterB);
		Debug.Log(getterA.Equals(getterB));

		float time = Time.realtimeSinceStartup;
		Debug.Log(CompareGetters(getterA, getterB));
		float elapsed = Time.realtimeSinceStartup - time;
		Debug.Log("Compare executed in " + elapsed + " seconds");
	}

	bool CompareGetters<T>(DOGetter<T> a, DOGetter<T> b)
	{
		var aBody = a.Method.GetMethodBody().GetILAsByteArray();
		var bBody = b.Method.GetMethodBody().GetILAsByteArray();

		int aLen = aBody.Length;
		Debug.Log("LEN: " + aLen);
		if (aLen != bBody.Length) return false;

		for(int i = 0; i < aLen; i++) {
			if(aBody[i] != bBody[i]) return false;
		}
		return true;
	}
}