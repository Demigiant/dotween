using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TempPro : MonoBehaviour
{
	void OnMouseDown()
	{
		Debug.Log("DOWN");
		this.GetComponent<DOTweenPath>().DOPlay();
	}
}