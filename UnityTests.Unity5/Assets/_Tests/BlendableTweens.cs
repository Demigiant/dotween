using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BlendableTweens : BrainBase
{
	public Transform[] targets;

	Vector3[] startPositions;

	IEnumerator Start()
	{
		startPositions = new Vector3[targets.Length];
		startPositions[0] = targets[0].position;
		startPositions[1] = targets[1].position + new Vector3(3, 3, 0);

		yield return new WaitForSeconds(0.6f);

	    targets[0].DOBlendableMoveBy(new Vector3(3, 3, 0), 3).SetAutoKill(false).Pause();
        targets[0].DOBlendableMoveBy(new Vector3(-3, 0, 0), 1f).SetLoops(3, LoopType.Yoyo).SetAutoKill(false).Pause();
        // Same as above but using From
        targets[1].DOBlendableMoveBy(new Vector3(3, 3, 0), 3).From().SetAutoKill(false).Pause();
        targets[1].DOBlendableMoveBy(new Vector3(-3, 0, 0), 1f).SetLoops(3, LoopType.Yoyo).SetAutoKill(false).Pause();
	}

	void OnGUI()
	{
		if (GUILayout.Button("Toggle Pause")) DOTween.TogglePauseAll();
		if (GUILayout.Button("Restart")) DOTween.RestartAll();
		if (GUILayout.Button("Flip")) DOTween.FlipAll();
	}

	void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;

		foreach (Vector3 pos in startPositions) Gizmos.DrawSphere(pos, 0.2f);
	}
}