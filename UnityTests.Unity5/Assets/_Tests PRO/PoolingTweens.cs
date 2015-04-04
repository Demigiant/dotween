using DG.Tweening;
using Holoville.HOPoolOperatorV2;
using UnityEngine;
using System.Collections;

public class PoolingTweens : BrainBase
{
    public Transform container;
    public bool spawnInContainer;

    void Start()
    {
        container.DOMoveX(30, 40);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            Vector3 wPos = Camera.main.ScreenToWorldPoint(mousePos);
            if (Input.GetKey(KeyCode.LeftShift)) {
                // Spawn path
                if (Input.GetKey(KeyCode.Space)) {
                    HOPoolManager.Spawn(TestPOP.DOPathCubeasChild, wPos, spawnInContainer ? container : null);
                    // t.GetComponentInChildren<DOTweenPath>().DORestart(true);
                } else {
                    HOPoolManager.Spawn(TestPOP.DOPathCube, wPos, spawnInContainer ? container : null);
                    // t.GetComponent<DOTweenPath>().DORestart(true);
                }
            } else {
                // Spawn dice
                if (Input.GetKey(KeyCode.Space)) {
                    HOPoolManager.Spawn(TestPOP.DOAnimationDiceasChild, wPos, spawnInContainer ? container : null);
                    // t.GetComponentInChildren<DOTweenAnimation>().DORestart(true);
                } else {
                    HOPoolManager.Spawn(TestPOP.DOAnimationDice, wPos, spawnInContainer ? container : null);
                    // t.GetComponent<DOTweenAnimation>().DORestart(true);
                }
            }
        }
    }
}