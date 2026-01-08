using DG.Tweening;
using UnityEngine;

public class TweenDuration : BrainBase
{
	public Transform target;

	void Start()
	{
		Tween();
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("Tween")) Tween();

		DGUtils.EndGUI();
	}

	void Tween()
	{
		float startTime = Time.realtimeSinceStartup;
		target.DOMove(target.position + new Vector3(1, 2, 1), 1)
			.OnStart(()=> Debug.Log("START > " + (Time.realtimeSinceStartup - startTime)))
			.OnUpdate(()=> Debug.Log("UPDATE > frameCount: " + Time.frameCount))
			.OnComplete(()=> Debug.Log("COMPLETE > " + (Time.realtimeSinceStartup - startTime)));
	}
}