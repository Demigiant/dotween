using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : BrainBase
{
	public RectTransform cardRoot;
	public Button bt;

	Tween startTween;

	void Start()
	{
		Time.timeScale = 0;
		
		bt.onClick.AddListener(() => {
			string id = "card_00";
			DOTween.Kill(id, complete: false);
			cardRoot.anchoredPosition = Vector2.zero;
			cardRoot.localScale = Vector3.one;

			startTween = DOTween.Sequence()
				.SetId(id)
				.SetTarget(cardRoot)
				.SetUpdate(true)
				.Append(cardRoot.DOPunchScale(Vector3.one * 2f, 0.24f, vibrato: 1, elasticity: 0.2f)
					.SetUpdate(true) // Unnecessary
					.SetEase(Ease.OutQuad)
				)
				.OnKill(() => {
					cardRoot.anchoredPosition = Vector2.zero;
					cardRoot.localScale = Vector3.one;
				});
		});
	}
}