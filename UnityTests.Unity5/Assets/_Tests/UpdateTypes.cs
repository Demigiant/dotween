using UnityEngine;
using System.Collections;
using DG.Tweening;

public class UpdateTypes : BrainBase
{
	public Transform[] targets;
	public bool independentUpdate;
	public float timeScale = 1;

	void Start()
	{
		Time.timeScale = timeScale;

		targets[0].DOMoveX(5, 2).SetUpdate(UpdateType.Normal, independentUpdate).SetLoops(-1, LoopType.Yoyo);
		targets[1].DOMoveX(5, 2).SetUpdate(UpdateType.Late, independentUpdate).SetLoops(-1, LoopType.Yoyo);
		targets[2].DOMoveX(5, 2).SetUpdate(UpdateType.Fixed, independentUpdate).SetLoops(-1, LoopType.Yoyo);
		targets[3].GetComponent<Rigidbody>().DOMoveX(5, 2).SetUpdate(UpdateType.Fixed, independentUpdate).SetLoops(-1, LoopType.Yoyo);
	}
}