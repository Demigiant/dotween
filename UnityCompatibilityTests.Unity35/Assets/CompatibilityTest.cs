using DG.Tweening;
using UnityEngine;

public class CompatibilityTest : MonoBehaviour
{
	public Transform cubeCont;
	public Transform[] cubes;
	public GUITexture logo;

	Tween twSuccess;
	bool success;
	Color logoCol;

	void Start()
	{
		DOTween.Init(true);
		Color c = logoCol = logo.color;
		c.a = 0;
		logo.color = c;

		// Create sequence
		Sequence seq = DOTween.Sequence()
			.SetLoops(-1, LoopType.Restart)
			.OnStepComplete(Success);
		seq.Append(cubeCont.DORotate(new Vector3(0, 720, 360), 2.25f).SetRelative().SetEase(Ease.Linear));
		foreach (Transform trans in cubes) {
			Transform t = trans;
			seq.Insert(0, t.DOScale(Vector3.one * 0.5f, 1f));
			seq.Insert(0, t.DOLocalMove(t.position * 8, 1f).SetEase(Ease.InQuint));
			seq.Insert(1, t.DOScale(Vector3.one * 0.5f, 1f));
			seq.Insert(1, t.DOLocalMove(t.position, 1f).SetEase(Ease.OutQuint));
		}

		// Create success tween
		twSuccess = DOTween.To(()=> logo.color, x => logo.color = x, logoCol, 1.25f).Pause();
	}

	void Success()
	{
		if (success) return;

		success = true;
		twSuccess.Play();
	}
}