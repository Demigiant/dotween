using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class TestScene : MonoBehaviour
{
	public Text logText;

	string log;
	string logPrefix = "➨ Test ";

	void Start()
	{
		SampleFloatClass sampleFloatClass;
		ISampleClass isampleClass;
		string testId;

		// Test A
		testId = "A";
		try {
			sampleFloatClass = new SampleFloatClass();
			log += string.Format("{0}{1} " + ((sampleFloatClass as AbstractSampleClass<float>) == null ? "failed" : "success"), logPrefix, testId);
		} catch (Exception e) {
			log += string.Format("<color=#ff0000>{0}{1} error > " + e.Message + "</color>", logPrefix, testId);
		}

		// Test A2
		testId = "A2";
		try {
			isampleClass = new SampleFloatClass();
			log += string.Format("\n{0}{1} " + ((isampleClass as AbstractSampleClass<float>) == null ? "failed" : "success"), logPrefix, testId);
		} catch (Exception e) {
			log += string.Format("\n<color=#ff0000>{0}{1} error > " + e.Message + "</color>", logPrefix, testId);
		}

		// Test B
		testId = "B";
		try {
			sampleFloatClass = new SampleFloatClass();
			log += string.Format("\n{0}{1} " + ((AbstractSampleClass<float>)sampleFloatClass == null ? "failed" : "success"), logPrefix, testId);
		} catch (Exception e) {
			log += string.Format("\n<color=#ff0000>{0}{1} error > " + e.Message + "</color>", logPrefix, testId);
		}

		// Test B2
		testId = "B2";
		try {
			isampleClass = new SampleFloatClass();
			log += string.Format("\n{0}{1} " + ((AbstractSampleClass<float>)isampleClass == null ? "failed" : "success"), logPrefix, testId);
		} catch (Exception e) {
			log += string.Format("\n<color=#ff0000>{0}{1} error > " + e.Message + "</color>", logPrefix, testId);
		}


		log += "\n\n<color=#00ff00>TEST ENDED</color>";
		logText.text = log;
		Debug.Log(log);
	}
}