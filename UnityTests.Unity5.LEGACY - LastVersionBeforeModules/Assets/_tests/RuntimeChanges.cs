using DG.Tweening;
using System;
using UnityEngine;

public class RuntimeChanges : BrainBase
{
	enum FollowMode
	{
		OnClick,
		Continuous
	}

	public bool speedBased;
	public GUIText txtDuration;
	public Transform[] targets;

	Tweener[] tweens = new Tweener[2];
	Vector3[] originalPos = new Vector3[2];
	float durationToApply = 2f;
	Vector3 prevMousePos;
	FollowMode followMode;
	string[] followModeList;

	void Start()
	{
		followModeList = Enum.GetNames(typeof(FollowMode));
		SetGUITexts();

		int len = targets.Length;
		tweens = new Tweener[len];
		originalPos = new Vector3[len];

		for (int i = 0; i < len; ++i) {
			originalPos[i] = targets[i].position;
			tweens[i] = targets[i].DOMove(Vector3.zero, durationToApply)
				.SetLoops(-1, LoopType.Yoyo).SetEase(speedBased ? Ease.Linear : Ease.OutQuint)
				.SetSpeedBased(speedBased)
				.Pause();
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P)) DOTween.TogglePauseAll();
		else if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
			durationToApply -= 0.25f;
			if (durationToApply < 0.25f) durationToApply = 0.25f;
			SetGUITexts();
		} else if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
			durationToApply += 0.25f;
			SetGUITexts();
		}

		if (followMode == FollowMode.Continuous || Input.GetMouseButtonDown(0)) {
			// Find mouse position to set as tween's new endValue
			Vector3 clickPos = Input.mousePosition;
			if (clickPos == prevMousePos) return;

			prevMousePos = clickPos;
			clickPos.z = -Camera.main.transform.position.z;
			clickPos = Camera.main.ScreenToWorldPoint(clickPos);

			// Change end value - snapStartValue
			tweens[0].ChangeEndValue(clickPos, durationToApply, true);
			// Change end value - NO snapStartValue
			tweens[1].ChangeEndValue(clickPos, durationToApply);
			// Change start value
			originalPos[2] = clickPos;
			tweens[2].ChangeStartValue(clickPos, durationToApply);
			// Change start and end value
			Vector3 newStartValue = new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), 0);
			originalPos[3] = newStartValue;
			tweens[3].ChangeValues(newStartValue, clickPos, durationToApply);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(Vector3.zero, 0.25f);
		foreach (Vector3 pos in originalPos) Gizmos.DrawWireCube(pos, Vector3.one);
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		followMode = (FollowMode)GUILayout.Toolbar((int)followMode, followModeList);

		DGUtils.EndGUI();
	}

	void SetGUITexts()
	{
		txtDuration.text = "Duration: " + durationToApply + " (-/+ to change)";
	}
}