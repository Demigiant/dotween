using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AddTweenIndexOutOfRange : BrainBase
{
	public GameObject prefab;
	public Transform container;
	public int totObjs = 300;

	Transform[] ts;

	void Start()
	{
		ts = new Transform[totObjs];
		for (int i = 0; i < totObjs; ++i) {
			GameObject go = Instantiate(prefab) as GameObject;
			go.transform.position = RandomV3();
			go.transform.parent = container;
			ts[i] = go.transform;
		}
	}

	void OnGUI()
	{
		if (GUILayout.Button("Tween")) CreateTweens();
		if (GUILayout.Button("Kill All & Create Tweens")) {
			DOTween.KillAll();
			CreateTweens();
		}
		if (GUILayout.Button("Kill All & Create Tweens Combo")) {
			DOTween.KillAll();
			CreateTweens();
			DOTween.KillAll();
			CreateTweens();
			DOTween.KillAll();
			CreateTweens();
			DOTween.KillAll();
			CreateTweens();
		}
		if (GUILayout.Button("Kill All")) DOTween.KillAll();
	}

	void CreateTweens()
	{
		foreach (Transform trans in ts) trans.DOMove(RandomV3(), 2).SetLoops(-1, LoopType.Yoyo);
	}

	Vector3 RandomV3()
	{
		const float range = 7;
		return new Vector3(Random.Range(-range,range), Random.Range(-range,range), Random.Range(-range,range));
	}
}