using UnityEngine;
using DG.Tweening;

public class DOTweenAnimationSequence_RewindAndPlay : MonoBehaviour
{
	public DOTweenAnimation dotweenAnimation;

	void OnGUI()
	{
		if (GUILayout.Button("Rewind and Play")) {
			dotweenAnimation.DORewind();
			dotweenAnimation.DOPlay();
		}
	}
}