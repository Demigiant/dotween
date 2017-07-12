using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PathsWDrawPaths : MonoBehaviour
{
	public DOTweenPath path;
	public int subdivisionsXSegment = 10;
	public Color startColor = Color.white;
	public Color endColor = Color.white;
	public float startW = 1;
	public float endW = 1;
	public Material material;

	void Start()
	{
		Vector3[] drawPoints = path.GetTween().PathGetDrawPoints(subdivisionsXSegment);

		int pointsCount = drawPoints.Length;
		LineRenderer lr = path.gameObject.AddComponent<LineRenderer>();
		lr.material = material;
		lr.SetColors(startColor, endColor);
		lr.SetWidth(startW, endW);
		lr.SetVertexCount(pointsCount);
		for (int i = 0; i < pointsCount; ++i) lr.SetPosition(i, drawPoints[i]);
	}
}