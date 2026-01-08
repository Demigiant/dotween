using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UIFillAmount : MonoBehaviour
{
	public Image fillImage;
	public float tweenTime = 2;

	void Start()
	{
		fillImage.DOFillAmount(0, tweenTime).SetLoops(-1, LoopType.Yoyo);
	}
}