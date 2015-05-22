using UnityEngine;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core.Easing;

// Test class for EaseFactory feature created by Andrei Stanescu - https://github.com/reydanro
public class EaseFactoryTests : BrainBase
{
	public enum EaseType
	{
		Default,
		Ease,
		AnimationCurve
	}

	public int stopMotionFPS = 5;
	public EaseType easeType;
	public Ease ease = Ease.Linear;
	public AnimationCurve easeCurve;
	public Transform target;

	void Start()
	{
	    Tween t = target.DOMove(new Vector3(0,4,0), 2);
	    // Tween t = target.DORotate(new Vector3(0,180,0), 2);
	    if (easeType == EaseType.AnimationCurve) t.SetEase(EaseFactory.StopMotion(stopMotionFPS, easeCurve));
	    else if (easeType == EaseType.Ease) t.SetEase(EaseFactory.StopMotion(stopMotionFPS, ease));
	    else t.SetEase(EaseFactory.StopMotion(stopMotionFPS));
	    t.SetAutoKill(false).SetLoops(3, LoopType.Yoyo).Pause();
	}

	void OnGUI()
	{
		if (GUILayout.Button("TogglePause")) DOTween.TogglePauseAll();
		if (GUILayout.Button("Restart")) DOTween.RestartAll();
		if (GUILayout.Button("Complete")) DOTween.CompleteAll();
		if (GUILayout.Button("Rewind")) DOTween.RewindAll();
		if (GUILayout.Button("Flip")) DOTween.FlipAll();
		if (GUILayout.Button("Kill And Complete")) DOTween.KillAll(true);
	}
}