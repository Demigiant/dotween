using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MaterialLerp : BrainBase
{
	public Renderer target;
	public Material switchMaterial;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.7f);

		float n = 0;
		DOTween.To(()=> n, x=> n = x, 1, 2).SetEase(Ease.Linear)
			.OnUpdate(()=> {
				target.material.Lerp(target.material, switchMaterial, n);
			});
	}
}