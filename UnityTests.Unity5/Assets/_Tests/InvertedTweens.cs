using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InvertedTweens : MonoBehaviour
{
    enum CommandType
    {
        Rewind,
        Complete,
        Restart,
        Play,
        Pause,
        PlayBackwards,
        PlayForward
    }

    public float duration = 1;
    public bool invert;
    public bool autoKill = true;
    [Range(1, 10)]
    public int loops = 1;
    public LoopType loopType = LoopType.Yoyo;
    public bool composedSequence = true;
    public Transform tweenTarget, sequenceTarget;

    float creationTime;
    Tween t;
    Sequence s;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        Debug.Log("CREATION ► tween: " + VerboseVector3(tweenTarget.position) + ", sequence: " + VerboseVector3(sequenceTarget.position));
        creationTime = Time.time;

        t = tweenTarget.DOMoveX(2, duration).SetRelative()
            .SetAutoKill(autoKill)
            .SetLoops(loops, loopType)
            .OnRewind(() => Log("TWEEN Rewinded"))
            .OnComplete(() => Log("TWEEN Completed ► " + VerboseVector3(tweenTarget.position)))
            .OnKill(() => Log("TWEEN Killed"));
        t.isInverted = invert;

        s = DOTween.Sequence()
            .SetAutoKill(autoKill)
            .SetLoops(loops, loopType)
            .InsertCallback(duration * 0.75f, () => Log("SEQUENCE 3/4 callback", "00ff00"))
            .OnRewind(() => Log("SEQUENCE Rewinded"))
            .OnComplete(() => Log("SEQUENCE Completed ►" + VerboseVector3(sequenceTarget.position)))
            .OnKill(() => Log("SEQUENCE Killed"));
        if (composedSequence) {
            s.Insert(0, sequenceTarget.DOMoveX(2, duration * 0.25f).SetRelative()
                    .OnRewind(() => Log("INNER 1/4 Rewinded", "00ff00"))
                    .OnComplete(() => Log("INNER 1/4 Completed", "00ff00"))
                )
                .Insert(duration * 0.25f, sequenceTarget.DOMoveX(-2, duration * 0.75f).SetRelative()
                    .OnRewind(() => Log("INNER 3/4 Rewinded", "00ff00"))
                    .OnComplete(() => Log("INNER 3/4 Completed", "00ff00"))
                );
        } else {
            s.Insert(0, sequenceTarget.DOMoveX(2, duration).SetRelative()
                .OnRewind(() => Log("INNER Rewinded", "00ff00"))
                .OnComplete(() => Log("INNER Completed", "00ff00")));
        }
        s.isInverted = invert;

        Debug.Log("CREATED ► tween: " + VerboseVector3(tweenTarget.position) + ", sequence: " + VerboseVector3(sequenceTarget.position));
    }

    string VerboseVector3(Vector3 v)
    {
        return v.ToString("N6");
    }

    void Log(string message, string hex = null)
    {
        string m = string.Format("<color=#ffab27>{0:N0}%</color> ", ((Time.time - creationTime) * 100) / duration);
        if (hex != null) m += string.Format("<color=#{0}>{1}</color>", hex, message);
        else m += message;
        Debug.Log(m);
    }

    public void Tween_Rewind()
    { TweenCommand(CommandType.Rewind); }
    public void Tween_Complete()
    { TweenCommand(CommandType.Complete); }
    public void Tween_Restart()
    { TweenCommand(CommandType.Restart); }
    public void Tween_Play()
    { TweenCommand(CommandType.Play); }
    public void Tween_Pause()
    { TweenCommand(CommandType.Pause); }
    public void Tween_PlayBackwards()
    { TweenCommand(CommandType.PlayBackwards); }
    public void Tween_PlayForward()
    { TweenCommand(CommandType.PlayForward); }

    void TweenCommand(CommandType command)
    {
        if (t == null) return;
        switch (command) {
        case CommandType.Rewind:
            t.Rewind();
            s.Rewind();
            break;
        case CommandType.Complete:
            t.Complete();
            s.Complete();
            break;
        case CommandType.Restart:
            t.Restart();
            s.Restart();
            break;
        case CommandType.Play:
            t.Play();
            s.Play();
            break;
        case CommandType.Pause:
            t.Pause();
            s.Pause();
            break;
        case CommandType.PlayBackwards:
            t.PlayBackwards();
            s.PlayBackwards();
            break;
        case CommandType.PlayForward:
            t.PlayForward();
            s.PlayForward();
            break;
        }
    }
}