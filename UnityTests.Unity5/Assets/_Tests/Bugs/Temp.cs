using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public Transform target;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.6f);

        while (true) {
            if (Time.frameCount % 10 == 0) {
                // Spawn empty sequences
                for (int i = 0; i < 200; ++i) {
                    Sequence s = DOTween.Sequence();
                }
                yield return null;
            }
            yield return null;
        }
    }
}