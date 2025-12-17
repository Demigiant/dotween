using DG.Tweening;
using System.Collections;
using UnityEngine;

// Code to replicate by Andrey Timofeev: https://github.com/Demigiant/dotween/pull/288/files#diff-0
public class IndexOutOfRange04 : BrainBase
{
    public static int updateCounter = -1;

    int _callCounter = 0;

    void Start()
    {
        Time.maximumDeltaTime = 0.02f;
        DOTween.Init(true, true, LogBehaviour.Verbose);
        DOTween.SetTweensCapacity(1000, 1000);
        StartCoroutine(CoTest());
    }

    IEnumerator CoTest()
    {
        updateCounter = -1;
        yield return null;
        yield return null;
        yield return null;
        updateCounter = 0;

        StartSequence(0.75f, true); // callNumber 1

        yield return new WaitForSeconds(0.5f);

        var t0 = StartSequence(0.75f, false); // callNumber 2

        yield return new WaitForSeconds(0.5f);

        var t1 = StartSequence(99999f, false); // callNumber 3
        StartSequence(99999f, false); // callNumber 4

        while (t1.active) yield return null;
//        while (t0.active) yield return null;
        Debug.Log("Done waiting for tween");

        StartSequence(99999f, true); // callNumber 5
    }

    Tweener StartSequence(float duration, bool failStart)
    {
        int callNumber = ++_callCounter;
        Debug.Log(string.Format("StartSequence, callNumber:{0} duration:{1} failStart:{2}", callNumber, duration, failStart));

        GameObject go = new GameObject(callNumber.ToString());

        var sequence = DOTween.Sequence();
        var tweener = go.transform.DOMoveX(callNumber + 1000, duration);
        sequence.Append(tweener);

        if (failStart) {
            Debug.Log(string.Format("Destroying gameobject, callNumber {0}", callNumber));
            Destroy(go);
        }

        return tweener;
    }

    void Update()
    {
//        if (updateCounter >= 0) {
//            Debug.Log(string.Format("Update {0}, time: {1}", updateCounter, Time.time));
//            ++updateCounter;
//        }
    }
}
