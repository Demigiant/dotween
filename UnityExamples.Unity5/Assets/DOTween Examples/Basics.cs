using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Basics : MonoBehaviour
{
	public Transform redCube, greenCube, blueCube, purpleCube;

	IEnumerator Start()
	{
		// Start after one second delay (to ignore Unity hiccups when activating Play mode in Editor)
		yield return new WaitForSeconds(1);

		// Let's move the red cube TO 0,4,0 in 2 seconds
		redCube.DOMove(new Vector3(0,4,0), 2);

		// Let's move the green cube FROM 0,4,0 in 2 seconds
		greenCube.DOMove(new Vector3(0,4,0), 2).From();
		
		// Let's move the blue cube BY 0,4,0 in 2 seconds
		blueCube.DOMove(new Vector3(0,4,0), 2).SetRelative();

		// Let's move the purple cube BY 6,0,0 in 2 seconds
		// and also change its color to yellow.
		// To change its color, we'll have to use its material as a target (instead than its transform).
		purpleCube.DOMove(new Vector3(6,0,0), 2).SetRelative();
		// Also, let's set the color tween to loop infinitely forward and backwards
		purpleCube.GetComponent<Renderer>().material.DOColor(Color.yellow, 2).SetLoops(-1, LoopType.Yoyo);
	}
}