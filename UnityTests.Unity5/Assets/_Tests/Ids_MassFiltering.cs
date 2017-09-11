using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Ids_MassFiltering : MonoBehaviour
{
    public int totItemsPerId = 200;
    public Transform objectId;

    int[] intsToTween = new[] {1, 2, 3};
    int int0, int1, int2;

    void Start()
    {
        for (int i = 0; i < 3; ++i) {
            for (int c = 0; c < totItemsPerId; ++c) {
                int index = i;
                Tween t = DOTween.To(() => intsToTween[index], x => intsToTween[index] = x, UnityEngine.Random.Range(1, 10), 2)
                    .Pause().SetAutoKill(false).SetLoops(-1, LoopType.Yoyo);
                switch (i) {
                case 0: // Int (uses id)
                    t.SetId(15);
                    break;
                case 1: // String (uses stringId)
                    t.SetId("aString");
                    break;
                case 2: // Object (uses id)
                    t.SetId(objectId);
                    break;
                }
            }
        }
    }

    void OnGUI()
	{
		DGUtils.BeginGUI();
		GUILayout.Space(50);

	    float elapsed;
		GUILayout.BeginHorizontal();
	    if (GUILayout.Button("TogglePause by Int Id")) {
	        elapsed = Time.realtimeSinceStartup;
            DOTween.TogglePause(15);
            Debug.Log("elapsed: " + (decimal)(Time.realtimeSinceStartup - elapsed));
	    }
	    if (GUILayout.Button("TogglePause by String Id")) {
            elapsed = Time.realtimeSinceStartup;
	        DOTween.TogglePause("aString");
            Debug.Log("elapsed: " + (decimal)(Time.realtimeSinceStartup - elapsed));
	    }
	    if (GUILayout.Button("TogglePause by Object Id")) {
            elapsed = Time.realtimeSinceStartup;
	        DOTween.TogglePause(objectId);
            Debug.Log("elapsed: " + (decimal)(Time.realtimeSinceStartup - elapsed));
	    }
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}

    Vector3 RndVector3()
    {
        return new Vector3(
            UnityEngine.Random.Range(3, 10),
            UnityEngine.Random.Range(3, 10),
            UnityEngine.Random.Range(3, 10)
        );
    }
}