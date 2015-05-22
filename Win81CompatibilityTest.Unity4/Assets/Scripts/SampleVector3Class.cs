using UnityEngine;
using System.Collections;

public class SampleVector3Class : AbstractSampleClass<Vector3>
{
	public override void SampleMethod(Vector3 value)
	{
		Debug.Log("Sample method log > " + value);
	}
}