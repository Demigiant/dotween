using DG.Tweening;
using UnityEngine;

public class Pooling : BrainBase
{
	public GameObject prefab;

	bool recycle;
	Transform spawnsParent;

	void Start()
	{
		DOTween.Init(true, false, LogBehaviour.Verbose);
		spawnsParent = new GameObject("Spawn Container").transform;
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();

		GUILayout.Label("Position Tweens");
		GUILayout.BeginHorizontal();
	    if (GUILayout.Button("Spawn 1")) SpawnPosition(1);
	    if (GUILayout.Button("Spawn 10")) SpawnPosition(10);
	    if (GUILayout.Button("Spawn 100")) SpawnPosition(100);
	    if (GUILayout.Button("Spawn 300")) SpawnPosition(300);
	    GUILayout.EndHorizontal();
	    GUILayout.Space(5);

	    GUILayout.Label("PositionX Tweens");
		GUILayout.BeginHorizontal();
	    if (GUILayout.Button("Spawn 1")) SpawnPositionX(1);
	    if (GUILayout.Button("Spawn 10")) SpawnPositionX(10);
	    if (GUILayout.Button("Spawn 100")) SpawnPositionX(100);
	    if (GUILayout.Button("Spawn 300")) SpawnPositionX(300);
	    GUILayout.EndHorizontal();
	    GUILayout.Space(5);

	    GUILayout.Label("PositionX Tweens with snapping");
		GUILayout.BeginHorizontal();
	    if (GUILayout.Button("Spawn 1")) SpawnPositionX(1, true);
	    if (GUILayout.Button("Spawn 10")) SpawnPositionX(10, true);
	    if (GUILayout.Button("Spawn 100")) SpawnPositionX(100, true);
	    if (GUILayout.Button("Spawn 300")) SpawnPositionX(300, true);
	    GUILayout.EndHorizontal();
	    GUILayout.Space(5);

	    GUILayout.Label("PositionY Tweens");
		GUILayout.BeginHorizontal();
	    if (GUILayout.Button("Spawn 1")) SpawnPositionY(1);
	    if (GUILayout.Button("Spawn 10")) SpawnPositionY(10);
	    if (GUILayout.Button("Spawn 100")) SpawnPositionY(100);
	    if (GUILayout.Button("Spawn 300")) SpawnPositionY(300);
	    GUILayout.EndHorizontal();
	    GUILayout.Space(5);

	    GUILayout.Label("Rotation Tweens");
		GUILayout.BeginHorizontal();
	    if (GUILayout.Button("Spawn 1")) SpawnRotation(1);
	    if (GUILayout.Button("Spawn 10")) SpawnRotation(10);
	    if (GUILayout.Button("Spawn 100")) SpawnRotation(100);
	    if (GUILayout.Button("Spawn 300")) SpawnRotation(300);
	    GUILayout.EndHorizontal();
	    GUILayout.Space(5);

	    GUILayout.Label("Emit Particles Tweens");
		GUILayout.BeginHorizontal();
	    if (GUILayout.Button("Spawn 1")) SpawnParticles(1);
	    if (GUILayout.Button("Spawn 10")) SpawnParticles(10);
	    if (GUILayout.Button("Spawn 100")) SpawnParticles(100);
	    if (GUILayout.Button("Spawn 300")) SpawnParticles(300);
	    if (GUILayout.Button("Spawn 1000")) SpawnParticles(1000);
	    GUILayout.EndHorizontal();
	    GUILayout.Space(5);

	    GUILayout.BeginHorizontal();
	    if (GUILayout.Button("Toggle Pause")) DOTween.TogglePauseAll();
	    if (GUILayout.Button("Kill")) DOTween.KillAll();
	    if (GUILayout.Button("Clear")) Clear();
	    if (GUILayout.Button("Clear FULL")) Clear(true);
	    GUILayout.EndHorizontal();
	    recycle = GUILayout.Toggle(recycle, "Recycle");

	    GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	void SpawnPosition(int tot)
	{
		for (int i = 0; i < tot; i++) {
        	GameObject go = Instantiate(prefab) as GameObject;
        	go.name += i;
            Transform t = go.transform;
            t.parent = spawnsParent;
            t.position = RandomVector3();
            DOTween.To(() => t.position, x => t.position = x, RandomVector3(), 1f)
            	.SetRecyclable(recycle)
            	.OnComplete(() => Destroy(go));
        }
        fpsGadget.ResetFps();
	}
	void SpawnPositionX(int tot, bool snapping = false)
	{
		for (int i = 0; i < tot; i++) {
        	GameObject go = Instantiate(prefab) as GameObject;
        	go.name += i;
            Transform t = go.transform;
            t.parent = spawnsParent;
            t.position = RandomVector3();
            DOTween.ToAxis(() => t.position, x => t.position = x, RandomFloat(), 1f)
            	.SetOptions(snapping)
            	.SetRecyclable(recycle)
            	.OnComplete(() => Destroy(go));
        }
        fpsGadget.ResetFps();
	}
	void SpawnPositionY(int tot, bool snapping = false)
	{
		for (int i = 0; i < tot; i++) {
        	GameObject go = Instantiate(prefab) as GameObject;
        	go.name += i;
            Transform t = go.transform;
            t.parent = spawnsParent;
            t.position = RandomVector3();
            if (snapping) {
            	DOTween.ToAxis(() => t.position, x => t.position = x, RandomFloat(), 1f)
            		.SetOptions(AxisConstraint.Y, true)
            		.SetRecyclable(recycle)
            		.OnComplete(() => Destroy(go));
        	} else {
        		DOTween.ToAxis(() => t.position, x => t.position = x, RandomFloat(), 1f)
        			.SetOptions(AxisConstraint.Y)
        			.SetRecyclable(recycle)
        			.OnComplete(() => Destroy(go));
        	}
        }
        fpsGadget.ResetFps();
	}
	void SpawnRotation(int tot)
	{
		for (int i = 0; i < tot; i++) {
        	GameObject go = Instantiate(prefab) as GameObject;
        	go.name += i;
            Transform t = go.transform;
            t.parent = spawnsParent;
            t.position = RandomVector3();
            DOTween.To(() => t.rotation, x => t.rotation = x, RandomVector3(720), 1f)
            	.SetRecyclable(recycle)
            	.OnComplete(() => Destroy(go));
        }
        fpsGadget.ResetFps();
	}

	void SpawnParticles(int tot)
	{
		for (int i = 0; i < tot; i++) {
			GameObject go = Instantiate(prefab) as GameObject;
	    	go.name += i;
	        Transform t = go.transform;
	        t.parent = spawnsParent;
	        Material m = t.gameObject.GetComponent<Renderer>().material;
			ResetParticleAndAssignTween(t, m);
		}
		fpsGadget.ResetFps();
	}
	void ResetParticleAndAssignTween(Transform t, Material m)
	{
		t.position = Vector3.zero;
		Color col = m.color;
		col.a = 1;
		m.color = col;
		float duration = Random.Range(0.2f, 2f);
        m.DOFade(0, duration).SetRecyclable(recycle);
        t.DOMove(RandomVector3(), duration)
        	.SetRecyclable(recycle)
        	.OnComplete(()=> ResetParticleAndAssignTween(t, m));
	}

	void Clear(bool fullDOTweenClear = false)
	{
		DOTween.Clear(fullDOTweenClear);
		Transform[] ts = spawnsParent.GetComponentsInChildren<Transform>();
		foreach (Transform t in ts) {
			if (t != spawnsParent) Destroy(t.gameObject);
		}
	}

	Vector3 RandomVector3(float limit = 10)
	{
		return new Vector3(Random.Range(-limit, limit), Random.Range(-limit, limit), Random.Range(-limit, limit));
	}

	float RandomFloat(float limit = 10)
	{
		return Random.Range(-limit, limit);
	}
}