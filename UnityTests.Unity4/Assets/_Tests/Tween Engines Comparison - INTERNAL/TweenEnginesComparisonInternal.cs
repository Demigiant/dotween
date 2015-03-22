using DG.Tweening;
using Holoville.HOTween;
using System;
using System.Collections;
using UnityEngine;

public class TweenEnginesComparisonInternal : BrainBase
{
	enum State {
		Menu,
		NoEmitWITween,
		ConfirmGoKit,
		ConfirmITween,
		CreatingObjects,
		TweensSetup,
		Running
	}
	enum Engine {
		DOTween,
		HOTween,
		LeanTween,
		GoKit,
		iTween
	}
	enum TestType {
		Transforms,
		Floats
	}
	enum TestSetup {
		Emit,
		Loop,
		YoyoLoop
	}
	enum Easing {
		Linear,
		InOutQuad
	}

	public bool doTweenSafeMode;
	public GameObject prefab;
	public float someVal; // used for iTween tests

	State state = State.Menu;
	Engine engine = Engine.DOTween;
	TestType testType = TestType.Transforms;
	TestSetup testSetup = TestSetup.YoyoLoop;
	Easing easing = Easing.Linear;
	int numTweens;
	float duration;
	bool disableRenderers;
	bool positionTween = true;
	bool rotationTween, scaleTween;
	Transform container;
	GameObject[] testObjsGos; // Here to test LeanTween fairly
	Transform[] testObjsTrans;
	TestObjectData[] testObjsData;
	Vector3[] rndPositions;
	Vector3[] rndRotations;
	Vector3[] rndStartupPos;
	readonly Vector3 rndScale = new Vector3(6,6,6);
	float[] rndFloats;

	float totCreationTime;

	bool guiInitialized;
	GUIStyle labelStyle;
	const int vspace = 10;
	string[] engineList, testTypeList, testSetupList, easingList;
	string[] durationList = new[] {
		"0.25", "0.5", "1", "2", "4", "8", "16", "32"
	};
	string[] numTweensList = new[] {
		"1", "2", "50", "100", "500", "1000", "2000", "4000", "8000", "16000", "32000"
	};
	int durationSelId = 2, numTweensSelId = 4;

	override protected void Awake()
	{
		base.Awake();
		DOTween.Init(true, false);
		DOTween.useSafeMode = doTweenSafeMode;
		HOTween.Init(true, false, false);
		LeanTween.init(Convert.ToInt32(numTweensList[numTweensList.Length - 1]) + 1);
	}

	void Start()
	{
		engineList = Enum.GetNames(typeof(Engine));
		testTypeList = Enum.GetNames(typeof(TestType));
		testSetupList = Enum.GetNames(typeof(TestSetup));
		easingList = Enum.GetNames(typeof(Easing));
		container = new GameObject("Test Objects Container").transform;
	}

	IEnumerator StartRun()
	{
		state = State.CreatingObjects;
		yield return null;

		duration = Convert.ToSingle(durationList[durationSelId]);
		// Generate random values for tweens
		rndStartupPos = new Vector3[numTweens];
		rndPositions = new Vector3[numTweens];
		rndRotations = new Vector3[numTweens];
		rndFloats = new float[numTweens];
		for (int i = 0; i < numTweens; ++i) {
			rndStartupPos[i] = RandomVector3(50, 50, 20);
			rndPositions[i] = RandomVector3(50, 50, 20);
			rndRotations[i] = RandomVector3(180, 180, 180);
			rndFloats[i] = UnityEngine.Random.Range(-1000f, 1000f);
		}
		// Generate testObjs
		if (testType == TestType.Transforms) {
			testObjsGos = new GameObject[numTweens];
			testObjsTrans = new Transform[numTweens];
		} else {
			testObjsData = new TestObjectData[numTweens];
		}
		for (int i = 0; i < numTweens; ++i) {
			if (testType == TestType.Transforms) {
				GameObject go = (GameObject)Instantiate(prefab);
				go.SetActive(true);
				Transform t = go.transform;
				if (testSetup != TestSetup.Emit) t.position = rndStartupPos[i];
				t.parent = container;
				testObjsGos[i] = go;
				testObjsTrans[i] = t;
				if (disableRenderers || testType == TestType.Floats) go.renderer.enabled = false;
			} else testObjsData[i] = new TestObjectData();
		}
		if (engine == Engine.DOTween) {
			// Set max capacity for this run.
			// We could set it to the correct amount, but it would be somehow unfair for LeanTween
			int minCapacityToBeFair = Convert.ToInt32(numTweensList[numTweensList.Length - 1]);
			int capacityX1 = Convert.ToInt32(numTweensList[numTweensSelId]);
			int neededCapacity = 0;
			int capacity = minCapacityToBeFair;
			if (testType == TestType.Transforms && positionTween) neededCapacity += capacityX1;
			if (testType == TestType.Transforms && rotationTween) neededCapacity += capacityX1;
			if (testType == TestType.Transforms && scaleTween) neededCapacity += capacityX1;
			if (minCapacityToBeFair < neededCapacity) capacity = neededCapacity;
			DOTween.SetTweensCapacity(capacity, 0);
		}
		
		yield return null;
		state = State.TweensSetup;
		yield return null;
		totCreationTime = Time.realtimeSinceStartup;
		SetupTweens();
		totCreationTime = Time.realtimeSinceStartup - totCreationTime;
		yield return null;
		state = State.Running;
		// Reset FPS so average is more correct
		fpsGadget.ResetFps();
	}

	void StopRun()
	{
		this.StopAllCoroutines();
		state = State.Menu;
		// Clear tweens
		if (engine == Engine.DOTween) DOTween.Clear();
		else if (engine == Engine.HOTween) HOTween.Kill();
		else if (engine == Engine.LeanTween) LeanTween.reset();
		else if (engine == Engine.GoKit) KillAllGoTweens();
		else if (engine == Engine.iTween) iTween.Stop();
		// Clean
		if (testObjsGos != null) foreach (GameObject go in testObjsGos) Destroy(go);
		testObjsGos = null;
		testObjsTrans = null;
		testObjsData = null;
		rndPositions = null;
		rndRotations = null;
	}

	void Reset(bool complete = true)
	{
		if (complete) {
			if (engine == Engine.DOTween) DOTween.Clear();
			else if (engine == Engine.HOTween) HOTween.Kill();
			else if (engine == Engine.LeanTween) LeanTween.reset();
			else if (engine == Engine.GoKit) KillAllGoTweens();
			else if (engine == Engine.iTween) iTween.Stop();
		}
		if (testObjsTrans != null) {
			for (int i = 0; i < testObjsTrans.Length; ++i) {
				Transform t = testObjsTrans[i];
				t.position = rndStartupPos[i];
				t.localScale = Vector3.one;
				t.rotation = Quaternion.identity;
			}
		}
	}

	void SetupTweens()
	{
		// Ease
		DG.Tweening.Ease dotweenEase = easing == Easing.Linear ? DG.Tweening.Ease.Linear : DG.Tweening.Ease.InOutQuad;
		Holoville.HOTween.EaseType hotweenEase = easing == Easing.Linear ? Holoville.HOTween.EaseType.Linear : Holoville.HOTween.EaseType.EaseInOutQuad;
		LeanTweenType leanEase = easing == Easing.Linear ? LeanTweenType.linear : LeanTweenType.easeInOutQuad;
		GoEaseType goEase = easing == Easing.Linear ? GoEaseType.Linear : GoEaseType.QuadInOut;
		iTween.EaseType iTweenEase = easing == Easing.Linear ? iTween.EaseType.linear : iTween.EaseType.easeInOutQuad;
		// Loop
		int loops = testSetup == TestSetup.Emit ? 1 : -1;
		DG.Tweening.LoopType dotweenLoopType = testSetup == TestSetup.YoyoLoop ? DG.Tweening.LoopType.Yoyo : DG.Tweening.LoopType.Restart;
		Holoville.HOTween.LoopType hotweenLoopType = testSetup == TestSetup.YoyoLoop ? Holoville.HOTween.LoopType.Yoyo : Holoville.HOTween.LoopType.Restart;
		LeanTweenType leanLoopType = testSetup == TestSetup.YoyoLoop ? LeanTweenType.pingPong : LeanTweenType.clamp;
		GoLoopType goLoopType = testSetup == TestSetup.YoyoLoop ? GoLoopType.PingPong : GoLoopType.RestartFromBeginning;
		iTween.LoopType iTweenLoopType = loops != -1 ? iTween.LoopType.none : testSetup == TestSetup.YoyoLoop ? iTween.LoopType.pingPong : iTween.LoopType.loop;
		// Create tweens
		switch (testType) {
		case TestType.Floats:
			for (int i = 0; i < numTweens; ++i) {
				TestObjectData data = testObjsData[i];
				switch (engine) {
				case Engine.HOTween:
					HOTween.To(data, duration, new Holoville.HOTween.TweenParms()
						.Prop("floatValue", rndFloats[i])
						.Ease(hotweenEase)
						.Loops(loops, hotweenLoopType)
					);
					break;
				case Engine.LeanTween:
					LeanTween.value(this.gameObject, x=> data.floatValue = x, data.floatValue, rndFloats[i], duration).setEase(leanEase).setRepeat(loops).setLoopType(leanLoopType);
					break;
				case Engine.GoKit:
					Go.to(data, duration, new GoTweenConfig()
						.floatProp("floatValueProperty", rndFloats[i])
						.setEaseType(goEase)
						.setIterations(loops, goLoopType)
					);
					break;
				case Engine.iTween:
					Hashtable hs = new Hashtable();
					hs.Add("from", data.floatValue);
					hs.Add("to", rndFloats[i]);
					hs.Add("time", duration);
					hs.Add("onupdate", "UpdateiTweenFloat");
					hs.Add("looptype", iTweenLoopType);
					hs.Add("easetype", iTweenEase);
					iTween.ValueTo(this.gameObject, hs);
					break;
				default:
					// tCopy is needed to create correct closure object,
					// otherwise closure will pass the same t to all the loop
					TestObjectData dataCopy = data;
					DOTween.To(()=> dataCopy.floatValue, x=> dataCopy.floatValue = x, rndFloats[i], duration).SetEase(dotweenEase).SetLoops(loops, dotweenLoopType);
					break;
				}
			}
			break;
		default:
			for (int i = 0; i < numTweens; ++i) {
				float twDuration = testSetup == TestSetup.Emit ? UnityEngine.Random.Range(0.25f, 1f) : duration;
				Transform t = testObjsTrans[i];
				GameObject go = testObjsGos[i]; // Used by LeanTween and iTween
				switch (engine) {
				case Engine.HOTween:
					Holoville.HOTween.TweenParms tp = new Holoville.HOTween.TweenParms()
						.Ease(hotweenEase)
						.Loops(loops, hotweenLoopType);
					if (positionTween) {
						Vector3 toPos = rndPositions[i];
						tp.Prop("position", toPos);
						if (testSetup == TestSetup.Emit) tp.OnComplete(()=> EmitHOTweenPositionFor(t, toPos, twDuration, hotweenEase));
					}
					if (rotationTween) {
						Vector3 toRot = rndRotations[i];
						tp.Prop("rotation", toRot);
						if (testSetup == TestSetup.Emit) tp.OnComplete(()=> EmitHOTweenRotationFor(t, toRot, twDuration, hotweenEase));
					}
					if (scaleTween) {
						tp.Prop("localScale", rndScale);
						if (testSetup == TestSetup.Emit) tp.OnComplete(()=> EmitHOTweenScaleFor(t, rndScale, twDuration, hotweenEase));
					}
					HOTween.To(t, twDuration, tp);
					break;
				case Engine.LeanTween:
					LTDescr leanTween;
					if (positionTween) {
						Vector3 toPos = rndPositions[i];
						leanTween = LeanTween.move(go, rndPositions[i], twDuration).setEase(leanEase).setRepeat(loops).setLoopType(leanLoopType);
						if (testSetup == TestSetup.Emit) leanTween.setOnComplete(()=> EmitLeanTweenPositionFor(t, go, toPos, twDuration, leanEase));
					}
					if (rotationTween) {
						Vector3 toRot = rndRotations[i];
						leanTween = LeanTween.rotate(go, rndRotations[i], twDuration).setEase(leanEase).setRepeat(loops).setLoopType(leanLoopType);
						if (testSetup == TestSetup.Emit) leanTween.setOnComplete(()=> EmitLeanTweenRotationFor(t, go, toRot, twDuration, leanEase));
					}
					if (scaleTween) {
						leanTween = LeanTween.scale(go, rndScale, twDuration).setEase(leanEase).setRepeat(loops).setLoopType(leanLoopType);
						if (testSetup == TestSetup.Emit) leanTween.setOnComplete(()=> EmitLeanTweenScaleFor(t, go, rndScale, twDuration, leanEase));
					}
					break;
				case Engine.GoKit:
					GoTweenConfig goConfig = new GoTweenConfig()
						.setEaseType(goEase)
						.setIterations(loops, goLoopType);
					if (positionTween) {
						Vector3 toPos = rndPositions[i];
						goConfig.addTweenProperty(new PositionTweenProperty(toPos));
						if (testSetup == TestSetup.Emit) goConfig.onComplete(x=> EmitGoKitPositionFor(t, toPos, twDuration, goEase));
					}
					if (rotationTween) {
						Vector3 toRot = rndRotations[i];
						goConfig.addTweenProperty(new RotationTweenProperty(toRot));
						if (testSetup == TestSetup.Emit) goConfig.onComplete(x=> EmitGoKitRotationFor(t, toRot, twDuration, goEase));
					}
					if (scaleTween) {
						goConfig.addTweenProperty(new ScaleTweenProperty(rndScale));
						if (testSetup == TestSetup.Emit) goConfig.onComplete(x=> EmitGoKitScaleFor(t, rndScale, twDuration, goEase));
					}
					Go.to(t, twDuration, goConfig);
					break;
				case Engine.iTween:
					Hashtable hs;
					if (positionTween) {
						hs = new Hashtable();
						hs.Add("position", rndPositions[i]);
						hs.Add("time", twDuration);
						hs.Add("looptype", iTweenLoopType);
						hs.Add("easetype", iTweenEase);
						iTween.MoveTo(go, hs);
					}
					if (rotationTween) {
						hs = new Hashtable();
						hs.Add("rotation", rndRotations[i]);
						hs.Add("time", twDuration);
						hs.Add("looptype", iTweenLoopType);
						hs.Add("easetype", iTweenEase);
						iTween.RotateTo(go, hs);
					}
					if (scaleTween) {
						hs = new Hashtable();
						hs.Add("scale", rndScale);
						hs.Add("time", twDuration);
						hs.Add("looptype", iTweenLoopType);
						hs.Add("easetype", iTweenEase);
						iTween.ScaleTo(go, hs);
					}
					break;
				default:
					// tCopy is needed to create correct closure object,
					// otherwise closure will pass the same t to all the loop
					Transform tCopy = t;
					DG.Tweening.Tween dotween;
					if (positionTween) {
						Vector3 toPos = rndPositions[i];
						dotween = tCopy.DOMove(toPos, twDuration).SetEase(dotweenEase).SetLoops(loops, dotweenLoopType);
						if (testSetup == TestSetup.Emit) dotween.OnComplete(()=> EmitDOTweenPositionFor(t, toPos, twDuration, dotweenEase));
					}
					if (rotationTween) {
						Vector3 toRot = rndRotations[i];
						dotween = tCopy.DORotate(toRot, twDuration).SetEase(dotweenEase).SetLoops(loops, dotweenLoopType);
						if (testSetup == TestSetup.Emit) dotween.OnComplete(()=> EmitDOTweenRotationFor(t, toRot, twDuration, dotweenEase));
					}
					if (scaleTween) {
						dotween = tCopy.DOScale(rndScale, twDuration).SetEase(dotweenEase).SetLoops(loops, dotweenLoopType);
						if (testSetup == TestSetup.Emit) dotween.OnComplete(()=> EmitDOTweenScaleFor(t, rndScale, twDuration, dotweenEase));
					}
					break;
				}
			}
			break;
		}
	}

	void EmitDOTweenPositionFor(Transform t, Vector3 to, float twDuration, DG.Tweening.Ease ease)
	{
		t.position = Vector3.zero;
		t.DOMove(to, twDuration).SetEase(ease).OnComplete(()=> EmitDOTweenPositionFor(t, to, twDuration, ease));
	}
	void EmitDOTweenRotationFor(Transform t, Vector3 to, float twDuration, DG.Tweening.Ease ease)
	{
		t.rotation = Quaternion.identity;
		t.DORotate(to, twDuration).SetEase(ease).OnComplete(()=> EmitDOTweenRotationFor(t, to, twDuration, ease));
	}
	void EmitDOTweenScaleFor(Transform t, Vector3 to, float twDuration, DG.Tweening.Ease ease)
	{
		t.localScale = Vector3.one;
		t.DOScale(to, twDuration).SetEase(ease).OnComplete(()=> EmitDOTweenScaleFor(t, to, twDuration, ease));
	}

	void EmitHOTweenPositionFor(Transform t, Vector3 to, float twDuration, Holoville.HOTween.EaseType ease)
	{
		t.position = Vector3.zero;
		HOTween.To(t, twDuration, new Holoville.HOTween.TweenParms()
			.Prop("position", to)
			.Ease(ease)
			.OnComplete(()=> EmitHOTweenPositionFor(t, to, twDuration, ease))
		);
	}
	void EmitHOTweenRotationFor(Transform t, Vector3 to, float twDuration, Holoville.HOTween.EaseType ease)
	{
		t.rotation = Quaternion.identity;
		HOTween.To(t, twDuration, new Holoville.HOTween.TweenParms()
			.Prop("rotation", to)
			.Ease(ease)
			.OnComplete(()=> EmitHOTweenRotationFor(t, to, twDuration, ease))
		);
	}
	void EmitHOTweenScaleFor(Transform t, Vector3 to, float twDuration, Holoville.HOTween.EaseType ease)
	{
		t.localScale = Vector3.one;
		HOTween.To(t, twDuration, new Holoville.HOTween.TweenParms()
			.Prop("localScale", to)
			.Ease(ease)
			.OnComplete(()=> EmitHOTweenScaleFor(t, to, twDuration, ease))
		);
	}

	void EmitLeanTweenPositionFor(Transform t, GameObject go, Vector3 to, float twDuration, LeanTweenType ease)
	{
		t.position = Vector3.zero;
		LeanTween.move(go, to, twDuration).setEase(ease).setOnComplete(()=> EmitLeanTweenPositionFor(t, go, to, twDuration, ease));
	}
	void EmitLeanTweenRotationFor(Transform t, GameObject go, Vector3 to, float twDuration, LeanTweenType ease)
	{
		t.rotation = Quaternion.identity;
		LeanTween.rotate(go, to, twDuration).setEase(ease).setOnComplete(()=> EmitLeanTweenRotationFor(t, go, to, twDuration, ease));
	}
	void EmitLeanTweenScaleFor(Transform t, GameObject go, Vector3 to, float twDuration, LeanTweenType ease)
	{
		t.localScale = Vector3.one;
		LeanTween.scale(go, to, twDuration).setEase(ease).setOnComplete(()=> EmitLeanTweenScaleFor(t, go, to, twDuration, ease));
	}

	void EmitGoKitPositionFor(Transform t, Vector3 to, float twDuration, GoEaseType ease)
	{
		t.position = Vector3.zero;
		Go.to(t, twDuration, new GoTweenConfig()
			.addTweenProperty(new PositionTweenProperty(to))
			.onComplete(x=> EmitGoKitPositionFor(t, to, twDuration, ease))
		);
	}
	void EmitGoKitRotationFor(Transform t, Vector3 to, float twDuration, GoEaseType ease)
	{
		t.rotation = Quaternion.identity;
		Go.to(t, twDuration, new GoTweenConfig()
			.addTweenProperty(new RotationTweenProperty(to))
			.onComplete(x=> EmitGoKitRotationFor(t, to, twDuration, ease))
		);
	}
	void EmitGoKitScaleFor(Transform t, Vector3 to, float twDuration, GoEaseType ease)
	{
		t.localScale = Vector3.one;
		Go.to(t, twDuration, new GoTweenConfig()
			.addTweenProperty(new ScaleTweenProperty(to))
			.onComplete(x=> EmitGoKitScaleFor(t, to, twDuration, ease))
		);
	}

	Vector3 RandomVector3(float rangeX, float rangeY, float rangeZ)
	{
		return new Vector3(UnityEngine.Random.Range(-rangeX, rangeX), UnityEngine.Random.Range(-rangeY, rangeY), UnityEngine.Random.Range(-rangeZ, rangeZ));
	}

	// GoKit has no "KillAll" method, so we'll have to kill the tweens one by one based on target
	void KillAllGoTweens()
	{
		if(testObjsTrans != null) foreach(Transform t in testObjsTrans) Go.killAllTweensWithTarget(t);
		if(testObjsData != null) foreach(TestObjectData t in testObjsData) Go.killAllTweensWithTarget(t);
	}

	public void UpdateiTweenFloat(float newVal)
	{
		// Practically does nothing: iTween can't logically tween many floats
		// Still a valid test though, and even grants iTween some slack since it will do less than other engines
		someVal = newVal;
	}

	void OnGUI()
	{
		if (!guiInitialized) {
			guiInitialized = true;
			labelStyle = new GUIStyle(GUI.skin.label);
			labelStyle.padding = new RectOffset(0, 0, 0, 0);
			labelStyle.margin = new RectOffset(4, 4, 0, 0);
		}

		GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();

		switch (state) {
		case State.CreatingObjects:
			DrawCreatingObjectsGUI();
			break;
		case State.TweensSetup:
			DrawTweensSetupGUI();
			break;
		case State.Running:
			DrawRunningGUI();
			break;
		case State.NoEmitWITween:
			DrawNoEmitWITweenGUI();
			break;
		case State.ConfirmGoKit:
			DrawConfirmGoKitGUI();
			break;
		case State.ConfirmITween:
			DrawConfirmITweenGUI();
			break;
		default:
			DrawMenuGUI();
			break;
		}

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	void DrawMenuGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		GUILayout.Label("Tween Duration", labelStyle);
		durationSelId = GUILayout.Toolbar(durationSelId, durationList);
		GUILayout.Space(vspace);
		GUILayout.Label("Number of Tweens", labelStyle);
		numTweensSelId = GUILayout.Toolbar(numTweensSelId, numTweensList);
		GUILayout.Space(vspace);
		GUILayout.Label("Tweens", labelStyle);
		GUILayout.BeginHorizontal();
			positionTween = GUILayout.Toggle(positionTween, "Position");
			rotationTween = GUILayout.Toggle(rotationTween, "Rotation");
			scaleTween = GUILayout.Toggle(scaleTween, "Scale");
			if (!positionTween && !rotationTween && !scaleTween) positionTween = true;
		GUILayout.EndHorizontal();
		GUILayout.Space(vspace);
		GUILayout.Label("Test Type", labelStyle);
		testType = (TestType)GUILayout.Toolbar((int)testType, testTypeList);
		if (testType == TestType.Floats && testSetup == TestSetup.Emit) testSetup = TestSetup.Loop;
		GUILayout.Space(vspace);
		GUILayout.Label("Test Setup", labelStyle);
		testSetup = (TestSetup)GUILayout.Toolbar((int)testSetup, testSetupList);
		GUILayout.Space(vspace);
		GUILayout.Label("Easing", labelStyle);
		easing = (Easing)GUILayout.Toolbar((int)easing, easingList);
		GUILayout.Space(vspace);
		GUILayout.Label("Options", labelStyle);
		disableRenderers = GUILayout.Toggle(disableRenderers, "Disable Renderers");
		GUILayout.Space(vspace);
		GUILayout.Label("Engine", labelStyle);
		engine = (Engine)GUILayout.Toolbar((int)engine, engineList);
		GUILayout.Space(vspace);
		if (GUILayout.Button("START")) {
			numTweens = Convert.ToInt32(numTweensList[numTweensSelId]);
			if (engine == Engine.GoKit && testType == TestType.Floats && numTweens >= 8000) state = State.ConfirmGoKit;
			else if (engine == Engine.iTween) {
				if (testSetup == TestSetup.Emit) state = State.NoEmitWITween;
				else if (numTweens > 4000) state = State.ConfirmITween;
				else StartCoroutine(StartRun());
			}
			else StartCoroutine(StartRun());
		}

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	void DrawNoEmitWITweenGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		GUILayout.Label("Sorry, but iTween doesn't allow to create a good Emit test");
		GUILayout.Space(8);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Ok")) state = State.Menu;
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	void DrawConfirmGoKitGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		GUILayout.Label("Beware, GoKit takes a long time to startup and to stop custom tweens,\nand your computer might hang for a while.\n\nAre you sure you want to proceed?");
		GUILayout.Space(8);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Continue")) StartCoroutine(StartRun());
		if (GUILayout.Button("Cancel")) state = State.Menu;
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	void DrawConfirmITweenGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		GUILayout.Label("Beware, ITween takes a long time to startup and a longer time to stop tweens,\nand your computer might hang for a while.\n\nAre you sure you want to proceed?");
		GUILayout.Space(8);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Continue")) StartCoroutine(StartRun());
		if (GUILayout.Button("Cancel")) state = State.Menu;
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	void DrawCreatingObjectsGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		GUILayout.BeginHorizontal(GUI.skin.box);
		GUILayout.Space(8);
		GUILayout.Label("Preparing environment...");
		GUILayout.Space(8);
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	void DrawTweensSetupGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		GUILayout.BeginHorizontal(GUI.skin.box);
		GUILayout.Space(8);
		GUILayout.Label("Starting up tweens...");
		GUILayout.Space(8);
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	void DrawRunningGUI()
	{
		GUILayout.BeginVertical();
		if (testType == TestType.Floats) {
			GUILayout.FlexibleSpace();
			GUILayout.Label("Tweening random float values on each test object");
		} else if (disableRenderers) {
			GUILayout.FlexibleSpace();
			GUILayout.Label("Tweening transforms even if you can't see them (renderers disabled)");
		}
		GUILayout.FlexibleSpace();

		GUILayout.Label(engine.ToString() + " (startup time: " + totCreationTime + ")");
		if (GUILayout.Button("STOP")) StopRun();

		GUILayout.EndVertical();
	}
}