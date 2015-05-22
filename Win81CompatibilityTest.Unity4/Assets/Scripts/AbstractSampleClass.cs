using UnityEngine;
using System.Collections;

public abstract class AbstractSampleClass<T> : ISampleClass
{
	public abstract void SampleMethod(T value);
}