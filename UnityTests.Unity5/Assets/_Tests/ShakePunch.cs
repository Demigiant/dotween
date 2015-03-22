using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakePunch : BrainBase
{
	public float duration = 1; // Shake duration
	public float shakePosStrength = 2; // Shake position power
	public Vector3 shakePosStrengthV3 = new Vector3(2,2,2);
	public float shakeRotStrength = 90; // Shake rotation power
	public Vector3 shakeRotStrengthV3 = new Vector3(90,90,90);
	public float shakeScaleStrength = 2; // Shake scale power
	public Vector3 shakeScaleStrengthV3 = new Vector3(2,2,2);
	public bool useVectorStrength;
	public int shakeVibrato = 10; // Shake iterations x seconds
	public float shakeRandomness = 90;
	public int punchVibrato = 10;
	public float punchElasticity = 1;
	public Vector3 punchDirection = Vector3.up;
	public Vector3 punchScale = new Vector3(2,2,2);
	public Vector3 punchRotation = new Vector3(0, 180, 0);
	public Transform[] targets;

	Tween shakePositionTween, shakeRotationTween, shakeScaleTween, punchPositionTween, punchScaleTween, punchRotationTween;

	void Start()
	{
		DOTween.defaultRecyclable = false;
		// DOTween.logBehaviour = LogBehaviour.Verbose;
		Camera.main.transform.LookAt(targets[0]);
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Shake Camera Position")) ShakePosition(true);
		if (GUILayout.Button("Shake Camera Position + LookAt")) ShakePosition(true, targets[0].position);
		if (GUILayout.Button("Shake Camera Rotation")) ShakeRotation(true);
		if (GUILayout.Button("Shake Camera All")) {
			ShakePosition(true);
			ShakeRotation(true);
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Shake Position")) ShakePosition();
		if (GUILayout.Button("Shake Rotation")) ShakeRotation();
		if (GUILayout.Button("Shake Scale")) ShakeScale();
		if (GUILayout.Button("Shake All")) {
			ShakePosition();
			ShakeRotation();
			ShakeScale();
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Punch Position")) PunchPosition();
		if (GUILayout.Button("Punch Scale")) PunchScale();
		if (GUILayout.Button("Punch Rotation")) PunchRotation();
		if (GUILayout.Button("Punch All")) {
			PunchPosition();
			PunchRotation();
			PunchScale();
		}
		if (GUILayout.Button("Punch All Semi-Random")) {
			PunchPosition(true);
			PunchRotation(true);
			PunchScale(true);
		}
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}

	void ShakePosition(bool isCamera = false, Vector3? lookAt = null)
	{
		shakePositionTween.Complete();

		shakePositionTween = isCamera
			? useVectorStrength
				? Camera.main.DOShakePosition(duration, shakePosStrengthV3, shakeVibrato, shakeRandomness)
				: Camera.main.DOShakePosition(duration, shakePosStrength, shakeVibrato, shakeRandomness)
			: useVectorStrength
				? targets[0].DOShakePosition(duration, shakePosStrengthV3, shakeVibrato, shakeRandomness)
				: targets[0].DOShakePosition(duration, shakePosStrength, shakeVibrato, shakeRandomness);
		if (isCamera && lookAt != null) {
			shakePositionTween.OnUpdate(()=> Camera.main.transform.LookAt((Vector3)lookAt));
		}
	}

	void ShakeRotation(bool isCamera = false)
	{
		shakeRotationTween.Complete();

		shakeRotationTween = isCamera
			? useVectorStrength
				? Camera.main.DOShakeRotation(duration, shakeRotStrengthV3, shakeVibrato, shakeRandomness)
				: Camera.main.DOShakeRotation(duration, shakeRotStrength, shakeVibrato, shakeRandomness)
			: useVectorStrength
				? targets[0].DOShakeRotation(duration, shakeRotStrengthV3, shakeVibrato, shakeRandomness)
				: targets[0].DOShakeRotation(duration, shakeRotStrength, shakeVibrato, shakeRandomness);
	}

	void ShakeScale()
	{
		shakeScaleTween.Complete();

		shakeScaleTween = useVectorStrength
			? targets[0].DOShakeScale(duration, shakeScaleStrengthV3, shakeVibrato, shakeRandomness)
			: targets[0].DOShakeScale(duration, shakeScaleStrength, shakeVibrato, shakeRandomness);
	}

	void PunchPosition(bool random = false)
	{
		punchPositionTween.Complete();

		punchPositionTween = targets[0].DOPunchPosition(random ? RandomVector3(-1, 1) : punchDirection, duration, punchVibrato, punchElasticity);
	}

	void PunchScale(bool random = false)
	{
		punchScaleTween.Complete();

		punchScaleTween = targets[0].DOPunchScale(random ? RandomVector3(0.5f, 1) : punchScale, duration, punchVibrato, punchElasticity);
	}

	void PunchRotation(bool random = false)
	{
		punchRotationTween.Complete();

		punchRotationTween = targets[0].DOPunchRotation(random ? RandomVector3(-180, 180) : punchRotation, duration, punchVibrato, punchElasticity);
	}

	Vector3 RandomVector3(float min, float max)
	{
		return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
	}
}