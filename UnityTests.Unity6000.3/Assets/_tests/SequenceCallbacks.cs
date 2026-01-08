using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SequenceCallbacks : BrainBase
{
	public Transform[] targets;

	Sequence main;

    void OnGUI()
    {
        DGUtils.BeginGUI();

        if (GUILayout.Button("TogglePause")) DOTween.TogglePauseAll();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Rewind")) DOTween.RewindAll();
        if (GUILayout.Button("Restart")) DOTween.RestartAll();
        if (GUILayout.Button("Complete")) DOTween.CompleteAll();
        if (GUILayout.Button("Flip")) DOTween.FlipAll();
        if (GUILayout.Button("Goto 0.5")) DOTween.GotoAll(0.5f);
        if (GUILayout.Button("Goto 1")) DOTween.GotoAll(1);
        if (GUILayout.Button("Goto 1.5")) DOTween.GotoAll(1.5f);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Sequence > LoopType.Restart")) NewSequence(LoopType.Restart, false);
        if (GUILayout.Button("Sequence > LoopType.Restart > Flipped")) NewSequence(LoopType.Restart, true);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Sequence > LoopType.Yoyo")) NewSequence(LoopType.Yoyo, false);
        if (GUILayout.Button("Sequence > LoopType.Yoyo > Flipped")) NewSequence(LoopType.Yoyo, true);
        GUILayout.EndHorizontal();

        DGUtils.EndGUI();
    }

    void NewSequence(LoopType loopType, bool flip)
    {
    	main.Rewind();
    	main.Kill();

    	Sequence innerS0 = DOTween.Sequence()
            .SetId("INNER")
            // .SetLoops(3, loopType)
            .OnStepComplete(()=>Debug.Log("INNER Step Complete"));
            // .Append(targets[0].DOMoveX(3, 1).SetEase(Ease.Linear));
        // innerS0.InsertCallback(0.25f, ()=> Callback("INNER"));

        Sequence innerS1 = DOTween.Sequence()
            .SetId("INNER INNER")
            // .SetLoops(3, loopType)
            .OnStepComplete(()=> Debug.Log("INNER INNER Step Complete"));
		innerS1.Append(targets[0].DOMoveX(3, 1).SetEase(Ease.Linear));
        innerS1.InsertCallback(0.25f, ()=> Callback("INNER INNER"));
        innerS0.Append(innerS1);
        innerS0.InsertCallback(0.25f, ()=> Callback("INNER"));

        main = DOTween.Sequence()
            .SetId("MAIN")
            .SetLoops(3, loopType)
            .SetAutoKill(false)
            .OnStepComplete(()=> Debug.Log("MAIN Step Complete"));
            // .Append(targets[0].DOMoveX(3, 1).SetEase(Ease.Linear));
        main.Append(innerS0);
        main.InsertCallback(0.25f, ()=> Callback("MAIN"));

        if (flip) {
        	main.Complete();
        	main.Flip();
        	main.PlayBackwards();
        }
    }

    void Callback(string id)
    {
    	string prefix = ">>>";
    	if (id == "INNER") prefix += ">>>";
    	if (id == "INNER INNER") prefix += ">>>>>>";
    	Debug.Log("<color=#92FF70>" + prefix + " " + id + " > Callback > " + main.Elapsed(true) + "</color>");
    }
}