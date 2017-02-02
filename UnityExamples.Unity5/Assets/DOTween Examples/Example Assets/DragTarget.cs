using UnityEngine;
using System.Collections;

public class DragTarget : MonoBehaviour
{
	Transform t;
	Camera mainCam;
	Vector3 offset;

	void Start()
	{
		t = this.transform;
		mainCam = Camera.main;
	}

	void OnMouseDown()
	{
		Vector2 mousePos = Input.mousePosition;
		float distance = mainCam.WorldToScreenPoint(t.position).z;
 		Vector3 worldPos = mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distance));
		offset = t.position - worldPos;
	}

	void OnMouseDrag()
	{
		Vector2 mousePos = Input.mousePosition;
		float distance = mainCam.WorldToScreenPoint(t.position).z;
 		t.position = mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distance)) + offset;
	}
}