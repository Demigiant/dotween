using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempTests : MonoBehaviour
{
    public TMP_Text t;
    public float duration = 1;
    public string toText = "Some...<sprite=3>";

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.8f);
        // t.DOText("", duration).From().SetEase(Ease.Linear);
        t.DOText(toText, duration).SetEase(Ease.Linear)
            .OnComplete(()=> Debug.Log("COMPLETE"));
    }
}