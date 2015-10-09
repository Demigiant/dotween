using DG.Tweening;
using DG.Tweening.Plugins;
using UnityEngine;
using UnityEngine.UI;

public class CustomPluginExampleBrain : MonoBehaviour
{
    public Text txtCustomRange ; // Used to show the custom range tween results

	CustomRange customRange = new CustomRange(0, 10);

    // Store the plugin so you won't have to instantiate it every time you use it
    // (you can pass the same plugin instance to each tween, since they just do calculations and don't store data)
    static CustomRangePlugin customRangePlugin = new CustomRangePlugin();

	void Start()
	{
        // The difference with the regular generic way is simply
        // that you have to pass the plugin to use as an additional first parameter
	    DOTween.To(customRangePlugin, () => customRange, x => customRange = x, new CustomRange(20, 100), 4);
	}

    void Update()
    {
        txtCustomRange.text = customRange.min + "\n" + customRange.max;
    }
}