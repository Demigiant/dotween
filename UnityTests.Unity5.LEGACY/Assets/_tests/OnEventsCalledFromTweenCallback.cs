using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OnEventsCalledFromTweenCallback : MonoBehaviour
{
    public bool createTween = true;
    public bool createSequence = true;
    public bool completeWCallbacks = false;
    public bool onUpdateCallFirst = true;
    public Transform target;

    bool _testCalled;
    Tween _tween, _sequence;

    void Start()
    {
        if (createTween) {
            Debug.Log("<color=#71cfe9>Creating TWEEN</color>");
            _tween = target.DOMoveX(2, 1).SetId("TWEEN")
                .OnComplete(() => Debug.Log("<color=#00ff00>Tween complete</color>"));
        }

        if (createSequence) {
            Debug.Log("<color=#71cfe9>Creating SEQUENCE</color>");
            _sequence = DOTween.Sequence().SetId("SEQUENCE").SetAutoKill(false)
                .AppendCallback(() => Debug.Log("<color=#71cfe9>Sequence internal callback 0</color>"))
                .Append(target.DOMoveY(2, 1).OnComplete(() => Debug.Log("<color=#71cfe9>Nested tween complete</color>")))
                .AppendCallback(() => Debug.Log("<color=#71cfe9>Sequence internal callback 1</color>"))
                .OnRewind(() => Debug.Log("<color=#71cfe9>Sequence rewound</color>"))
                .OnPlay(() => Debug.Log("<color=#71cfe9>Sequence onPlay</color>"))
                .OnComplete(() => Debug.Log("<color=#00ff00>Sequence complete</color>"));
        }

        Debug.Log("<color=#71cfe9>Creating CALLER</color>");
        DOTween.Sequence().SetId("CALLER")
            .InsertCallback(onUpdateCallFirst ? 3f : 0.5f, () => Test(false))
            .AppendInterval(3)
            .OnUpdate(() => {
                if (!_testCalled && Time.time > (onUpdateCallFirst ? 0.5f : 3)) Test(true);
            })
            .OnComplete(() => Debug.Log("<color=#71cfe9>CALLER complete</color>"));
    }

    void Test(bool fromOnUpdate)
    {
        _testCalled = true;

        if (fromOnUpdate) Debug.Log("<color=#71cfe9>TEST called from CALLER OnUpdate</color>");
        else Debug.Log("<color=#71cfe9>TEST called from CALLER callback</color>");
        if (_tween.IsActive()) _tween.Complete(completeWCallbacks);
        if (_sequence.IsActive()) _sequence.Complete(completeWCallbacks);
    }
}