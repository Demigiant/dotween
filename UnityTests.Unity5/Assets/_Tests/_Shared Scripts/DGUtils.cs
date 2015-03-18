using DG.Tweening;
using UnityEngine;

public static class DGUtils
{
	static float sliderPos;

	public static void Log(object o) {
		Debug.Log(Time.frameCount + "/" + Time.realtimeSinceStartup + " : " + o);
	}

	public static void BeginGUI()
	{
		GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("RELOAD SCENE")) Application.LoadLevel(Application.loadedLevel);
		if (GUILayout.Button("VALIDATE TWEENS")) Debug.Log("Invalid tweens found: " + DOTween.Validate());
		GUILayout.EndHorizontal();
	}

	public static void EndGUI()
	{
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	public static void GUIScrubber(float duration) {
		GUIScrubber(duration, null);
	}
	public static void GUIScrubber(Tween controller) {
		GUIScrubber(-1, controller);
	}
	static void GUIScrubber(float duration, Tween controller)
	{
		if (controller == null) {
			float prevSliderPos = sliderPos;
			sliderPos = GUILayout.HorizontalSlider(sliderPos, 0.0f, duration);
			if (!Mathf.Approximately(sliderPos, prevSliderPos)) DOTween.GotoAll(sliderPos);
		} else {
			// Get slider ID to be used to check mouseDown behaviour
			int sliderId = GUIUtility.GetControlID(FocusType.Passive) + 1;
			DOTween.GotoAll(GUILayout.HorizontalSlider(controller.Elapsed(false), 0.0f, controller.Duration(false)), controller.IsPlaying());
			// Check mouse down on slider, and pause tweens accordingly.
			if (sliderId != 0 && Event.current.type == EventType.used) {
				if (GUIUtility.hotControl == sliderId) DOTween.PauseAll();
			}

			// DOTween.Goto(GUILayout.HorizontalSlider(controller.Elapsed(false), 0.0f, controller.Duration(false)), controller.IsPlaying());
		}
	}
}