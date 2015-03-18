using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : MonoBehaviour
{
	void Start()
	{
		SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();

		DOTween.To(
            () => sprite.color,
            x => sprite.color = x,
            new Color(sprite.color.r,
                sprite.color.g,
                sprite.color.b,
                0),
            0.2f).SetId("instructions").SetLoops(6, LoopType.Yoyo).OnComplete(OnComplete);
	}

	void OnComplete()
	{
		DOTween.Kill("instructions");
		if(gameObject)
        Destroy(gameObject);
	}
}