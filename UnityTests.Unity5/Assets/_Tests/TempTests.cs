using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempTests : MonoBehaviour
{
    public Transform target;
    public bool forceEnd;
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        DOTween.To(() => target.position, x => {target.position = x; Debug.Log("FFF");}, Vector3.zero, 1);
    }
}