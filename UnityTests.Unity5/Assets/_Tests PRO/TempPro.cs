using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TempPro : MonoBehaviour
{
	public float timeScale = 0;

	void Start()
	{
		Time.timeScale = timeScale;
	}
}