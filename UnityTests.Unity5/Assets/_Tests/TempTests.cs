using System;
using DG.Tweening;
using UnityEngine;

public class TempTests : BrainBase
{
    public static event Action OnTest;
    public static void Dispatch_OnTest() { if (OnTest != null) OnTest(); }

    public Transform target;

    void OnDestroy()
    {
        // Kill all Sequences/Tweens this MonoBehaviour generated
        // by killing all Sequences with this ID
        DOTween.Kill("CubeKicks");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) KickCube(target, 0.8f, true);
    }

    void KickCube(Transform cube, float duration, bool pushLeft)
    {
        // Let's create the Sequence while giving it an ID so we can kill
        // all the Sequences this Monobehaviour created when it's destroyed (see OnDestroy).
        // Also, beware the parenthesis. You were chaining the SetEase and SetRelative to the Sequence
        // instead of to the Tween inside the Sequence (I fixed that)
        Sequence s = DOTween.Sequence().SetId("CubeKicks")
            // Z is left then right (or viceversa, see pushLeft parameter) so we're going to set it as a half-duration Yoyo loop
            .Append(cube.DOMoveX(2f * (pushLeft ? -1 : 1), duration / 2f).SetRelative().SetEase(Ease.OutCubic).SetLoops(2, LoopType.Yoyo))
            // Y is up then down so we're going to set it as a half-duration Yoyo loop
            // but with a more dramatic ease than the X one
            .Insert(0, cube.DOMoveY(4f, duration / 2f).SetRelative().SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo))
            // Z which is very slow then fast so an EaseQuint
            .Insert(0, cube.DOMoveZ(40f, duration).SetRelative().SetEase(Ease.InQuint))
            // And the scale to force the cube to disappear
            .Insert(0, cube.DOScale(0.001f, duration).SetEase(Ease.InExpo))
            .OnComplete(() => Debug.Log("Your code here :P"));
    }
}