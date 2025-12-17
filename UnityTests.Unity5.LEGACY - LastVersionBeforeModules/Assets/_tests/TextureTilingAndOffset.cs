using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TextureTilingAndOffset : BrainBase
{
	public Renderer rend;

	void Start()
	{
		rend.material.DOTiling(new Vector2(3, 3), 2).SetLoops(-1, LoopType.Yoyo);
		rend.material.DOOffset(new Vector2(1, 0), "_Detail", 2).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
	}
}