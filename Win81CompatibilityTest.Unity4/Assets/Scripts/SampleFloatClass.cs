using UnityEngine;
using System.Collections;

public class SampleFloatClass : AbstractSampleClass<float>
{
	public override void SampleMethod(float value)
	{
		Debug.Log("Sample method log > " + value);
	}
}