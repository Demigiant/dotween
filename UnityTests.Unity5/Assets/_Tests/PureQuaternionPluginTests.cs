using UnityEngine;
using System.Collections;
using DG.Tweening;
using DG.Tweening.CustomPlugins;

public class PureQuaternionPluginTests : BrainBase
{
	public Transform target;
	public Vector3 to;
	public bool isRelative;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);
		target.DORotateQuaternion(Quaternion.Euler(to), 3).SetLoops(-1, LoopType.Yoyo).SetRelative(isRelative);
	}
}