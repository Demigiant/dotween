using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TempPro : MonoBehaviour
{
	public DOTweenPath tweenP;

	void Start()
	{
		Debug.Log(tweenP.GetTween().PathGetPoint(0.5f));
	}
}