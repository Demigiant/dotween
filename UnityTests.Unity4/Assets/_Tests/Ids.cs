using DG.Tweening;
using UnityEngine;

public class Ids : BrainBase
{
	public GameObject prefab;

	Transform[] ts;

	void Start()
	{
		ts = new Transform[3];
		float startX = -3;
		for (int i = 0; i < 3; ++i) {
			Transform t = ((GameObject)Instantiate(prefab)).transform;
			ts[i] = t;
			t.position = new Vector3(startX + 3 * i, 0, 0);
			Tween tween = t.DOMoveY(4, 1).SetLoops(-1, LoopType.Yoyo);
			switch (i) {
			case 0:
				tween.SetId(0);
				break;
			case 1:
				tween.SetId("string");
				break;
			case 2:
				tween.SetId(this);
				break;
			}
		}
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();
		GUILayout.Space(50);

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("TogglePause by Id")) DOTween.TogglePause(0);
		if (GUILayout.Button("TogglePause by StringId")) DOTween.TogglePause("string");
		if (GUILayout.Button("TogglePause by ObjId")) DOTween.TogglePause(this);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("TogglePause by Transform (0)")) DOTween.TogglePause(ts[0]);
		if (GUILayout.Button("TogglePause by Transform (1)")) DOTween.TogglePause(ts[1]);
		// This won't work because target was changed by applying a different objId
		if (GUILayout.Button("TogglePause by Transform (2)")) DOTween.TogglePause(ts[2]);
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}
}