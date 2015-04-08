using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public Transform target;

    public void TweenThis(int direction)
    {
    	Debug.Log("HERE");
    	if (DOTween.IsTweening("infoTabSwipeAnim")) {
    		Debug.Log("IsTweening");
    		// DOTween.Rewind("infoTabSwipeAnim", false);                    
    		DOTween.Kill("infoTabSwipeAnim", true);
    	}
    	                
    	target.DOLocalMoveX(direction*800, 0.3f, false).From().SetEase(Ease.OutBack).SetId("infoTabSwipeAnim");
    }
}