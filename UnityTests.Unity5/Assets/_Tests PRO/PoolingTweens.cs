using DG.Tweening;
using Holoville.HOPoolOperatorV2;
using UnityEngine;
using System.Collections;

public class PoolingTweens : BrainBase
{
    public GameObject prefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            Vector3 wPos = Camera.main.ScreenToWorldPoint(mousePos);
            if (Input.GetKey(KeyCode.LeftShift)) {
                // Spawn path
                if (Input.GetKey(KeyCode.Space)) {
                    Transform t = HOPoolManager.Spawn(TestPOP.DOPathCubeasChild, wPos);
                    t.GetComponentInChildren<DOTweenPath>().DORestart(true);
                } else {
                    Transform t = HOPoolManager.Spawn(TestPOP.DOPathCube, wPos);
                    t.GetComponent<DOTweenPath>().DORestart(true);
                }
            } else {
                // Spawn dice
                if (Input.GetKey(KeyCode.Space)) {
                    Transform t = HOPoolManager.Spawn(TestPOP.DOAnimationDiceasChild, wPos);
                    t.GetComponentInChildren<DOTweenAnimation>().DORestart(true);
                } else {
                    Transform t = HOPoolManager.Spawn(TestPOP.DOAnimationDice, wPos);
                    t.GetComponent<DOTweenAnimation>().DORestart(true);
                }
            }
        }
    }
}