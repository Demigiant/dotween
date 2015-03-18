using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public Image img;
	Sequence _seq;

	void Start()
	{
		Debug.Log("START");
		_seq = DOTween.Sequence().SetId("SEQ");
        _seq.Append(img.DOFade(1, 0.5f));
        _seq.AppendInterval(3f);
        _seq.Append(img.DOFade(0, 0.5f));
        _seq.AppendCallback(BalloonComplete);
        // _seq.OnComplete(BalloonComplete);
	}

	void BalloonComplete()
	{
		Debug.Log("Complete");
		_seq.Rewind();
		Debug.Log("Rewind called");
	}

	public void Goforit()
	{
		Debug.Log("GO");
		_seq.Restart();
	}
}