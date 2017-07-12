using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using DG.Tweening;

public class AllocationsTests : BrainBase
{
	public enum AllocationTestType
	{
		DOMove,
		DOPath
	}

	public AllocationTestType testType;
	public Transform[] targets;
	public Vector3[] wps;

	Tween tween0;

	IEnumerator Start()
	{
		DOTween.Init();
		yield return new WaitForSeconds(1);

		switch (testType) {
		case AllocationTestType.DOMove:
			Profiler.BeginSample("Create tween");
			// Regular move tweens test
			Debug.Log("Create tween");
			tween0 = targets[0].DOMoveX(2, 1);
			Profiler.EndSample();
			yield return null;
			Profiler.BeginSample("Kill tween");
			Debug.Log("Kill tween");
			tween0.Kill();
			Profiler.EndSample();

			yield return null;

			Profiler.BeginSample("Create tween 2");
			// Regular move tweens test
			Debug.Log("Create tween 2");
			tween0 = targets[0].DOMoveX(2, 1);
			Profiler.EndSample();
			yield return null;
			Profiler.BeginSample("Kill tween 2");
			Debug.Log("Kill tween 2");
			tween0.Kill();
			Profiler.EndSample();
			break;
		case AllocationTestType.DOPath:
			// Regular move tweens test
			Debug.Log("Create tween");
			tween0 = targets[0].DOPath(wps, 1);
			yield return null;
			Debug.Log("Kill tween");
			tween0.Kill();
			break;
		}

		yield return null;
		yield return null;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPaused = true;
#endif
	}
}