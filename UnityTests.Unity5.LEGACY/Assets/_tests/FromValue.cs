using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FromValue : BrainBase
{
    public GameObject cube;

    IEnumerator Start()
    {
        Tween t = cube.transform.DOMoveX(2, 2);
        yield return t.WaitForCompletion();

        t = cube.transform.DOMoveX(3, 2).From(10);
        cube.GetComponent<Renderer>().material.DOFade(1, 2).From(0.5f);
        yield return t.WaitForCompletion();
    }
}