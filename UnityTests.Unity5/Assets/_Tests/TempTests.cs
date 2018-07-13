using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempTests : BrainBase
{
    public Image target;

    IEnumerator Start()
    {
        Sequence _passenger_arrow_out = DOTween.Sequence();
        _passenger_arrow_out.Insert(0, target.GetComponent<RectTransform>().DOAnchorPos(new Vector2(300f, 26f), 1f));
        _passenger_arrow_out.Insert(0, target.DOFade(0f, 0.8f));
        _passenger_arrow_out.SetAutoKill(false);
        _passenger_arrow_out.Pause();

        Sequence _passenger_arrow_in = DOTween.Sequence();
        _passenger_arrow_in.Insert(0, target.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-200f, 26f), 1f));
        _passenger_arrow_in.Insert(0, target.DOFade(1f, 0.8f));
        _passenger_arrow_in.SetAutoKill(false);
        _passenger_arrow_in.Pause();

        yield return new WaitForSeconds(1);

        _passenger_arrow_out.Rewind();
        _passenger_arrow_out.PlayForward();
    }
}