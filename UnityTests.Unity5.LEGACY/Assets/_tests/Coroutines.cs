using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Coroutines : BrainBase
{
	public Transform[] targets;

	void Start()
	{
		foreach (Transform t in targets) {
			Tween tween = t.DOMove(new Vector3(Random.Range(10f, 10f), Random.Range(10f, 10f), 0), 2f).SetLoops(3).Pause();
			StartCoroutine(WaitForCompletion(t, tween));
			StartCoroutine(WaitForRewind(t, tween));
			StartCoroutine(WaitForKill(t, tween));
			StartCoroutine(WaitForStart(t, tween));
			StartCoroutine(WaitForElapsedLoops(t, tween));
			StartCoroutine(WaitForPosition(t, tween));
		}
	}

	IEnumerator WaitForCompletion(Transform t, Tween tween)
	{
		yield return tween.WaitForCompletion();

		Debug.Log(t + " complete");
	}

	IEnumerator WaitForRewind(Transform t, Tween tween)
	{
		yield return tween.WaitForRewind();

		Debug.Log(t + " rewinded");
	}

	IEnumerator WaitForKill(Transform t, Tween tween)
	{
		yield return tween.WaitForKill();

		Debug.Log(t + " killed");
	}

	IEnumerator WaitForElapsedLoops(Transform t, Tween tween)
	{
		yield return tween.WaitForElapsedLoops(2);

		Debug.Log(t + " loops elapsed");
	}

	IEnumerator WaitForPosition(Transform t, Tween tween)
	{
		yield return tween.WaitForPosition(3.5f);

		Debug.Log(t + " position reached");
	}

	IEnumerator WaitForStart(Transform t, Tween tween)
	{
		yield return tween.WaitForStart();

		Debug.Log(t + " started");
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("Play")) DOTween.PlayAll();
		if (GUILayout.Button("Kill")) DOTween.KillAll();
		if (GUILayout.Button("Rewind")) DOTween.RewindAll();

		DGUtils.EndGUI();
	}
}