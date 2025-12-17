using DG.Tweening;
using System.Collections;
using UnityEngine;

public class StartupSpeed : BrainBase
{
	const int maxIterations = 32000;
	Vector3[] toPositions = new Vector3[maxIterations];
	float[] toValues = new float[maxIterations];
	Transform[] transforms = new Transform[maxIterations];
	SampleData[] sampleDatas = new SampleData[maxIterations];

	float startupTime;

	void Start()
	{
		DOTween.Init(true, false, LogBehaviour.ErrorsOnly);
		DOTween.SetTweensCapacity(maxIterations, 0);

		Transform container = new GameObject("Container").transform;
		for (int i = 0; i < maxIterations; ++i) {
			toPositions[i] = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
			toValues[i] = Random.Range(1f, 100f);

			Transform t = new GameObject("T " + i).transform;
			t.parent = container;
			transforms[i] = t;

			sampleDatas[i] = new SampleData();
		}
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Startup 2,000 position tweens")) StartCoroutine(StartupPos(2000));
		if (GUILayout.Button("Startup 2,000 float tweens")) StartCoroutine(StartupFloats(2000));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Startup 5,000 position tweens")) StartCoroutine(StartupPos(5000));
		if (GUILayout.Button("Startup 5,000 float tweens")) StartCoroutine(StartupFloats(5000));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Startup 10,000 position tweens")) StartCoroutine(StartupPos(10000));
		if (GUILayout.Button("Startup 10,000 float tweens")) StartCoroutine(StartupFloats(10000));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Startup 20,000 position tweens")) StartCoroutine(StartupPos(20000));
		if (GUILayout.Button("Startup 20,000 float tweens")) StartCoroutine(StartupFloats(20000));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Startup 32,000 position tweens")) StartCoroutine(StartupPos(32000));
		if (GUILayout.Button("Startup 32,000 float tweens")) StartCoroutine(StartupFloats(32000));
		GUILayout.EndHorizontal();

		GUILayout.Label("Startup time: " + startupTime);

		DGUtils.EndGUI();
	}

	IEnumerator StartupPos(int tot)
	{
		DOTween.KillAll();
		yield return null;

		float time = Time.realtimeSinceStartup;
		for (int i = 0; i < tot; ++i) transforms[i].DOMove(toPositions[i], 2);
		startupTime = Time.realtimeSinceStartup - time;
	}

	IEnumerator StartupFloats(int tot)
	{
		DOTween.KillAll();
		yield return null;

		float time = Time.realtimeSinceStartup;
		for (int i = 0; i < tot; ++i) {
			SampleData d = sampleDatas[i];
			SampleData dd = d;
			DOTween.To(()=> dd.floatVal, x=> dd.floatVal = x, toValues[i], 2);
		}
		startupTime = Time.realtimeSinceStartup - time;
	}
}