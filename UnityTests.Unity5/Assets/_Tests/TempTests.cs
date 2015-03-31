using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;

public class TempTests : BrainBase
{
	int count;
	float fToTween = 14.2f;
	ulong uToTween = 512UL;
	long lToTween = 512L;
	ulong uFull = 18446744073709551615UL;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		Debug.Log(uFull.GetType() + " - " + uFull.ToString("N20"));
		Debug.Log(Convert.ToUInt64(uFull).GetType() + " - " + Convert.ToUInt64(uFull).ToString("N20"));
		Debug.Log((uFull * 0.9f).GetType() + " - " + (uFull * 0.9f).ToString("N20"));
		Debug.Log("------------------------------");
		Debug.Log(((ulong)(uFull * 0.9f)).GetType() + " - " + Convert.ToString((ulong)(uFull * 0.9f)));
		Debug.Log((Convert.ToUInt64(uFull * 0.9f)).GetType() + " - " + Convert.ToString(Convert.ToUInt64(uFull * 0.9f)));
		Debug.Log((uFull * 1f).ToString("N20"));
		Debug.Log("------------------------------");
		Debug.Log((((double)uFull * (double)0.9f)).GetType() + " - " + Convert.ToString(((double)uFull * (double)0.9f)));
		Debug.Log(((ulong)((double)uFull * (double)0.9f)).GetType() + " - " + Convert.ToString((ulong)((double)uFull * (double)0.9f)));
		Debug.Log("------------------------------");
		byte b = (byte)(uFull * 0.9f);
		Debug.Log(b);
		Debug.Log((ulong)b);
		Debug.Log("------------------------------");
		Debug.Log("------------------------------");
		Debug.Log((uFull * 0.9f).ToString("#"));
		Debug.Log(Convert.ToUInt64((uFull * 0.9f).ToString("#")));
		Debug.Log("------------------------------");
		Debug.Log(Math.Round(uFull * 0.9f));
		Debug.Log((ulong)Math.Round(uFull * 0.9f));
		Debug.Log(Convert.ToUInt64(Math.Round(uFull * 0.9f)));
		Debug.Log("------------------------------");
		Debug.Log(((float)Math.Round(uFull * 0.9f)).ToString("N30"));
		Debug.Log((ulong)((float)Math.Round(uFull * 0.9f)));
		Debug.Log("::::::::::::::::::::::::::::::");
		Debug.Log((ulong)(uFull * (decimal)0.9f));


		DOTween.To(()=> uToTween, x=> uToTween = x, 18446744073709551615UL, 2).OnComplete(()=> Debug.Log("Complete: " + Time.realtimeSinceStartup + " - " + uToTween.ToString("N0")));

		// DOTween.To(()=> lToTween, x=> lToTween = x, 8446744073709551615L, 2).OnComplete(()=> Debug.Log("Complete: " + Time.realtimeSinceStartup + " - " + lToTween.ToString("N0")));
		// DOTween.To(()=>fToTween, x=> fToTween = 0, 0, 2).OnComplete(()=> Debug.Log(fToTween.ToString("N30")));
	}

	void OnGUI()
	{
		GUILayout.Label(uToTween.ToString("N0"));
	}
}