using DG.Tweening;
using UnityEngine;

public class Clone : BrainBase
{
	public GameObject prefab;
	public AnimationCurve animCurve;

	void Start()
	{
		Transform t = NewTransform();
		Tween tween = t.DOMoveY(5f, 1)
			.SetDelay(2f)
			.SetRelative()
			.SetEase(animCurve)
			.OnStart(()=> Debug.Log("OnStart"))
			.SetLoops(-1, LoopType.Yoyo);
		for (int i = 0; i < 4; ++i) {
			t = NewTransform();
			Transform t2 = t;
			t2.DOMoveY(5f, 1).SetAs(tween);
		}
	}

	Transform NewTransform()
	{
		Transform t = ((GameObject)Instantiate(prefab)).transform;
		t.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
		return t;
	}
}