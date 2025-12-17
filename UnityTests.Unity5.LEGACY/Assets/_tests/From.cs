using DG.Tweening;
using System.Collections;
using UnityEngine;

public class From : BrainBase
{
	public GameObject prefab;

	Transform t;

	void LateUpdate()
	{
		if (t != null) Debug.Log(">>> " + t.position);
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("New FROM")) FromTween();

		DGUtils.EndGUI();
	}

	void FromTween()
	{
		t = ((GameObject)Instantiate(prefab)).transform;
		t.DOMove(new Vector3(0, 2, 0), 1)
			.From()
			.OnKill(() => Destroy(t.gameObject));

		Debug.Log(t.position);
	}
}