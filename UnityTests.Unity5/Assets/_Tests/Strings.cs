using DG.Tweening;
using UnityEngine;

public class Strings : BrainBase
{
	public GUIText[] txts;
	string stringToTween0 = "Short text",
		stringToTween1 = "Long text to show how it gets completely replaced",
		stringToTween2 = "Relative text... ",
		stringToTween3 = "Scramble short text",
		stringToTween4 = "Scramble long text to show how it gets completely replaced",
		stringToTween5 = "Scramble relative text... ";
	string[] strings;

	void Start()
	{
		// String
		DOTween.To(()=> stringToTween0, x=> stringToTween0 = x, "Hello I'm a new string!", 1.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).Pause();
		DOTween.To(()=> stringToTween1, x=> stringToTween1 = x, "Hello I'm a new string!", 1.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).Pause();
		DOTween.To(()=> stringToTween2, x=> stringToTween2 = x, "Hello I'm a new string!", 1.5f).SetRelative().SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).Pause();
		// String scramble
		DOTween.To(()=> stringToTween3, x=> stringToTween3 = x, "Hello I'm a new string!", 1.5f).SetOptions(true).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).Pause();
		DOTween.To(()=> stringToTween4, x=> stringToTween4 = x, "Hello I'm a new string!", 1.5f).SetOptions(true).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).Pause();
		DOTween.To(()=> stringToTween5, x=> stringToTween5 = x, "Hello I'm a new string!", 1.5f).SetOptions(true).SetRelative().SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).Pause();

		// Additional string
		strings = new string[500];
		for (int i = 0; i < strings.Length; ++i) {
			int index = i;
			strings[i] = "Some String";
			DOTween.To(()=> strings[index], x=> strings[index] = x, "Modified string", 1.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).Pause();
		}
	}

	void LateUpdate()
	{
		txts[0].text = "<color=orange>String 0:</color> " + stringToTween0;
		txts[1].text = "<color=orange>String 1:</color> " + stringToTween1;
		txts[2].text = "<color=orange>String 2 (relative tween):</color> " + stringToTween2;
		txts[3].text = "<color=orange>String 3 (scramble):</color> " + stringToTween3;
		txts[4].text = "<color=orange>String 4 (scramble):</color> " + stringToTween4;
		txts[5].text = "<color=orange>String 5 (scramble relative):</color> " + stringToTween5;
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.Space(100);

		if (GUILayout.Button("Toggle Pause")) {
			DOTween.TogglePauseAll();
		}

		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}