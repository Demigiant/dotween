using UnityEngine;
using System.Collections;
using DG.Tweening;
using DG.Tweening.CustomPlugins;

public class PureQuaternionPluginTests : BrainBase
{
	public Transform target;
	public Transform toTarget;
	public Vector3 to;
	public bool isRelative;
	public bool useShortcut = true;
	public bool useToTarget;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);
		if (useShortcut) target.DORotateQuaternion(useToTarget ? toTarget.rotation : Quaternion.Euler(to), 3).SetLoops(-1, LoopType.Yoyo).SetRelative(isRelative);
		else DOTween.To(PureQuaternionPlugin.Plug(), ()=> target.rotation, x=> target.rotation = x, useToTarget ? toTarget.rotation : Quaternion.Euler(to), 3).SetRelative(isRelative);
	}
}