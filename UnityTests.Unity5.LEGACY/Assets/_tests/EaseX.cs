using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class EaseX : MonoBehaviour
{
	public Image imgFlash, imgSmoothFlash;
	public Color flashColor;
	[Header("Tween Settings")]
	public float duration = 2;
	public Ease ease;
	public float overshootOrAmplitude = 16;
	public float period = 1;

	public void Play()
	{
		DOTween.RewindAll();
		DOTween.KillAll();
		
		imgFlash.DOColor(flashColor, duration).SetEase(ease, overshootOrAmplitude, period);
		imgSmoothFlash.DOColor(flashColor, duration).SetEase(ease, overshootOrAmplitude - 1, period);
	}
}