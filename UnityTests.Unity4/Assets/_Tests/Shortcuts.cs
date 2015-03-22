using DG.Tweening;
using UnityEngine;

public class Shortcuts : MonoBehaviour
{
	public GameObject prefab;

	void Start()
	{
		////////////////////////////////////////////
		// Transform shortcuts /////////////////////

		// Position
		NewTransform().DOMove(RandomVector3(), 1).SetLoops(-1, LoopType.Yoyo);
		// X Position
		NewTransform().DOMoveX(Random.Range(-10f, 10f), 1).SetLoops(-1, LoopType.Yoyo);
		// Local position
		NewTransform().DOLocalMove(RandomVector3(), 1).SetLoops(-1, LoopType.Yoyo);
		// Rotation
		NewTransform().DORotate(RandomVector3(720), 1).SetLoops(-1, LoopType.Yoyo);
		// Local rotation
		NewTransform().DOLocalRotate(RandomVector3(720), 1).SetLoops(-1, LoopType.Yoyo);
		// Scale
		NewTransform().DOScale(RandomVector3(3), 1).SetLoops(-1, LoopType.Yoyo);
		// Color
		NewTransform().renderer.material.DOColor(Color.green, 1).SetLoops(-1, LoopType.Yoyo);
		// Alpha
		NewTransform().renderer.material.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
	}

	Transform NewTransform(bool randomPos = true)
	{
		Transform t = ((GameObject)Instantiate(prefab)).transform;
		if (randomPos) t.position = RandomVector3();
		return t;
	}

	Vector3 RandomVector3(float limit = 10f)
	{
		return new Vector3(Random.Range(-limit, limit), Random.Range(-limit, limit), Random.Range(-limit, limit));
	}
}