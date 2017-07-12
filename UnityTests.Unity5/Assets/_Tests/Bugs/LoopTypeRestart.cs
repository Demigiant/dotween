using UnityEngine;
using System.Collections;
using DG.Tweening;

public class LoopTypeRestart : BrainBase
{
	public enum SequenceType
	{
		Test,
		Bugged
	}
	public SequenceType sequencetype;
	public bool initialInterval;
	public bool midIntervals = true;
	public LoopType loopType = LoopType.Restart;
	public Transform target;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.6f);

		Sequence s = DOTween.Sequence();
		s.SetLoops(-1, loopType);

		if (sequencetype == SequenceType.Bugged) {
			if (initialInterval) s.AppendInterval(0.5f);
	        s.AppendCallback(()=> {
	        	Debug.Log("Callback A");
	        	MoveSpriteToPoint(new Vector3(0, 0, 0));
	    	});
			if (midIntervals) s.AppendInterval(0.5f);
	        s.AppendCallback(()=> {
	        	Debug.Log("Callback B");
	        	MoveSpriteToPoint(new Vector3(2, 0, 0));
	    	});
			if (midIntervals) s.AppendInterval(0.5f);
		} else {
			s.Append(target.DOMoveX(3, 0.5f));
			s.Append(target.DORotate(new Vector3(0, 180, 0), 0.5f));
		}
	}

	void MoveSpriteToPoint(Vector3 destPos)
	{
		target.DOMove(destPos, 0.5f);
    }
}