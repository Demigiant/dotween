using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class DORotateTests : BrainBase
{
    public enum TweenType
    {
        Normal,
        FromCurrent,
        FromDirect
    }

    [Header("Main")]
    public TweenType tweenType = TweenType.Normal;
    float tweenDuration = 2;
    public bool relative = false;
    public Vector2 offsetBetweenDices = new Vector2(7, 4);
    public int maxDicesPerRow = 4;
    [Header("Fast")]
    public Vector3[] fastStartValues = new[] {
        new Vector3(0, 0, 0),
        new Vector3(85, 0, 0),
        new Vector3(95, 0, 0),
        new Vector3(180, 0, 0),
        new Vector3(0, 85, 0),
        new Vector3(0, 95, 0),
        new Vector3(0, 0, 85),
        new Vector3(0, 0, 95),
        new Vector3(180, 0, 0),
    };
    public Vector3 fastEndValue = new Vector3(100, 0, 0);
    [Header("Beyond360")]
    public Vector3[] beyond360StartValues = new[] {
        new Vector3(0, 0, 0),
        new Vector3(85, 0, 0),
        new Vector3(95, 0, 0),
        new Vector3(180, 0, 0),
        new Vector3(0, 85, 0),
        new Vector3(0, 95, 0),
        new Vector3(0, 0, 85),
        new Vector3(0, 0, 95),
        new Vector3(180, 0, 0),
    };
    public Vector3 beyond360EndValue = new Vector3(100, 0, 0);

    GameObject[] _dices;
    GameObject _diceGroupPrefab;
    RotateMode _currRotateMode;

    #region Main Methods

    void Start()
    {
        _diceGroupPrefab = GameObject.Find("n:DiceGroupPrefab");
        _diceGroupPrefab.SetActive(false);
    }

    // Create and distribute dices, then set their rotation
    void SetupFor(RotateMode mode)
    {
        DOTween.KillAll();
        // Destroy previous
        if (_dices != null) {
            for (int i = 0; i < _dices.Length; ++i) Destroy(_dices[i]);
            _dices = null;
        }
        // Create and distribute
        _currRotateMode = mode;
        Vector3[] startVals;
        Vector3 endVal;
        switch (mode) {
        case RotateMode.FastBeyond360:
            startVals = beyond360StartValues;
            endVal = beyond360EndValue;
            break;
        default:
            startVals = fastStartValues;
            endVal = fastEndValue;
            break;
        }
        int totDices = startVals.Length;
        int totRows = Mathf.CeilToInt(totDices / (float)maxDicesPerRow);
        Vector3 startP = new Vector3(
            -(offsetBetweenDices.x * (Mathf.Min(totDices, maxDicesPerRow) - 1)) * 0.5f,
            (offsetBetweenDices.y * (totRows + 1)) * 0.5f,
            0
        );
        int xIndex = -1;
        _dices = new GameObject[totDices];
        for (int i = 0; i < totDices; ++i) {
            if (i % maxDicesPerRow == 0) {
                xIndex = -1;
                startP.y -= offsetBetweenDices.y;
            }
            xIndex++;
            _dices[i] = Instantiate(_diceGroupPrefab, _diceGroupPrefab.transform.parent);
            _dices[i].name = "Dice " + i;
            _dices[i].SetActive(true);
            _dices[i].transform.localPosition = startP + new Vector3(offsetBetweenDices.x, 0, 0) * xIndex;
            Transform dice = GetDiceFromGroup(_dices[i]);
            dice.localEulerAngles = startVals[i];
            TextMesh label = _dices[i].GetComponentInChildren<TextMesh>();
            label.text = startVals[i] + "\n" + endVal;
        }
    }

    void StartTweening()
    {
        if (_dices == null) {
            Debug.Log("Nothing to tween, Setup something first");
            return;
        }

        Vector3[] startVals;
        Vector3 endVal;
        switch (_currRotateMode) {
        case RotateMode.FastBeyond360:
            startVals = beyond360StartValues;
            endVal = beyond360EndValue;
            break;
        default:
            startVals = fastStartValues;
            endVal = fastEndValue;
            break;
        }
        for (int i = 0; i < _dices.Length; ++i) {
            Transform dice = GetDiceFromGroup(_dices[i]);
            TextMesh label = _dices[i].GetComponentInChildren<TextMesh>();
            Vector3 startVal = startVals[i];
            Vector3 actualEndVal = endVal;
            Tween t;
            switch (tweenType) {
            case TweenType.FromCurrent:
                actualEndVal = startVal;
                t = dice.DOLocalRotate(endVal, tweenDuration, _currRotateMode).From();
                break;
            case TweenType.FromDirect:
                actualEndVal = startVal;
                t = dice.DOLocalRotate(startVal, tweenDuration, _currRotateMode).From(endVal);
                break;
            default:
                t = dice.DOLocalRotate(endVal, tweenDuration, _currRotateMode);
                break;
            }
            t.OnUpdate(() => {
                label.text = dice.eulerAngles + "\n" + actualEndVal;
            });
        }
    }

    #endregion

    #region Utils

    Transform GetDiceFromGroup(GameObject diceGroup)
    {
        Transform[] ts = diceGroup.GetComponentsInChildren<Transform>();
        for (int i = 0; i < ts.Length; ++i) {
            if (ts[i].name == "Dice") return ts[i];
        }
        return null;
    }

    #endregion

    #region UI Buttons

    public void SetupForFast()
    {
        SetupFor(RotateMode.Fast);
    }

    public void SetupForBeyond360()
    {
        SetupFor(RotateMode.FastBeyond360);
    }

    public void Tween()
    {
        StartTweening();
    }

    #endregion
}