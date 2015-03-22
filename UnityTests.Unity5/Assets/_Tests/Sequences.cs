using DG.Tweening;
using System.Text;
using UnityEngine;

public class Sequences : BrainBase
{
	public int loops = 10001;
	public LoopType loopType;
	public GameObject prefab;

	Sequence mainSequence;
	int stepCompleteMS, stepCompleteS1, stepCompleteS2, stepCompleteT1, stepCompleteT2, stepCompleteT3;
	int completeMS, completeS1, completeS2, completeT1, completeT2, completeT3;
	StringBuilder sb = new StringBuilder();

	void Start()
	{
		mainSequence = CreateSequence();
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Restart")) {
			ResetStepsCounters();
			mainSequence.Restart();
		}
		if (GUILayout.Button("Rewind")) {
			ResetStepsCounters();
			mainSequence.Rewind();
		}
		if (GUILayout.Button("Complete")) mainSequence.Complete();
		if (GUILayout.Button("Flip")) mainSequence.Flip();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("TogglePause")) mainSequence.TogglePause();
		if (GUILayout.Button("PlayForward")) mainSequence.PlayForward();
		if (GUILayout.Button("PlayBackwards")) mainSequence.PlayBackwards();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Kill All")) DOTween.KillAll();
		if (GUILayout.Button("Create MAIN Sequence")) mainSequence = CreateSequence();
		if (GUILayout.Button("Create FROM Sequence")) CreateFromSequence();
		if (GUILayout.Button("Create Tween")) CreateTween();
		GUILayout.EndHorizontal();

		GUILayout.Space(10);
		sb.Remove(0, sb.Length);
		sb.Append("IsPlaying: ").Append(mainSequence.IsPlaying());
		sb.Append("\nIsBackwards: ").Append(mainSequence.IsBackwards());
		sb.Append("\nElapsed: ").Append(mainSequence.Elapsed(false));
		sb.Append("\nFullElapsed: ").Append(mainSequence.Elapsed());
		sb.Append("\nCompletedLoops: ").Append(mainSequence.CompletedLoops());
		GUILayout.Label(sb.ToString());

		GUILayout.Space(10);
		sb.Remove(0, sb.Length);
		sb.Append("MAINSequence Steps/Complete: ").Append(stepCompleteMS).Append("/").Append(completeMS);
		sb.Append("\nSequence OUTER Steps/Complete: ").Append(stepCompleteS1).Append("/").Append(completeS1);
		sb.Append("\nSequence INNER Steps/Complete: ").Append(stepCompleteS2).Append("/").Append(completeS2);
		sb.Append("\nMove Steps/Complete: ").Append(stepCompleteT1).Append("/").Append(completeT1);
		sb.Append("\nRotation Steps/Complete: ").Append(stepCompleteT2).Append("/").Append(completeT2);
		sb.Append("\nColor Steps/Complete: ").Append(stepCompleteT3).Append("/").Append(completeT3);
		GUILayout.Label(sb.ToString());

		DGUtils.EndGUI();
	}

	Sequence CreateSequence()
	{
		Transform target = ((GameObject)Instantiate(prefab)).transform;
		Material mat = target.gameObject.GetComponent<Renderer>().material;

		Sequence seq = DOTween.Sequence()
			.SetId("Sequence INNER")
			.OnStart(()=> DGUtils.Log("Sequence INNER Start"))
			.OnStepComplete(()=> { stepCompleteS2++; DGUtils.Log("SEQUENCE INNER Step Complete"); })
			.OnComplete(()=> { completeS2++; });

		seq.AppendInterval(0.5f);
		seq.Append(
			target.DOMove(new Vector3(2, 2, 2), 1f).SetLoops(3, LoopType.Yoyo)
			.SetSpeedBased() // will not work since it's not allowed
			.SetId("Move")
			.OnStart(()=> DGUtils.Log("Move Start"))
			.OnStepComplete(()=> { stepCompleteT1++; DGUtils.Log("Move Step Complete"); })
			.OnComplete(()=> { completeT1++; })
		);
		seq.Append(
			target.DORotate(new Vector3(0, 225, 2), 1)
			.SetId("Rotate")
			.SetDelay(1)
			.OnStart(()=> DGUtils.Log("Rotate Start"))
			.OnStepComplete(()=> { stepCompleteT2++; DGUtils.Log("Rotate Step Complete"); })
			.OnComplete(()=> { completeT2++; })
		);
		seq.Insert(
			0.5f, mat.DOColor(Color.green, 1)
			.SetId("Color")
			.OnStart(()=> DGUtils.Log("Color Start"))
			.OnStepComplete(()=> { stepCompleteT3++; DGUtils.Log("Color Step Complete"); })
			.OnComplete(()=> { completeT3++; })
		);
		seq.AppendInterval(0.5f);

		seq.InsertCallback(1.25f, ()=> DGUtils.Log("1.25f Sequence callback"));

		Sequence seqPre = DOTween.Sequence()
			.SetId("Sequence OUTER")
			.OnStart(()=> DGUtils.Log("Sequence OUTER Start"))
			.OnStepComplete(()=> { stepCompleteS1++; DGUtils.Log("Sequence OUTER Step Complete"); })
			.OnComplete(()=> { completeS1++; });
		seqPre.Append(seq);
		seqPre.PrependInterval(1);

		Sequence mainSeq = DOTween.Sequence().SetUpdate(true).SetLoops(loops, loopType).SetAutoKill(false)
			.SetId("MAIN SEQUENCE")
			.OnStart(()=> DGUtils.Log("MAINSequence Start"))
			.OnStepComplete(()=> { stepCompleteMS++; DGUtils.Log("MAINSEQUENCE Step Complete"); })
			.OnComplete(()=> { completeMS++; });
		mainSeq.Append(seqPre);
		mainSeq.PrependInterval(1);
		target = ((GameObject)Instantiate(prefab)).transform;
		target.position = new Vector3(-5, 0, 0);
		mainSeq.Append(target.DOMove(Vector3.zero, 1));

		mainSeq.InsertCallback(1.75f, ()=> DGUtils.Log("1.75f MAINSEQUENCE callback"));
		mainSeq.PrependCallback(()=> DGUtils.Log("1.75f MAINSEQUENCE prepended callback"));

		return mainSeq;
	}

	void CreateFromSequence()
	{
		Transform target = ((GameObject)Instantiate(prefab)).transform;
		Sequence seq = DOTween.Sequence()
			.SetId("FROM Sequence")
			.OnStart(()=> DGUtils.Log("FROM Sequence Start"))
			.OnStepComplete(()=> { stepCompleteS2++; DGUtils.Log("FROM SEQUENCE Step Complete"); })
			.OnComplete(()=> DGUtils.Log("FROM SEQUENCE Complete"));
		seq.Append(target.DOMove(new Vector3(0, -3, 0), 1)
			.From(true)
		);
		seq.Append(target.DOMove(new Vector3(0, 3, 0), 1)
			.From(true)
		);
	}

	void CreateTween()
	{
		Transform target = ((GameObject)Instantiate(prefab)).transform;

		target.DOMove(new Vector3(2, 2, 2), 1f).SetLoops(1, LoopType.Yoyo)
			.SetId("Move (Tween)")
			.SetLoops(3)
			.OnComplete(()=> Destroy(target.gameObject));
	}

	void ResetStepsCounters()
	{
		stepCompleteMS = stepCompleteS1 = stepCompleteS2 = stepCompleteT1 = stepCompleteT2 = stepCompleteT3 = completeMS = completeS1 = completeS2 = completeT1 = completeT2 = completeT3 = 0;
	}
}