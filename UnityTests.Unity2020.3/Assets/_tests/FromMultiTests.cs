using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FromMultiTests : MonoBehaviour
{
    public float stepDuration = 2;
    public float interval = 1;
    public Ease ease = Ease.InCubic;
    public int intFrom, intFromRelative, intFromNotImmediate, intFromNotImmediateRelative;
    public int intFrom50, intFrom50Relative, intFrom50NotImmediate, intFrom50NotImmediateRelative;
    public Text tfFrom, tfFromRelative, tfFromNotImmediate, tfFromNotImmediateRelative;
    public Text tfFrom50, tfFrom50Relative, tfFrom50NotImmediate, tfFrom50NotImmediateRelative;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            DOTween.KillAll();
            CreateSequences();
        } else if (Input.GetKeyDown(KeyCode.R)) {
            DOTween.KillAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) DOTween.RewindAll();
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) DOTween.PlayBackwardsAll();
        else if (Input.GetKeyDown(KeyCode.RightArrow)) DOTween.PlayForwardAll();
    }

    void CreateSequences()
    {
        intFrom = int.Parse(tfFrom.text);
        DOTween.Sequence().SetAutoKill(false)
            .Append(DOTween.To(() => intFrom, x => intFrom = x, 15, stepDuration).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFrom, x => intFrom = x, 100, stepDuration).From().SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFrom, x => intFrom = x, 30, stepDuration).SetEase(ease))
            .OnUpdate(()=> tfFrom.text = intFrom.ToString());

        intFromRelative = int.Parse(tfFromRelative.text);
        DOTween.Sequence().SetAutoKill(false)
            .Append(DOTween.To(() => intFromRelative, x => intFromRelative = x, 15, stepDuration).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFromRelative, x => intFromRelative = x, 100, stepDuration).From(true).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFromRelative, x => intFromRelative = x, 30, stepDuration).SetEase(ease))
            .OnUpdate(()=> tfFromRelative.text = intFromRelative.ToString());

        intFromNotImmediate = int.Parse(tfFromNotImmediate.text);
        DOTween.Sequence().SetAutoKill(false)
            .Append(DOTween.To(() => intFromNotImmediate, x => intFromNotImmediate = x, 15, stepDuration).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFromNotImmediate, x => intFromNotImmediate = x, 100, stepDuration).From(false, false).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFromNotImmediate, x => intFromNotImmediate = x, 30, stepDuration).SetEase(ease))
            .OnUpdate(()=> tfFromNotImmediate.text = intFromNotImmediate.ToString());

        intFromNotImmediateRelative = int.Parse(tfFromNotImmediateRelative.text);
        DOTween.Sequence().SetAutoKill(false)
            .Append(DOTween.To(() => intFromNotImmediateRelative, x => intFromNotImmediateRelative = x, 15, stepDuration).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFromNotImmediateRelative, x => intFromNotImmediateRelative = x, 100, stepDuration).From(false, true).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFromNotImmediateRelative, x => intFromNotImmediateRelative = x, 30, stepDuration).SetEase(ease))
            .OnUpdate(()=> tfFromNotImmediateRelative.text = intFromNotImmediateRelative.ToString());

        // -------------------------------------------

        intFrom50 = int.Parse(tfFrom50.text);
        DOTween.Sequence().SetAutoKill(false)
            .Append(DOTween.To(() => intFrom50, x => intFrom50 = x, 15, stepDuration).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFrom50, x => intFrom50 = x, 50, stepDuration).From(100).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFrom50, x => intFrom50 = x, 30, stepDuration).SetEase(ease))
            .OnUpdate(()=> tfFrom50.text = intFrom50.ToString());

        intFrom50Relative = int.Parse(tfFrom50Relative.text);
        DOTween.Sequence().SetAutoKill(false)
            .Append(DOTween.To(() => intFrom50Relative, x => intFrom50Relative = x, 15, stepDuration).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFrom50Relative, x => intFrom50Relative = x, 50, stepDuration).From(100, true, true).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFrom50Relative, x => intFrom50Relative = x, 30, stepDuration).SetEase(ease))
            .OnUpdate(()=> tfFrom50Relative.text = intFrom50Relative.ToString());

        intFrom50NotImmediate = int.Parse(tfFrom50NotImmediate.text);
        DOTween.Sequence().SetAutoKill(false)
            .Append(DOTween.To(() => intFrom50NotImmediate, x => intFrom50NotImmediate = x, 15, stepDuration).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFrom50NotImmediate, x => intFrom50NotImmediate = x, 50, stepDuration).From(100, false).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFrom50NotImmediate, x => intFrom50NotImmediate = x, 30, stepDuration).SetEase(ease))
            .OnUpdate(()=> tfFrom50NotImmediate.text = intFrom50NotImmediate.ToString());

        intFrom50NotImmediateRelative = int.Parse(tfFrom50NotImmediateRelative.text);
        DOTween.Sequence().SetAutoKill(false)
            .Append(DOTween.To(() => intFrom50NotImmediateRelative, x => intFrom50NotImmediateRelative = x, 15, stepDuration).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFrom50NotImmediateRelative, x => intFrom50NotImmediateRelative = x, 50, stepDuration).From(100, false, true).SetEase(ease))
            .AppendInterval(interval)
            .Append(DOTween.To(() => intFrom50NotImmediateRelative, x => intFrom50NotImmediateRelative = x, 30, stepDuration).SetEase(ease))
            .OnUpdate(()=> tfFrom50NotImmediateRelative.text = intFrom50NotImmediateRelative.ToString());
    }
}