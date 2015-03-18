using DG.Tweening;
using DG.Tweening.Plugins;
using UnityEngine;

public class Basics : BrainBase
{
	public bool tweenLightAndCam = true;
	public int loops = 100000;
	public LoopType loopType = LoopType.Yoyo;
	public Ease ease = Ease.OutQuad;
	public AnimationCurve easeCurve;
	public Vector3 toRotation = new Vector3(0, 180, 0);
	public Color toCamColor, toLightColor, toSpriteColor;
	public Transform[] targets;
	public Light mainLight;
	public SpriteRenderer spriteRenderer;
	public GameObject specularSphere;
	public Transform targetToDoLookAt, targetToDoLookFrom;
	public GUITexture guiTexAlpha, guiTexColor;
	public GUIText txtInfo, txtFloat, txtInt, txtUint, txtVector2, txtVector4, txtRect, txtRectOffset, txtString0, txtString1, txtString2;
	public GameObject txtBackwards;

	int intId = 4;
	string stringId = "hello";
	Tween[] tweens;

	int intToTween;
	uint uintToTween;
	float floatToTween;
	Vector2 vector2toTween;
	Vector4 vector4toTween;
	Rect rectToTween;
	RectOffset rectOffsetToTween;
	string stringToTween0 = "Short text", stringToTween1 = "Long text to show how it gets completely replaced", stringToTween2 = "Relative text... ";

	void Start()
	{
		DOTween.logBehaviour = LogBehaviour.ErrorsOnly;

		txtInfo.text = txtInfo.text.Replace("#N", loops.ToString("N0"));

		// Set RectOffset since it can't be set before
		rectOffsetToTween = new RectOffset(0, 0, 0, 0);

		TweenParams tp = new TweenParams();
		tp.SetLoops(loops, loopType).SetAutoKill(false);
		if (ease == Ease.INTERNAL_Custom) tp.SetEase(easeCurve);
		else tp.SetEase(ease);

		// Transform tweens
		tweens = new Tween[targets.Length - 1];
		for (int i = 0; i < targets.Length; ++i)
		{
			Transform t = targets[i];
			switch (i) {
			case 0:
				tweens[i] = DOTween.To(()=> t.position, x=> t.position = x, new Vector3(0, 5f, 0), 1.5f).SetAs(tp).SetRelative().SetUpdate(true).SetAutoKill(true);
				break;
			case 1:
				// Red cube (rotation)
				tweens[i] = DOTween.To(()=> t.rotation, x=> t.rotation = x, toRotation, 1.5f).SetAs(tp).SetRelative();
				break;
			case 2:
				tweens[i] = DOTween.To(()=> t.position, x=> t.position = x, new Vector3(0, 5f, 0), 1.5f).SetAs(tp).SetOptions(true).SetRelative().SetAutoKill(true);
				break;
			case 3:
				// Vector3Array (not stored)
				Vector3[] path = new[] {
					new Vector3(1,0,0), new Vector3(0,1,0), new Vector3(1,0,0), new Vector3(0,-1,0)
				};
				float[] durations = new[] { 0.5f, 0.5f, 0.5f, 0.5f };
				DOTween.ToArray(() => t.position, x => t.position = x, path, durations)
					.SetAs(tp).SetRelative().Pause();
				continue;
			}
			Tween tween = tweens[i];
			tween.OnStart(()=> Debug.Log("OnStart: " + t.name))
				.OnPlay(()=> Debug.Log("OnPlay: " + t.name))
				.OnPause(()=> Debug.Log("OnPause: " + t.name))
				.OnComplete(()=> Debug.Log("OnComplete: " + t.name))
				.Pause();
			switch (i) {
			case 0:
				tween.SetId(intId);
				// tween.OnStart(()=>tweens[2].Kill());
				break;
			case 1:
				tween.SetId(stringId);
				break;
			case 2:
				tween.SetId(this);
				break;
			}
		}
		// Additional tweens //////////////////////////
		// Float
		DOTween.To(()=> floatToTween, x=> floatToTween = x, 100, 1.5f).SetAs(tp).Pause();
		// Int
		DOTween.To(()=> intToTween, x=> intToTween = x, 100, 1.5f).SetAs(tp).Pause();
		// Uint
		DOTween.To(()=> uintToTween, x=> uintToTween = x, 50, 1.5f).SetAs(tp).Pause();
		// Vector2
		DOTween.To(()=> vector2toTween, x=> vector2toTween = x, new Vector2(50,100), 1.5f).SetAs(tp).Pause();
		// Vector4
		DOTween.To(()=> vector4toTween, x=> vector4toTween = x, new Vector4(50,100,150,200), 1.5f).SetAs(tp).Pause();
		// Rect
		DOTween.To(()=> rectToTween, x=> rectToTween = x, new Rect(10, 20, 50, 100), 1.5f).SetAs(tp).Pause();
		// RectOffset
		DOTween.To(()=> rectOffsetToTween, x=> rectOffsetToTween = x, new RectOffset(10, 20, 50, 100), 1.5f).SetAs(tp).Pause();
		// Color
		DOTween.To(()=> guiTexColor.color, x=> guiTexColor.color = x, Color.green, 1.5f).SetAs(tp).Pause();
		// Alpha
		DOTween.ToAlpha(()=> guiTexAlpha.color, x=> guiTexAlpha.color = x, 0f, 1.5f).SetAs(tp).Pause();
		// String
		DOTween.To(()=> stringToTween0, x=> stringToTween0 = x, "Hello I'm a new string!", 1.5f).SetAs(tp).Pause();
		// String
		DOTween.To(()=> stringToTween1, x=> stringToTween1 = x, "Hello I'm a new string!", 1.5f).SetAs(tp).Pause();
		// String (relative)
		DOTween.To(()=> stringToTween2, x=> stringToTween2 = x, "Hello I'm a new string!", 1.5f).SetAs(tp).SetRelative().Pause();
		if (tweenLightAndCam) {
			// Camera
			Camera.main.DOColor(toCamColor, 1.5f).SetAs(tp).Pause();
			// Light
			mainLight.DOColor(toLightColor, 1.5f).SetAs(tp).Pause();
			mainLight.DOIntensity(4, 1.5f).SetAs(tp).Pause();
		}
		// SpriteRenderer
		spriteRenderer.DOColor(toSpriteColor, 1.5f).SetAs(tp).Pause();
		// Specular material tween
		specularSphere.GetComponent<Renderer>().material.DOColor(Color.green, "_SpecColor", 1.5f).SetAs(tp).Pause();
		// Rotate towards
		targetToDoLookAt.DOLookAt(specularSphere.transform.position, 1.5f).SetAs(tp).Pause();
		targetToDoLookFrom.DOLookAt(specularSphere.transform.position, 1.5f).From().SetAs(tp).Pause();
	}

	void LateUpdate()
	{
		txtFloat.text = "float: " + floatToTween;
		txtInt.text = "int: " + intToTween;
		txtUint.text = "uint: " + uintToTween;
		txtVector2.text = "Vector2: " + vector2toTween;
		txtVector4.text = "Vector4: " + vector4toTween;
		txtRect.text = "Rect: " + rectToTween;
		txtRectOffset.text = "RectOffset: " + rectOffsetToTween;
		txtString0.text = "String 0: " + stringToTween0;
		txtString1.text = "String 1: " + stringToTween1;
		txtString2.text = "String 2: " + stringToTween2;

		bool isBackwards = tweens[1] != null && tweens[1].IsBackwards();
		if (isBackwards && !txtBackwards.activeSelf || !isBackwards && txtBackwards.activeSelf) {
			txtBackwards.SetActive(!txtBackwards.activeSelf);
		}
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Play All")) Debug.Log("Played tweens: " + DOTween.PlayAll());
		if (GUILayout.Button("Pause All")) Debug.Log("Paused tweens: " + DOTween.PauseAll());
		if (GUILayout.Button("Kill All")) Debug.Log("Killed tweens: " + DOTween.KillAll());
		if (GUILayout.Button("Complete+Kill All")) Debug.Log("Killed tweens: " + DOTween.KillAll(true));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Rewind All")) Debug.Log("Rewinded tweens: " + DOTween.RewindAll());
		if (GUILayout.Button("Restart All")) Debug.Log("Restarted tweens: " + DOTween.RestartAll());
		if (GUILayout.Button("Complete All")) Debug.Log("Completed tweens: " + DOTween.CompleteAll());
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("PlayForward All")) Debug.Log("PlayForwarded tweens: " + DOTween.PlayForwardAll());
		if (GUILayout.Button("PlayBackwards All")) Debug.Log("PlayBackwarded tweens: " + DOTween.PlayBackwardsAll());
		if (GUILayout.Button("Flip All")) Debug.Log("Flipped tweens: " + DOTween.FlipAll());
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Goto 1.5 All")) Debug.Log("Goto-ed tweens: " + DOTween.GotoAll(1.5f));
		if (GUILayout.Button("Goto 3 All")) Debug.Log("Goto-ed tweens: " + DOTween.GotoAll(3));
		if (GUILayout.Button("Goto 4.5 All")) Debug.Log("Goto-ed tweens: " + DOTween.GotoAll(4.5f));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Play by Id")) Debug.Log("Played tweens: " + DOTween.Play(intId));
		if (GUILayout.Button("Play by StringId")) Debug.Log("Played tweens: " + DOTween.Play(stringId));
		if (GUILayout.Button("Play by UnityObjectId")) Debug.Log("Played tweens: " + DOTween.Play(this));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Pause by Id")) Debug.Log("Paused tweens: " + DOTween.Pause(intId));
		if (GUILayout.Button("Pause by StringId")) Debug.Log("Paused tweens: " + DOTween.Pause(stringId));
		if (GUILayout.Button("Pause by UnityObjectId")) Debug.Log("PlaPausedyed tweens: " + DOTween.Pause(this));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Kill by Id")) Debug.Log("Killed tweens: " + DOTween.Kill(intId));
		if (GUILayout.Button("Complete+Kill by Id")) Debug.Log("Killed tweens: " + DOTween.Kill(intId, true));
		if (GUILayout.Button("Kill by StringId")) Debug.Log("Killed tweens: " + DOTween.Kill(stringId));
		if (GUILayout.Button("Kill by UnityObjectId")) Debug.Log("Killed tweens: " + DOTween.Kill(this));
		if (GUILayout.Button("Clear")) {
			Debug.Log(":::::::::::: CLEAR");
			DOTween.Clear();
		}
		if (GUILayout.Button("Clear & Reload")) {
			Debug.Log(":::::::::::: CLEAR AND RELOAD");
			int level = Application.loadedLevel;
			DOTween.Clear();
			Application.LoadLevel(level);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		foreach (Tween t in tweens) {
			if (GUILayout.Button("Direct Kill")) t.Kill();
		}
		GUILayout.EndHorizontal();

		GUILayout.Space(5);

		GUILayout.Label("Global DOTween Timescale");
		GUILayout.BeginHorizontal();
		DOTween.timeScale = GUILayout.HorizontalSlider(DOTween.timeScale, 0.0f, 20.0f);
		if (GUILayout.Button("Reset", GUILayout.Width(80))) DOTween.timeScale = 1;
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		for (int i = 0; i < tweens.Length; ++i) GUILayout.Label("Single Timescale");
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		foreach (Tween t in tweens) {
			t.timeScale = GUILayout.HorizontalSlider(t.timeScale, 0.0f, 20.0f, GUILayout.Width(60));
			if (GUILayout.Button("Reset", GUILayout.Width(80))) t.timeScale = 1;
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		foreach (Tween t in tweens) {
			GUILayout.Label("Elapsed: " + t.Elapsed(false) +
				"\nFullElapsed: " + t.Elapsed() +
				"\nElapsed %: " + t.ElapsedPercentage(false) +
				"\nFullElapsed %: " + t.ElapsedPercentage() +
				"\nCompleted Loops: " + t.CompletedLoops()
			);
		}
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}
}