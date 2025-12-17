using UnityEngine;
using DG.Tweening;

public class DrawSegmentWithBounce : MonoBehaviour
{
	public Material material;

	bool isDrawing;
	LineRenderer currLine;
	Vector3 startPos, endPos;

	void Update()
	{
		if (isDrawing) {
			// Update line
			endPos = GetWorldMousePos();
			currLine.SetPosition(1, endPos);
		}
		if (Input.GetMouseButtonDown(0)) {
			// Start new line
			CreateLine();
			isDrawing = true;
		} else if (Input.GetMouseButtonUp(0) && isDrawing) {
			// End line and add collider
			isDrawing = false;
			EdgeCollider2D col = currLine.gameObject.AddComponent<EdgeCollider2D>();
	        col.points = new Vector2[] { startPos, endPos };

	        // Tween line ending
	        // Find max bounce distance (along the correct axis)
	        // Change the last value (0.65f) to increase or decrease the bounce distance (inverse)
	        Vector3 bouncePoint = (endPos - startPos) - ((endPos - startPos) * 0.65f);
	        Vector3 tweenP = endPos;
	        LineRenderer tweenedL = currLine;
	        // The last 3 parameters indicate:
	        // - the duration of the tween
	        // - the vibration (how much it will oscillate)
	        // - if the bounce should go beyond the end point or not (0 means not)
	        DOTween.Punch(()=> tweenP, x=> tweenP = x, -bouncePoint, 0.6f, 8, 0)
	        	.OnUpdate(()=> tweenedL.SetPosition(1, tweenP));
		}
	}

	void CreateLine()
	{
        currLine = new GameObject("Line").AddComponent<LineRenderer>();
        currLine.material = material;
        currLine.SetVertexCount(2);
        currLine.SetWidth(0.12f, 0.12f);
        currLine.material.color = new Color(0, 0.7f, 0.63f, 0.2f);
        currLine.useWorldSpace = false;
        startPos = GetWorldMousePos();
        currLine.SetPosition(0, startPos);
        currLine.SetPosition(1, startPos);
	}

	Vector3 GetWorldMousePos()
	{
		Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
		p.z = 0;
		return p;
	}
}