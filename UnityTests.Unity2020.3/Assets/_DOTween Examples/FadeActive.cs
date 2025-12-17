using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FadeActive : MonoBehaviour
{
	public SpriteRenderer[] fadeTargets;
	Sequence fadeTween;

	void Start()
	{
		fadeTween = DOTween.Sequence().SetAutoKill(false).Pause();
		foreach (SpriteRenderer r in fadeTargets) fadeTween.Append(r.DOFade(0, 1));
		fadeTween.OnComplete(()=> {
			foreach (SpriteRenderer r in fadeTargets) r.gameObject.SetActive(false);
		});
	}

	public void FadeOut()
	{
		fadeTween.PlayForward();
	}

	public void FadeIn()
	{
		foreach (SpriteRenderer r in fadeTargets) r.gameObject.SetActive(true);
		fadeTween.PlayBackwards();
	}
}