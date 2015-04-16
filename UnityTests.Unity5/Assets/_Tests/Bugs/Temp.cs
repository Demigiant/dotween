using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public RectTransform t;

    IEnumerator Start()
    {
    	yield return new WaitForSeconds(0.6f);
    	DOTween.To(() => t.anchorMin, (x) => t.anchorMin = x, new Vector2(-1,0) , 2.0f).OnComplete(()=>Debug.Log(t.anchorMin.ToString("N16")));
    }
}