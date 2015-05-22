using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TempPro : MonoBehaviour
{
	void Start()
	{
		transform.DOMove(new Vector3(4,0,0), 1).From(true).SetDelay(1);
	}
}