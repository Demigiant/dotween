using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public Transform target;
	public bool independentUpdate = true;

	void Start()
	{
		target.DORotate(new Vector3(0, 180, 0), 2, RotateMode.Fast).SetUpdate(independentUpdate).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
	}

	void OnGUI()
	{
		if (GUILayout.Button("VSync 0")) QualitySettings.vSyncCount = 0;
		if (GUILayout.Button("VSync 1")) QualitySettings.vSyncCount = 1;
		if (GUILayout.Button("VSync 2")) QualitySettings.vSyncCount = 2;
	}
}