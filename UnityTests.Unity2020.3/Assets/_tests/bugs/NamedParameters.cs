using UnityEngine;
using System.Collections;
using DG.Tweening;

public class NamedParameters : BrainBase
{
	public Transform target;

	void Start()
	{
		// target.DOMoveX(4, 1).SetUpdate(updateType: UpdateType.Normal, isIndependentUpdate: true);
		target.DOMoveX(4, 1).SetUpdate(isIndependentUpdate: true);
		LogSomething(alog: "passed");
		target.Log(msg: "t passed");
	}

	void LogSomething(string alog = "default")
	{
		Debug.Log(alog);
	}
}

public static class Exts
{
	public static T Log<T>(this T t, string msg) where T : Transform
	{
		Debug.Log(t + " > " + msg);
		return t;
	}

	public static T Log<T>(this T t, int i = 10, string msg = "default") where T : Transform
	{
		Debug.Log(t + " > " + i + ": " + msg);
		return t;
	}
}