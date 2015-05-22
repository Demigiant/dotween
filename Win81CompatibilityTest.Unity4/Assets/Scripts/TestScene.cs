using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

public class TestScene : MonoBehaviour
{
	public Text logText;
	public Transform target;
	public TextMesh text;

	string log;
	string logPrefix = "\n➨ ";
	string successStr = "<color=#00FF00>SUCCESS</color>";
	string failStr = "<color=#FF0000>SUCCESS</color>";

	SampleFloatClass sampleFloatClass;
	SampleVector3Class sampleV3Class;
	ISampleClass isampleClass;
	// Vector3Plugin dotweenV3Class;
	ITweenPlugin idotweenClass;
	string testId;

	int intToTween;

	IEnumerator Start()
	{
		FloatTest();
		log += "\n";
		Vector3Test();
		// log += "\n";
		// DOTweenVector3Test();

		log += "\n\n<color=#00ff00>FIRST TEST ENDED</color>";
		log += "\n\n<color=#00ff00>NOW WAITING 1 SECOND...</color>";
		logText.text = log;
		Debug.Log(StripTags(log));

		yield return new WaitForSeconds(1);

		log += "\n\n<color=#00ff00>SPAWNING A MOVE TWEEN IN THE NEXT FRAME...</color>";
		logText.text = log;
		Debug.Log("SPAWNING A MOVE TWEEN IN THE NEXT FRAME...");

		yield return null;

		target.DOMove(new Vector3(3, 0, 0), 2);
		// DOTween.To(()=>intToTween, x=> intToTween = x, 100, 4).OnUpdate(()=> text.text = intToTween.ToString());
	}

	// void DOTweenVector3Test()
	// {
	// 	testId = "DOVector3 Class AsCast:";
	// 	try {
	// 		dotweenV3Class = new Vector3Plugin();
	// 		log += string.Format("{0}{1} " + ((dotweenV3Class as ABSTweenPlugin<Vector3,Vector3,VectorOptions>) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
	// 	} catch (Exception e) {
	// 		log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
	// 	}

	// 	testId = "DOVector3 Class AsCast (object):";
	// 	try {
	// 		dotweenV3Class = new Vector3Plugin();
	// 		log += string.Format("{0}{1} " + (((object)dotweenV3Class as ABSTweenPlugin<Vector3,Vector3,VectorOptions>) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
	// 	} catch (Exception e) {
	// 		log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
	// 	}

	// 	testId = "DOVector3 Class PrefCast:";
	// 	try {
	// 		dotweenV3Class = new Vector3Plugin();
	// 		log += string.Format("{0}{1} " + ((ABSTweenPlugin<Vector3,Vector3,VectorOptions>)dotweenV3Class == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
	// 	} catch (Exception e) {
	// 		log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
	// 	}

	// 	testId = "DOVector3 Class PrefCast (object):";
	// 	try {
	// 		dotweenV3Class = new Vector3Plugin();
	// 		log += string.Format("{0}{1} " + ((ABSTweenPlugin<Vector3,Vector3,VectorOptions>)((object)dotweenV3Class) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
	// 	} catch (Exception e) {
	// 		log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
	// 	}

	// 	testId = "DOVector3 Interface AsCast:";
	// 	try {
	// 		idotweenClass = new Vector3Plugin();
	// 		log += string.Format("{0}{1} " + ((idotweenClass as ABSTweenPlugin<Vector3,Vector3,VectorOptions>) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
	// 	} catch (Exception e) {
	// 		log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
	// 	}

	// 	testId = "DOVector3 Interface AsCast (object):";
	// 	try {
	// 		idotweenClass = new Vector3Plugin();
	// 		log += string.Format("{0}{1} " + (((object)idotweenClass as ABSTweenPlugin<Vector3,Vector3,VectorOptions>) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
	// 	} catch (Exception e) {
	// 		log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
	// 	}

	// 	testId = "DOVector3 Interface PrefCast:";
	// 	try {
	// 		idotweenClass = new Vector3Plugin();
	// 		log += string.Format("{0}{1} " + ((ABSTweenPlugin<Vector3,Vector3,VectorOptions>)idotweenClass == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
	// 	} catch (Exception e) {
	// 		log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
	// 	}

	// 	testId = "DOVector3 Interface PrefCast (object):";
	// 	try {
	// 		idotweenClass = new Vector3Plugin();
	// 		log += string.Format("{0}{1} " + ((ABSTweenPlugin<Vector3,Vector3,VectorOptions>)((object)idotweenClass) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
	// 	} catch (Exception e) {
	// 		log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
	// 	}
	// }

	void Vector3Test()
	{
		testId = "Vector3 Class AsCast:";
		try {
			sampleV3Class = new SampleVector3Class();
			log += string.Format("{0}{1} " + ((sampleV3Class as AbstractSampleClass<Vector3>) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Vector3 Class AsCast (object):";
		try {
			sampleV3Class = new SampleVector3Class();
			log += string.Format("{0}{1} " + (((object)sampleV3Class as AbstractSampleClass<Vector3>) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Vector3 Class PrefCast:";
		try {
			sampleV3Class = new SampleVector3Class();
			log += string.Format("{0}{1} " + ((AbstractSampleClass<Vector3>)sampleV3Class == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Vector3 Class PrefCast (object):";
		try {
			sampleV3Class = new SampleVector3Class();
			log += string.Format("{0}{1} " + ((AbstractSampleClass<Vector3>)((object)sampleV3Class) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Vector3 Interface AsCast:";
		try {
			isampleClass = new SampleVector3Class();
			log += string.Format("{0}{1} " + ((isampleClass as AbstractSampleClass<Vector3>) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Vector3 Interface AsCast (object):";
		try {
			isampleClass = new SampleVector3Class();
			log += string.Format("{0}{1} " + (((object)isampleClass as AbstractSampleClass<Vector3>) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Vector3 Interface PrefCast:";
		try {
			isampleClass = new SampleVector3Class();
			log += string.Format("{0}{1} " + ((AbstractSampleClass<Vector3>)isampleClass == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Vector3 Interface PrefCast (object):";
		try {
			isampleClass = new SampleVector3Class();
			log += string.Format("{0}{1} " + ((AbstractSampleClass<Vector3>)((object)isampleClass) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}
	}

	void FloatTest()
	{
		testId = "Float Class AsCast:";
		try {
			sampleFloatClass = new SampleFloatClass();
			log += string.Format("{0}{1} " + ((sampleFloatClass as AbstractSampleClass<float>) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Float Class AsCast (object):";
		try {
			sampleFloatClass = new SampleFloatClass();
			log += string.Format("{0}{1} " + (((object)sampleFloatClass as AbstractSampleClass<float>) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Float Class PrefCast:";
		try {
			sampleFloatClass = new SampleFloatClass();
			log += string.Format("{0}{1} " + ((AbstractSampleClass<float>)sampleFloatClass == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Float Class PrefCast (object):";
		try {
			sampleFloatClass = new SampleFloatClass();
			log += string.Format("{0}{1} " + ((AbstractSampleClass<float>)((object)sampleFloatClass) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Float Interface AsCast:";
		try {
			isampleClass = new SampleFloatClass();
			log += string.Format("{0}{1} " + ((isampleClass as AbstractSampleClass<float>) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Float Interface AsCast (object):";
		try {
			isampleClass = new SampleFloatClass();
			log += string.Format("{0}{1} " + (((object)isampleClass as AbstractSampleClass<float>) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Float Interface PrefCast:";
		try {
			isampleClass = new SampleFloatClass();
			log += string.Format("{0}{1} " + ((AbstractSampleClass<float>)isampleClass == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}

		testId = "Float Interface PrefCast (object):";
		try {
			isampleClass = new SampleFloatClass();
			log += string.Format("{0}{1} " + ((AbstractSampleClass<float>)((object)isampleClass) == null ? "{2}" : "{3}"), logPrefix, testId, failStr, successStr);
		} catch (Exception e) {
			log += string.Format("{0}{1} <color=#ff0000>error > " + e.Message + "</color>", logPrefix, testId);
		}
	}

	string StripTags(string s)
	{
		return Regex.Replace(s, @"<[^>]*>", "");
	}
}