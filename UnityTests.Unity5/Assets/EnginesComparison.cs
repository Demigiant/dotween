using DG.Tweening;
using Holoville.DebugFramework.Components;
using Holoville.HOTween;
using System;
using System.Collections;
using UnityEngine;

public class EnginesComparison : MonoBehaviour
{
	public GameObject prefab;

	enum State {
		Menu,
		Starting,
		Executing
	}
	enum TestType {
		Transforms,
		GenericFloats
	}
	enum EngineType {
		DOTween, HOTween, LeanTween, GoKit, iTween
	}
	string[] tweensList = new[] {
		"1", "10", "100", "500", "1,000", "2,000", "4,000", "8,000", "16,000", "32,000", "64,000", "128,000"
	};

	TestType testType;
	EngineType engineType;
	public static int totTweens;
	bool disableRenderers;

	State state = State.Menu;
	HOFpsGadget fpsGadget;
	float startupTime;
	Transform container;
	Action concludeTest;
	public static Transform[] ts;
	public static GameObject[] gos;
	[System.NonSerialized] public float floatVal; // Used by iTween to at least do something during its update

	string testTitle;
	string[] testTypeList, engineTypeList;
	int tweensListId = 4;


	void Start()
	{
		GameObject fpsGadgetGo = new GameObject("FPS");
		DontDestroyOnLoad(fpsGadgetGo);
		fpsGadget = fpsGadgetGo.AddComponent<HOFpsGadget>();
		fpsGadget.showMemory = true;

		testTypeList = Enum.GetNames(typeof(TestType));
		engineTypeList = Enum.GetNames(typeof(EngineType));
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		switch (state) {
		case State.Menu:
			testType = (TestType)GUILayout.Toolbar((int)testType, testTypeList);
			engineType = (EngineType)GUILayout.Toolbar((int)engineType, engineTypeList);
			tweensListId = GUILayout.Toolbar(tweensListId, tweensList);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("START")) StartCoroutine(StartTest());
			if (testType == TestType.Transforms) {
				if (GUILayout.Button("START (renderers disabled)")) {
					disableRenderers = true;
					StartCoroutine(StartTest());
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			break;
		case State.Starting:
			GUILayout.Label("Starting the test...");
			GUILayout.FlexibleSpace();
			break;
		case State.Executing:
			GUILayout.Label(testTitle);
			if (GUILayout.Button("STOP")) StopTest();
			break;
		}

		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	IEnumerator StartTest()
	{
		state = State.Starting;
		totTweens = int.Parse(tweensList[tweensListId], System.Globalization.NumberStyles.AllowThousands);
		testTitle = engineType.ToString();
		SampleClass[] cs = null;
		Vector3[] toPositions = null;
		float[] toFloats = null;
		// Prepare test
		switch (testType) {
		case TestType.Transforms:
			ts = new Transform[totTweens];
			gos = new GameObject[totTweens];
			toPositions = new Vector3[totTweens];
			container = new GameObject("Container").transform;
			for (int i = 0; i < totTweens; ++i) {
				GameObject go = (GameObject)Instantiate(prefab);
				if (disableRenderers) go.GetComponent<Renderer>().enabled = false;
				Transform t = go.transform;
				t.parent = container;
				t.position = new Vector3(UnityEngine.Random.Range(-40f, 40f), UnityEngine.Random.Range(-40f, 40f), UnityEngine.Random.Range(-40f, 40f));
				gos[i] = go;
				ts[i] = t;
				toPositions[i] = new Vector3(UnityEngine.Random.Range(-40f, 40f), UnityEngine.Random.Range(-40f, 40f), UnityEngine.Random.Range(-40f, 40f));
			}
			break;
		case TestType.GenericFloats:
			cs = new SampleClass[totTweens];
			toFloats = new float[totTweens];
			for (int i = 0; i < totTweens; ++i) {
				SampleClass c = new SampleClass(UnityEngine.Random.Range(-100f, 100f));
				cs[i] = c;
				toFloats[i] = UnityEngine.Random.Range(-100f, 100f);
			}
			break;
		}
		yield return null;

		// Prepare and start engine
		float time;
		switch (engineType) {
		case EngineType.DOTween:
			testTitle += " v" + DOTween.Version;
			concludeTest = DOTweenTester.Conclude;
			DOTween.Init(true, false);
			DOTween.SetTweensCapacity(totTweens, 0);
			yield return null;
			// Start
			time = Time.realtimeSinceStartup;
			if (testType == TestType.Transforms) DOTweenTester.Start(ts, toPositions);
			else DOTweenTester.Start(cs, toFloats);
			startupTime = Time.realtimeSinceStartup - time;
			break;
		case EngineType.HOTween:
			testTitle += " v" + HOTween.VERSION;
			concludeTest = HOTweenTester.Conclude;
			HOTween.Init(true, false, false);
			yield return null;
			// Start
			time = Time.realtimeSinceStartup;
			if (testType == TestType.Transforms) HOTweenTester.Start(ts, toPositions);
			else HOTweenTester.Start(cs, toFloats);
			startupTime = Time.realtimeSinceStartup - time;
			break;
		case EngineType.LeanTween:
			concludeTest = LeanTweenTester.Conclude;
			LeanTween.init(totTweens + 1);
			yield return null;
			// Start
			time = Time.realtimeSinceStartup;
			if (testType == TestType.Transforms) LeanTweenTester.Start(gos, toPositions);
			else LeanTweenTester.Start(this.gameObject, cs, toFloats);
			startupTime = Time.realtimeSinceStartup - time;
			break;
		case EngineType.GoKit:
			concludeTest = GoKitTester.Conclude;
			yield return null;
			// Start
			time = Time.realtimeSinceStartup;
			if (testType == TestType.Transforms) GoKitTester.Start(ts, toPositions);
			else GoKitTester.Start(cs, toFloats);
			startupTime = Time.realtimeSinceStartup - time;
			break;
		case EngineType.iTween:
			concludeTest = iTweenTester.Conclude;
			yield return null;
			// Start
			time = Time.realtimeSinceStartup;
			if (testType == TestType.Transforms) iTweenTester.Start(gos, toPositions);
			else iTweenTester.Start(this.gameObject, cs, toFloats);
			startupTime = Time.realtimeSinceStartup - time;
			break;
		}
		testTitle += " (startup time: " + startupTime + ")";
		yield return null;

		state = State.Executing;
		fpsGadget.ResetFps();
	}

	void StopTest()
	{
		state = State.Menu;
		this.StopAllCoroutines();
		concludeTest();
		if (container != null) {
			Destroy(container.gameObject);
			container = null;
		}
		ts = null;
		gos = null;
		disableRenderers = false;
		GC.Collect();
		fpsGadget.ResetFps();
	}

	public void UpdateiTweenFloat(float newVal)
	{
		// Practically does nothing: iTween can't logically tween many floats
		// Still a valid test though, and even grants iTween some slack since it will do a LOT less than other engines
		floatVal = newVal;
	}
}

public static class DOTweenTester
{
	public static void Start(Transform[] ts, Vector3[] to)
	{
		for (int i = 0; i < ts.Length; ++i) {
			ts[i].DOMove(to[i], 1).SetEase(Ease.InOutQuad).SetLoops(-1, DG.Tweening.LoopType.Yoyo);
		}
	}
	public static void Start(SampleClass[] cs, float[] to)
	{
		for (int i = 0; i < cs.Length; ++i) {
			SampleClass c = cs[i];
			DOTween.To(()=> c.floatVal, x=> c.floatVal = x, to[i], 1).SetEase(Ease.InOutQuad).SetLoops(-1, DG.Tweening.LoopType.Yoyo);
		}
	}
	public static void Conclude()
	{
		DOTween.Clear(true);
	}
}

public static class HOTweenTester
{
	public static void Start(Transform[] ts, Vector3[] to)
	{
		Holoville.HOTween.TweenParms tp = new Holoville.HOTween.TweenParms().Ease(EaseType.EaseInOutQuad).Loops(-1, Holoville.HOTween.LoopType.Yoyo);
		for (int i = 0; i < ts.Length; ++i) {
			HOTween.To(ts[i], 1, tp.NewProp("position", to[i]));
		}
	}
	public static void Start(SampleClass[] cs, float[] to)
	{
		Holoville.HOTween.TweenParms tp = new Holoville.HOTween.TweenParms().Ease(EaseType.EaseInOutQuad).Loops(-1, Holoville.HOTween.LoopType.Yoyo);
		for (int i = 0; i < cs.Length; ++i) {
			HOTween.To(cs[i], 1, tp.NewProp("floatVal", to[i]));
		}
	}
	public static void Conclude()
	{
		HOTween.Kill();
		UnityEngine.Object.Destroy(GameObject.Find("HOTween"));
	}
}

public static class LeanTweenTester
{
	public static void Start(GameObject[] gos, Vector3[] to)
	{
		for (int i = 0; i < gos.Length; ++i) {
			LeanTween.move(gos[i], to[i], 1).setEase(LeanTweenType.easeInOutQuad).setRepeat(-1).setLoopType(LeanTweenType.pingPong);
		}
	}
	public static void Start(GameObject target, SampleClass[] cs, float[] to)
	{
		for (int i = 0; i < cs.Length; ++i) {
			SampleClass c = cs[i];
			LeanTween.value(target, x=> c.floatVal = x, c.floatVal, to[i], 1).setEase(LeanTweenType.easeInOutQuad).setRepeat(-1).setLoopType(LeanTweenType.pingPong);
		}
	}
	public static void Conclude()
	{
		LeanTween.reset();
		UnityEngine.Object.Destroy(GameObject.Find("~LeanTween"));
	}
}

public static class GoKitTester
{
	public static void Start(Transform[] ts, Vector3[] to)
	{
		GoTweenConfig goConfig = new GoTweenConfig().setEaseType(GoEaseType.QuadInOut).setIterations(-1, GoLoopType.PingPong);
		for (int i = 0; i < ts.Length; ++i) {
			goConfig.clearProperties();
			goConfig.addTweenProperty(new PositionTweenProperty(to[i]));
			Go.to(ts[i], 1, goConfig);
		}
	}
	public static void Start(SampleClass[] cs, float[] to)
	{
		GoTweenConfig goConfig = new GoTweenConfig().setEaseType(GoEaseType.QuadInOut).setIterations(-1, GoLoopType.PingPong);
		for (int i = 0; i < cs.Length; ++i) {
			goConfig.clearProperties();
			goConfig.floatProp("floatVal", to[i]);
			Go.to(cs[i], 1, goConfig);
		}
	}
	public static void Conclude()
	{
		if(EnginesComparison.ts != null) for(int i = 0; i < EnginesComparison.ts.Length; ++i) Go.killAllTweensWithTarget(EnginesComparison.ts[i]);
		UnityEngine.Object.Destroy(GameObject.Find("GoKit (" + EnginesComparison.totTweens + " tweens)"));
	}
}

public static class iTweenTester
{
	public static void Start(GameObject[] gos, Vector3[] to)
	{
		for (int i = 0; i < gos.Length; ++i) {
			Hashtable hs = new Hashtable();
			hs.Add("position", to[i]);
			hs.Add("time", 1);
			hs.Add("looptype", iTween.LoopType.pingPong);
			hs.Add("easetype", iTween.EaseType.easeInOutQuad);
			iTween.MoveTo(gos[i], hs);
		}
	}
	public static void Start(GameObject target, SampleClass[] cs, float[] to)
	{
		for (int i = 0; i < cs.Length; ++i) {
			SampleClass c = cs[i];
			Hashtable hs = new Hashtable();
			hs.Add("from", c.floatVal);
			hs.Add("to", to[i]);
			hs.Add("time", 1);
			hs.Add("onupdate", "UpdateiTweenFloat");
			hs.Add("looptype", iTween.LoopType.pingPong);
			hs.Add("easetype", iTween.EaseType.easeInOutQuad);
			iTween.ValueTo(target, hs);
		}
	}
	public static void Conclude()
	{
		iTween.Stop();
	}
}