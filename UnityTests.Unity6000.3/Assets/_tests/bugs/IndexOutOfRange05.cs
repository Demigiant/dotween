using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IndexOutOfRange05 : BrainBase
{
    Sequence _sequence;
    
    void Play()
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(this.transform.DOMoveX(1, 2));
        _sequence.OnComplete(() =>
        {
            Debug.Log("Complete");
            if (this.gameObject != null) {
                this.gameObject.SetActive(false);
                this.gameObject.SetActive(true);
            }
        });
    }
    
    void OnDisable()
    {
        Debug.Log("OnDisable");
        _sequence.Complete();
    }
    
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space)) Play();
    }
}