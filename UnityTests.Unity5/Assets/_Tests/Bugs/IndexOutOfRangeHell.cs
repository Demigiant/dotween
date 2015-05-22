using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class IndexOutOfRangeHell : BrainBase
{
	public GameObject prefab;
	public Transform container;

	List<Transform> ts;

	IEnumerator Start()
	{
		ts = new List<Transform>();
		while (true) {
			if (ts.Count < 1000) {
				Debug.Log(Time.frameCount + " Creating tweens");
				SpawnObjectsAndTweens(1000);
			}
			if (Time.frameCount % 100 == 0) {
				Debug.Log("<color=#00FF00>Clearing DOTween</color>");
				foreach (Transform t in ts) Destroy(t.gameObject);
				ts.Clear();
				DOTween.Clear(true);
			}
			yield return null;
		}
	}

	void SpawnObjectsAndTweens(int tot)
	{
		for (int i = 0; i < tot; ++i) {
			GameObject go = Instantiate(prefab) as GameObject;
			go.transform.position = RandomV3();
			go.transform.parent = container;
			go.GetComponent<Renderer>().enabled = false;
			Transform t = go.transform;
			ts.Add(t);
			if (i % 2 == 0) {
				// Tweener
				t.DOMove(RandomV3(), Random.Range(0.1f, 1f)).OnComplete(()=> {
					ts.Remove(t);
					Destroy(t.gameObject);
				});
			} else {
				// Sequence
				DOTween.Sequence().Append(t.DOMove(RandomV3(), Random.Range(0.1f, 1f))).OnComplete(()=> {
					ts.Remove(t);
					Destroy(t.gameObject);
				});
			}
		}
	}

	Vector3 RandomV3()
	{
		const float range = 7;
		return new Vector3(Random.Range(-range,range), Random.Range(-range,range), Random.Range(-range,range));
	}
}