using UnityEngine;
using System.Collections;
using DG.Tweening;

public class IndexOutOfRangeReproduced : BrainBase
{
	public Transform target;

	int totDelayedCalls;
	int totCreatedTweens;

	void Start()
	{
		// DOTween.SetTweensCapacity(200, 50);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            for (int j = 0; j < 20; j++) {
            	totDelayedCalls++;
            	DOVirtual.DelayedCall(0.1f * j, MakeTween);
            }
        }
    }

    void MakeTween()
    {
        for (int i = 0; i < 50; i++) {
        	totCreatedTweens++;
        	Tween t = target.DOMove(new Vector3(1, 1), 0.5f);
        	if (i == 49) t.OnComplete(()=> DOTween.KillAll());
        	t.Goto(100);
        }
    }

    void OnGUI()
    {
    	GUILayout.Label("Tweens: " + totCreatedTweens);
    	GUILayout.Label("Delayed Calls: " + totDelayedCalls);
    }
}