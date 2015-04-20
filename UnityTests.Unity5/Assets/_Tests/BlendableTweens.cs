using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BlendableTweens : BrainBase
{
	public bool addBlendable = true;
	public bool from;
	public bool fromBlendable;
	public bool repeatBlendable = true;
	public RotateMode rotationMode;
	public Ease ease = Ease.OutQuad;
	public Color toBlendableColor0, toBlendableColor1;
	public Transform nonBlendableT;
	public Transform[] targets;
	public Renderer[] renderers;
	public SpriteRenderer[] sprites;

	Vector3[] startPositions;

	IEnumerator Start()
	{
		startPositions = new Vector3[2];
		startPositions[0] = targets[0].position;

		yield return new WaitForSeconds(0.6f);

		Vector3 to;
		Color toCol;
		float duration;
		int loops;

		// Move
		duration = 3;
		to = new Vector3(3, 3, 0);
		startPositions[1] = targets[0].position + to;
	    if (from) targets[0].DOBlendableMoveBy(to, duration).From().SetEase(ease).SetAutoKill(false).Pause();
	    else targets[0].DOBlendableMoveBy(to, duration).SetEase(ease).SetAutoKill(false).Pause();
        if (addBlendable) {
        	to = new Vector3(-3, 0, 0);
        	duration = repeatBlendable ? 1 : 3;
        	loops = repeatBlendable ? 3 : 1;
        	if (fromBlendable) targets[0].DOBlendableMoveBy(to, duration).From().SetEase(ease).SetLoops(loops, LoopType.Yoyo).SetAutoKill(false).Pause();
        	else targets[0].DOBlendableMoveBy(to, duration).SetEase(ease).SetLoops(loops, LoopType.Yoyo).SetAutoKill(false).Pause();
        }

        // Scale
        duration = 3;
        to = new Vector3(0, 1, 0);
	    if (from) targets[1].DOBlendableScaleBy(to, duration).From().SetEase(ease).SetAutoKill(false).Pause();
	    else targets[1].DOBlendableScaleBy(to, duration).SetEase(ease).SetAutoKill(false).Pause();
        if (addBlendable) {
        	to = new Vector3(1, 0, 0);
        	duration = repeatBlendable ? 1 : 3;
        	loops = repeatBlendable ? 3 : 1;
        	if (fromBlendable) targets[1].DOBlendableScaleBy(to, duration).From().SetEase(ease).SetLoops(loops, LoopType.Yoyo).SetAutoKill(false).Pause();
        	else targets[1].DOBlendableScaleBy(to, duration).SetEase(ease).SetLoops(loops, LoopType.Yoyo).SetAutoKill(false).Pause();
        }

        // Rotate
        duration = 3;
        to = new Vector3(0, 90, 0);
	    if (from) targets[2].DOBlendableRotateBy(to, duration, rotationMode).From().SetEase(ease).SetAutoKill(false).Pause();
	    else targets[2].DOBlendableRotateBy(to, duration, rotationMode).SetEase(ease).SetAutoKill(false).Pause();
        if (addBlendable) {
        	to = new Vector3(90, 0, 0);
        	duration = repeatBlendable ? 1 : 3;
        	loops = repeatBlendable ? 3 : 1;
        	if (fromBlendable) targets[2].DOBlendableRotateBy(to, duration, rotationMode).From().SetEase(ease).SetLoops(loops, LoopType.Yoyo).SetAutoKill(false).Pause();
        	else targets[2].DOBlendableRotateBy(to, duration, rotationMode).SetEase(ease).SetLoops(loops, LoopType.Yoyo).SetAutoKill(false).Pause();
        }
        // Non blendable for comparisons
        duration = 3;
        to = new Vector3(0, 90, 0);
	    if (from) nonBlendableT.DORotate(to, duration, rotationMode).From(true).SetEase(ease).SetAutoKill(false).Pause();
	    else nonBlendableT.DORotate(to, duration, rotationMode).SetEase(ease).SetAutoKill(false).Pause();

	    // Color
        duration = 3;
        toCol = toBlendableColor0;
	    if (from) renderers[0].material.DOBlendableColor(toCol, duration).From().SetEase(ease).SetAutoKill(false).Pause();
	    else renderers[0].material.DOBlendableColor(toCol, duration).SetEase(ease).SetAutoKill(false).Pause();
        if (addBlendable) {
        	toCol = toBlendableColor1;
        	duration = repeatBlendable ? 1 : 3;
        	loops = repeatBlendable ? 3 : 1;
        	if (fromBlendable) renderers[0].material.DOBlendableColor(toCol, duration).From().SetEase(ease).SetLoops(loops, LoopType.Yoyo).SetAutoKill(false).Pause();
        	else renderers[0].material.DOBlendableColor(toCol, duration).SetEase(ease).SetLoops(loops, LoopType.Yoyo).SetAutoKill(false).Pause();
        }

        duration = 3;
        toCol = toBlendableColor0;
	    if (from) sprites[0].DOBlendableColor(toCol, duration).From().SetEase(ease).SetAutoKill(false).Pause();
	    else sprites[0].DOBlendableColor(toCol, duration).SetEase(ease).SetAutoKill(false).Pause();
        if (addBlendable) {
        	toCol = toBlendableColor1;
        	duration = repeatBlendable ? 1 : 3;
        	loops = repeatBlendable ? 3 : 1;
        	if (fromBlendable) sprites[0].DOBlendableColor(toCol, duration).From().SetEase(ease).SetLoops(loops, LoopType.Yoyo).SetAutoKill(false).Pause();
        	else sprites[0].DOBlendableColor(toCol, duration).SetEase(ease).SetLoops(loops, LoopType.Yoyo).SetAutoKill(false).Pause();
        }
	}

	void OnGUI()
	{
		if (GUILayout.Button("Toggle Pause")) DOTween.TogglePauseAll();
		if (GUILayout.Button("Restart")) DOTween.RestartAll();
		if (GUILayout.Button("Rewind")) DOTween.RewindAll();
		if (GUILayout.Button("Complete")) DOTween.CompleteAll();
		if (GUILayout.Button("Flip")) DOTween.FlipAll();
	}

	void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;

		foreach (Vector3 pos in startPositions) Gizmos.DrawSphere(pos, 0.2f);
	}
}