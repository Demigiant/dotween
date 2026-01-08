using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RotationModes : BrainBase
{
	public Transform[] targets;

	void Start()
	{
		DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
			.Append(targets[0].DORotate(new Vector3(0, 450, 0), 1, RotateMode.FastBeyond360))
			.Append(targets[0].DORotate(new Vector3(450, 0, 0), 1, RotateMode.FastBeyond360));

		DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
			.Append(targets[1].DORotate(new Vector3(0, 450, 0), 1, RotateMode.Fast))
			.Append(targets[1].DORotate(new Vector3(450, 0, 0), 1, RotateMode.Fast));

		DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
			.Append(targets[2].DORotate(new Vector3(0, 90, 0), 1, RotateMode.LocalAxisAdd))
			.Append(targets[2].DORotate(new Vector3(90, 0, 0), 1, RotateMode.LocalAxisAdd));

		DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
			.Append(targets[3].DORotate(new Vector3(0, 90, 0), 1, RotateMode.WorldAxisAdd))
			.Append(targets[3].DORotate(new Vector3(90, 0, 0), 1, RotateMode.WorldAxisAdd));

		// FROM versions
		DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
			.Append(targets[4].DORotate(new Vector3(0, 450, 0), 1, RotateMode.FastBeyond360).From())
			.Append(targets[4].DORotate(new Vector3(450, 0, 0), 1, RotateMode.FastBeyond360));

		DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
			.Append(targets[5].DORotate(new Vector3(0, 450, 0), 1, RotateMode.Fast).From())
			.Append(targets[5].DORotate(new Vector3(450, 0, 0), 1, RotateMode.Fast));

		DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
			.Append(targets[6].DORotate(new Vector3(0, 90, 0), 1, RotateMode.LocalAxisAdd).From())
			.Append(targets[6].DORotate(new Vector3(90, 0, 0), 1, RotateMode.LocalAxisAdd));

		DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
			.Append(targets[7].DORotate(new Vector3(0, 90, 0), 1, RotateMode.WorldAxisAdd).From())
			.Append(targets[7].DORotate(new Vector3(90, 0, 0), 1, RotateMode.WorldAxisAdd));


		// DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
		// 	.Append(targets[4].DOLocalRotate(new Vector3(0, 450, 0), 1, RotateMode.FastBeyond360))
		// 	.Append(targets[4].DOLocalRotate(new Vector3(450, 0, 0), 1, RotateMode.FastBeyond360));

		// DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
		// 	.Append(targets[5].DOLocalRotate(new Vector3(0, 450, 0), 1, RotateMode.Fast))
		// 	.Append(targets[5].DOLocalRotate(new Vector3(450, 0, 0), 1, RotateMode.Fast));

		// DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
		// 	.Append(targets[6].DOLocalRotate(new Vector3(0, 90, 0), 1, RotateMode.LocalAxisAdd))
		// 	.Append(targets[6].DOLocalRotate(new Vector3(90, 0, 0), 1, RotateMode.LocalAxisAdd));

		// DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
		// 	.Append(targets[7].DOLocalRotate(new Vector3(0, 90, 0), 1, RotateMode.WorldAxisAdd))
		// 	.Append(targets[7].DOLocalRotate(new Vector3(90, 0, 0), 1, RotateMode.WorldAxisAdd));


		DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
			.Append(targets[8].DORotate(new Vector3(0, 90, 0), 1, RotateMode.Fast))
			.Append(targets[8].DORotate(new Vector3(90, 0, 0), 1, RotateMode.Fast));

		DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
			.Append(targets[9].DOLocalRotate(new Vector3(0, 90, 0), 1, RotateMode.Fast))
			.Append(targets[9].DOLocalRotate(new Vector3(90, 0, 0), 1, RotateMode.Fast));
	}
}