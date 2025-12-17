using DG.Tweening;
using UnityEngine;

public class PersistentComponent : BrainBase
{
	public Transform target;

	void OnDestroy()
	{
		Debug.Log("OnDestroy > Create tween");
		target.DOMoveX(3, 1);
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("Init")) DOTween.Init();

		DGUtils.EndGUI();
	}
}