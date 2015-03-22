using DG.Tweening;
using UnityEngine;
using System.Collections;

public class KillVSKillTarget : MonoBehaviour
{
	public Transform[] targets;

	void Start()
	{
		targets[0].DOMoveX(2, 2).SetRelative().SetId(1);
		targets[1].DOMoveY(2, 2).SetRelative();
		DOTween.Kill(targets[0]);
		// DOTween.Kill(1);
		// DOTween.Kill(null);
	}
}