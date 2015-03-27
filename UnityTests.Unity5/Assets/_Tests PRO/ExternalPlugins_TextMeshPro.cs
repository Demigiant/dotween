using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;

public class ExternalPlugins_TextMeshPro : BrainBase
{
	public TextMeshPro[] texts;

	const string ReplaceText = "This text is gonna appear with a tween, yabadabadoo!";

	void Start()
	{
		DOTween.Init();
	}

	void OnGUI()
	{
		GUILayout.Label("These tweens won't stack. Run one at a time");
		if (GUILayout.Button("Color To Green")) foreach (TextMeshPro t in texts) t.DOColor(Color.green, 1).SetEase(Ease.Linear);
		if (GUILayout.Button("Color To Red")) foreach (TextMeshPro t in texts) t.DOColor(Color.red, 1).SetEase(Ease.Linear);
		if (GUILayout.Button("Fade Out")) foreach (TextMeshPro t in texts) t.DOFade(0, 1).SetEase(Ease.Linear);
		if (GUILayout.Button("Fade In")) foreach (TextMeshPro t in texts) t.DOFade(1, 1).SetEase(Ease.Linear);
		if (GUILayout.Button("Font Resize Down")) foreach (TextMeshPro t in texts) t.DOFontSize(10, 1);
		if (GUILayout.Button("Font Resize Up")) foreach (TextMeshPro t in texts) t.DOFontSize(32, 1);
		if (GUILayout.Button("Trim Max Visible Characters")) foreach (TextMeshPro t in texts) t.DOMaxVisibleCharacters(22, 1);
		if (GUILayout.Button("Text Replace")) foreach (TextMeshPro t in texts) t.DOText(ReplaceText, 2).SetEase(Ease.Linear);
		if (GUILayout.Button("Text Replace W Scramble")) foreach (TextMeshPro t in texts) t.DOText(ReplaceText, 2, true).SetEase(Ease.Linear);
	}
}