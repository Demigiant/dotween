using DG.Tweening;
using System;
using UnityEngine;

public class Eases : BrainBase
{
	public GameObject prefab;
	public AnimationCurve easeCurve;

	void Start()
	{
		// Create a tween for each easeType
		int totTypes = Enum.GetNames(typeof(Ease)).Length;
		const int distX = 2;
		const int distY = 6;
		const int totCols = 10;
		float startX = -((totCols * distX) * 0.5f);
		float startY = (int)(totTypes / totCols) * distY * 0.5f;
		Vector2 gridCount = Vector2.zero;
		for (int i = 0; i < totTypes; ++i) {
			Transform t = ((GameObject)Instantiate(prefab)).transform;
			t.position = new Vector3(startX + distX * gridCount.x, startY - distY * gridCount.y, 0);
			gridCount.x++;
			if (gridCount.x > totCols) {
				gridCount.y++;
				gridCount.x = 0;
			}
			Tween tween = t.DOMoveY(2, 1).SetRelative().SetLoops(-1, LoopType.Yoyo);
			Ease easeType = (Ease)i;
			Debug.Log(i + ", " + easeType);
			if (easeType == Ease.INTERNAL_Custom) tween.SetEase(easeCurve);
			else tween.SetEase(easeType);
		}
	}
}