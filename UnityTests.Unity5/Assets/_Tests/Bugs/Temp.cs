using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	[SerializeField]
    private Vector3 pos;
    [SerializeField]
    private float duration;
    [SerializeField]
    private GameObject goToMove;

    private Tweener moveTween;

    // Update is called once per frame
    void Update() {
        if(moveTween == null) {
            moveTween = goToMove.transform.DOLocalMove(pos, duration).SetAutoKill(false);
        }
        moveTween.ChangeEndValue(pos, duration, true);
    }
}