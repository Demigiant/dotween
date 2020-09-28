using System;
using DG.Tweening;
using UnityEngine;

public class TempTests : BrainBase
{
    // public static event Action OnTest;
    // public static void Dispatch_OnTest() { if (OnTest != null) OnTest(); }
    //
    // public Transform target;
    //
    // void OnDestroy()
    // {
    //     // Kill all Sequences/Tweens this MonoBehaviour generated
    //     // by killing all Sequences with this ID
    //     DOTween.Kill("CubeKicks");
    // }
    //
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space)) KickCube(target, 0.8f, true);
    // }
    //
    // void KickCube(Transform cube, float duration, bool pushLeft)
    // {
    //     // Let's create the Sequence while giving it an ID so we can kill
    //     // all the Sequences this Monobehaviour created when it's destroyed (see OnDestroy).
    //     // Also, beware the parenthesis. You were chaining the SetEase and SetRelative to the Sequence
    //     // instead of to the Tween inside the Sequence (I fixed that)
    //     Sequence s = DOTween.Sequence().SetId("CubeKicks")
    //         // Z is left then right (or viceversa, see pushLeft parameter) so we're going to set it as a half-duration Yoyo loop
    //         .Append(cube.DOMoveX(2f * (pushLeft ? -1 : 1), duration / 2f).SetRelative().SetEase(Ease.OutCubic).SetLoops(2, LoopType.Yoyo))
    //         // Y is up then down so we're going to set it as a half-duration Yoyo loop
    //         // but with a more dramatic ease than the X one
    //         .Insert(0, cube.DOMoveY(4f, duration / 2f).SetRelative().SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo))
    //         // Z which is very slow then fast so an EaseQuint
    //         .Insert(0, cube.DOMoveZ(40f, duration).SetRelative().SetEase(Ease.InQuint))
    //         // And the scale to force the cube to disappear
    //         .Insert(0, cube.DOScale(0.001f, duration).SetEase(Ease.InExpo))
    //         .OnComplete(() => Debug.Log("Your code here :P"));
    // }

    public Transform target;
    // TweenCallback Event_1;
    // TweenCallback Event_2;
    TweenCallback TC;
    EvClass c;

    void Start()
    {
        c = new EvClass();
        c.Event_1 = TestingFunction;
        c.Event_1 += TestingFunction2;
        c.Event_2 = c.Event_1;
        c.Event_2 += TestingFunction3;
        Invoke("InvokeFunction",1.0f);

        Tween t = target.DOMoveX(2, 1.2f);
        Debug.Log("ASSIGN");
        t.onComplete += TweenFunc;
        TC = t.onComplete;
        // t.onComplete += TweenFunc2;
        TC += TweenFunc2;
        TC();
    }

    void TestingFunction3()
    {
        Debug.Log("TestingFunction 3 working");
    }


    public void InvokeFunction()
    {
        c.Event_2();
    }

    void TestingFunction2()
    {
        Debug.Log("TestingFunction 2 working");
    }

    void TestingFunction()
    {
        Debug.Log("TestingFunction working");
    }

    void TweenFunc()
    {
        Debug.Log("TWEEN FUNC 1");
    }

    void TweenFunc2()
    {
        Debug.Log("TWEEN FUNC 2");
    }

    class EvClass
    {
        public TweenCallback Event_1;
        public TweenCallback Event_2;
    }
}