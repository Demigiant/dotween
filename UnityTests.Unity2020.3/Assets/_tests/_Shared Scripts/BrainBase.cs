using Demigiant.DemiTools.Debugging;
using DG.Tweening;
using UnityEngine;

public class BrainBase : MonoBehaviour 
{
	public bool forceFrameRate;
	public int forcedFrameRate = 10;
	// public bool recycleTweens = true;
	// public bool safeMode = false;
	// public LogBehaviour logBehaviour = LogBehaviour.Default;

	public static DeFPSCounter fpsGadget;

	protected virtual void Awake()
	{
		if (fpsGadget == null) {
			GameObject fpsGadgetGo = new GameObject("FPS");
			DontDestroyOnLoad(fpsGadgetGo);
			fpsGadget = fpsGadgetGo.AddComponent<DeFPSCounter>();
			if (forceFrameRate) fpsGadget.limitFrameRate = forcedFrameRate;
			fpsGadget.showMemory = true;
		}

		// DOTween.showUnityEditorReport = true;
		// DOTween.useSafeMode = safeMode;
		// DOTween.defaultRecyclable = recycleTweens;
		// DOTween.logBehaviour = logBehaviour;
	}
    
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) {
            Debug.Log("Reloading scene");
            Application.LoadLevel(Application.loadedLevelName);
        }
    }
}