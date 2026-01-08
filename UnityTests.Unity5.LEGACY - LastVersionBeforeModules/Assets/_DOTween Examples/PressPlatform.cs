using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PressPlatform : MonoBehaviour
{
	public float speed = 2;

	Tweener tween;

	void Start()
	{
		// Create the tween
		tween = transform.DOLocalMoveY(-4, speed).SetAutoKill(false).Pause();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			tween.PlayForward();
		} else if (Input.GetKeyUp(KeyCode.DownArrow)) {
			tween.PlayBackwards();
		}
	}
}