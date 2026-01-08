using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TrailAndLineRenderer : BrainBase
{
	public LineRenderer lineRenderer;
	public TrailRenderer trailRenderer;

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("Tween LineRenderer Colors")) {
			lineRenderer.DOColor(new Color2(Color.white, Color.white), new Color2(Color.red, Color.white), 2).SetLoops(-1, LoopType.Yoyo);
		}
		if (GUILayout.Button("Tween TrailRenderer's Sizes")) {
			trailRenderer.DOResize(2, 4, 2).SetLoops(-1, LoopType.Yoyo);
		}
		if (GUILayout.Button("Tween TrailRenderer's Time")) {
			trailRenderer.DOTime(0.001f, 2).SetLoops(-1, LoopType.Yoyo);
		}

		DGUtils.EndGUI();
	}
}
