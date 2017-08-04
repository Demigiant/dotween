using UnityEngine;
using System.Collections;
using DG.Tweening;

public class UpdateTypes : BrainBase
{
	public Transform[] targets;
	public bool independentUpdate;
	public float timeScale = 1;

    bool runningManual = false;

	void Start()
	{
		Time.timeScale = timeScale;

		targets[0].DOMoveX(5, 2).SetUpdate(UpdateType.Normal, independentUpdate).SetLoops(-1, LoopType.Yoyo);
		targets[1].DOMoveX(5, 2).SetUpdate(UpdateType.Late, independentUpdate).SetLoops(-1, LoopType.Yoyo);
		targets[2].DOMoveX(5, 2).SetUpdate(UpdateType.Fixed, independentUpdate).SetLoops(-1, LoopType.Yoyo);
		targets[3].GetComponent<Rigidbody>().DOMoveX(5, 2).SetUpdate(UpdateType.Fixed, independentUpdate).SetLoops(-1, LoopType.Yoyo);
		targets[4].DOMoveX(5, 2).SetUpdate(UpdateType.Manual, independentUpdate).SetLoops(-1, LoopType.Yoyo);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            runningManual = !runningManual;
            if (runningManual) this.StartCoroutine(CO_ManualUpdate());
            else this.StopAllCoroutines();
        }
    }

    IEnumerator CO_ManualUpdate()
    {
        while (true) {
            yield return null;
            DOTween.ManualUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        }
    }
}