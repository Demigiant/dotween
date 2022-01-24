using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OnWaypointChangedTests : BrainBase
{
    public Transform target;
    public float tweenDuration = 0.5f;
    public int totLoops = 3;
    [Header("Main tests")]
    public bool testAllWPsDistant = true;
    public bool testFirstWPCoincides = true;
    public bool testLastWPCoincides = true;
    public bool testFirstAndLastWPsCoincide = true;
    [Header("Subtests")]
    public bool testOpenPathNoLoop = true;
    public bool testClosedPathNoLoop = true;
    public bool testOpenPathWRestartLoop = true;
    public bool testClosedPathWRestartLoop = true;
    public bool testOpenPathWYoyoLoop = true;
    public bool testClosedPathWYoyoLoop = true;

    Vector3 _targetOrP;
    Vector3[] _path_allWPsDistantFromTarget,
              _path_firstWPCoincidesWTarget,
              _path_lastWPCoincidesWTarget,
              _path_firstAndLastWPsCoincideWTarget;
    Tween[] _tweens;

    IEnumerator Start()
    {
        _targetOrP = target.position;
        _path_allWPsDistantFromTarget = new[] {
            _targetOrP + new Vector3(2, 1, 2),
            _targetOrP + new Vector3(4, 2, 4),
            _targetOrP + new Vector3(6, 3, 6)
        };
        _path_firstWPCoincidesWTarget = new[] {
            _targetOrP,
            _targetOrP + new Vector3(4, 2, 4),
            _targetOrP + new Vector3(6, 3, 6)
        };
        _path_lastWPCoincidesWTarget = new[] {
            _targetOrP + new Vector3(2, 1, 2),
            _targetOrP + new Vector3(4, 2, 4),
            _targetOrP
        };
        _path_firstAndLastWPsCoincideWTarget = new[] {
            _targetOrP,
            _targetOrP + new Vector3(4, 2, 4),
            _targetOrP
        };

        yield return new WaitForSeconds(1);

        if (testAllWPsDistant) {
            Debug.Log("<color=#00ff00>FIRST WP DISTANT FROM TARGET</color>");
            yield return CreateTweensAndWaitForCompletion(_path_allWPsDistantFromTarget);
        }
        if (testFirstWPCoincides) {
            Debug.Log("<color=#00ff00>FIRST WP COINCIDES WITH TARGET</color>");
            yield return CreateTweensAndWaitForCompletion(_path_firstWPCoincidesWTarget);
        }
        if (testLastWPCoincides) {
            Debug.Log("<color=#00ff00>LAST WP COINCIDES WITH TARGET</color>");
            yield return CreateTweensAndWaitForCompletion(_path_lastWPCoincidesWTarget);
        }
        if (testFirstAndLastWPsCoincide) {
            Debug.Log("<color=#00ff00>FIRST AND LAST WP COINCIDE WITH TARGET</color>");
            yield return CreateTweensAndWaitForCompletion(_path_firstAndLastWPsCoincideWTarget);
        }
    }

    YieldInstruction CreateTweensAndWaitForCompletion(Vector3[] path)
    {
        return StartCoroutine(CO_CreateTweensAndWaitForCompletion(path));
    }

    IEnumerator CO_CreateTweensAndWaitForCompletion(Vector3[] path)
    {
        Tween t;
        if (testOpenPathNoLoop) {
            target.position = _targetOrP;
            t = target.DOPath(path, tweenDuration, PathType.CatmullRom)
                .OnStart(() => Debug.Log("<color=#29b0cf>NO LOOPS :: Open path</color>"))
                .OnWaypointChange(x => Debug.Log("<color=#ffffff>" + x + "</color>"))
                .OnStepComplete(() => Debug.Log("<color=#ffffff>OnStepComplete</color>"))
                .OnComplete(() => Debug.Log("<color=#ffffff>Complete</color>"));
            yield return t.WaitForCompletion();
        }
        if (testClosedPathNoLoop) {
            target.position = _targetOrP;
            t = target.DOPath(path, tweenDuration, PathType.CatmullRom).SetOptions(true)
                .OnStart(() => Debug.Log("<color=#29b0cf>NO LOOPS :: Closed path</color>"))
                .OnWaypointChange(x => Debug.Log("<color=#ffffff>" + x + "</color>"))
                .OnStepComplete(() => Debug.Log("<color=#ffffff>OnStepComplete</color>"))
                .OnComplete(() => Debug.Log("<color=#ffffff>Complete</color>"));
            yield return t.WaitForCompletion();
        }
        if (testOpenPathWRestartLoop) {
            target.position = _targetOrP;
            t = target.DOPath(path, tweenDuration * 0.5f, PathType.CatmullRom)
                .SetLoops(totLoops, LoopType.Restart)
                .OnStart(() => Debug.Log("<color=#29b0cf>RESTART :: Open path</color>"))
                .OnWaypointChange(x => Debug.Log("<color=#ffffff>" + x + "</color>"))
                .OnStepComplete(() => Debug.Log("<color=#ffffff>OnStepComplete</color>"))
                .OnComplete(() => Debug.Log("<color=#ffffff>Complete</color>"));
            yield return t.WaitForCompletion();
        }
        if (testClosedPathWRestartLoop) {
            target.position = _targetOrP;
            t = target.DOPath(path, tweenDuration * 0.5f, PathType.CatmullRom).SetOptions(true)
                .SetLoops(totLoops, LoopType.Restart)
                .OnStart(() => Debug.Log("<color=#29b0cf>RESTART :: Closed path</color>"))
                .OnWaypointChange(x => Debug.Log("<color=#ffffff>" + x + "</color>"))
                .OnStepComplete(() => Debug.Log("<color=#ffffff>OnStepComplete</color>"))
                .OnComplete(() => Debug.Log("<color=#ffffff>Complete</color>"));
            yield return t.WaitForCompletion();
        }
        if (testOpenPathWYoyoLoop) {
            target.position = _targetOrP;
            t = target.DOPath(path, tweenDuration * 0.5f, PathType.CatmullRom)
                .SetLoops(totLoops, LoopType.Yoyo)
                .OnStart(() => Debug.Log("<color=#29b0cf>YOYO :: Open path</color>"))
                .OnWaypointChange(x => Debug.Log("<color=#ffffff>" + x + "</color>"))
                .OnStepComplete(() => Debug.Log("<color=#ffffff>OnStepComplete</color>"))
                .OnComplete(() => Debug.Log("<color=#ffffff>Complete</color>"));
            yield return t.WaitForCompletion();
        }
        if (testClosedPathWYoyoLoop) {
            target.position = _targetOrP;
            t = target.DOPath(path, tweenDuration * 0.5f, PathType.CatmullRom).SetOptions(true)
                .SetLoops(totLoops, LoopType.Yoyo)
                .OnStart(() => Debug.Log("<color=#29b0cf>YOYO :: Closed path</color>"))
                .OnWaypointChange(x => Debug.Log("<color=#ffffff>" + x + "</color>"))
                .OnStepComplete(() => Debug.Log("<color=#ffffff>OnStepComplete</color>"))
                .OnComplete(() => Debug.Log("<color=#ffffff>Complete</color>"));
            yield return t.WaitForCompletion();
        }
    }
}
