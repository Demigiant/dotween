using DG.Tweening;
using UnityEngine;
using System.Collections;

public class ReloadWithTimeScale0 : BrainBase
{
	public Transform target;
	public bool independentUpdate;

	void Start()
	{
		Time.timeScale = 1;
		target.DOMoveX(4, 2).SetRelative().SetUpdate(independentUpdate);
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("Set TimeScale to 0")) Time.timeScale = 0;
		if (GUILayout.Button("Set TimeScale to 1")) Time.timeScale = 1;

		DGUtils.EndGUI();
	}
}