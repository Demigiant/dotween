using UnityEngine;
using DG.Tweening;

public class PooledChildAnimation : MonoBehaviour
{
	public void OnSpawn()
	{
//		DOTweenAnimation doanim = this.GetComponentInChildren<DOTweenAnimation>();
//		if (doanim) doanim.DORestart(true);
//		DOTweenPath dopath = this.GetComponentInChildren<DOTweenPath>();
//		if (dopath) dopath.DORestart(true);
	}

	public void OnDespawn()
	{
//		DOTweenAnimation doanim = this.GetComponentInChildren<DOTweenAnimation>();
//		if (doanim) doanim.DORewind();
//		DOTweenPath dopath = this.GetComponentInChildren<DOTweenPath>();
//		if (dopath) dopath.DORewind();
	}
}