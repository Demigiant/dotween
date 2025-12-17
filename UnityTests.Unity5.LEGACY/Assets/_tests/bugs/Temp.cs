using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	[Range(0, 10)]
	public float interval = 0.33f;
	public bool timeIndependent = false;

	void Start(){
		DOSequence();
	}

	protected override void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space)) DOSequence();
		
		base.Update();
	}
	
	void DOSequence(){
		Debug.Log("Max deltaTime: " + Time.maximumDeltaTime + ", deltaTime: " + Time.deltaTime + ", frame:" + Time.frameCount);
		Sequence sequence = DOTween.Sequence();
		if (timeIndependent) sequence.SetUpdate(true);
		sequence.AppendCallback(()=> {
			Debug.Log(">>> Callback1, deltaTime: " + Time.deltaTime + ", frame:" + Time.frameCount + ", elapsed: " + sequence.Elapsed());
			sequence.Pause();
		});
		sequence.AppendInterval(interval);
		sequence.AppendCallback(() => { Debug.Log(">>> Callback2" + ", frame:" + Time.frameCount + ", elapsed: " + sequence.Elapsed()); });
		sequence.OnUpdate(() => Debug.Log(">>> Updated" + ", frame:" + Time.frameCount + ", elapsed: " + sequence.Elapsed()));
		sequence.OnStart(() => Debug.Log(">>> Start" + ", frame:" + Time.frameCount + ", elapsed: " + sequence.Elapsed()));
		int result = 0;
		//for loop's time cost is about 1.2 seconds(on tester pc)
		float time = Time.realtimeSinceStartup;
		Debug.Log("   FOR LOOP");
		for (int i = 0; i < 100000000; i++)
		{
			result = (result + 1) / 2;
		}
		Debug.Log("   FOR LOOP COMPLETED in " + (Time.realtimeSinceStartup - time));
	}
}