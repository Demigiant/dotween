using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

public class TempTests : MonoBehaviour
{
	Sequence sequence;

	void Start()
	{
		sequence = DOTween.Sequence();              
		sequence.AppendInterval(3);               
		sequence.AppendCallback(() => {
			this.transform.GetComponent<MeshRenderer>().enabled = !this.transform.GetComponent<MeshRenderer>().enabled;
		});

		sequence.SetLoops(-1, LoopType.Restart);
		sequence.Play();
	}

	void OnDestroy()
    {
        if (sequence != null) {
        	Debug.Log("Killing Sequence");
        	sequence.Kill();
        }
    }
}