using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RuntimeComponentInspector : BrainBase
{
    public int totTweensToCreate = 100, totSequencesToCreate = 100;
    public Transform prefab;
    public Transform instancesContainer;
    
    bool _isPlaying;
    readonly List<Transform> _tweenersTargets = new List<Transform>();
    readonly List<Transform> _sequencesTargets = new List<Transform>();
    
    void Start()
    {
        for (int i = 0; i < totTweensToCreate; ++i) {
            Transform t = Instantiate(prefab, instancesContainer);
            _tweenersTargets.Add(t);
            t.name = "Target " + i;
            t.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 5f), 0);
            t.DOMoveX(Random.Range(-1f, 1f), 2).SetRelative().Pause()
                .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
        for (int i = 0; i < totSequencesToCreate; ++i) {
            Sequence s = DOTween.Sequence().Pause()
                .SetLoops(-1, LoopType.Yoyo);
            for (int j = 0; j < 4; ++j) {
                Transform t = Instantiate(prefab, instancesContainer);
                _sequencesTargets.Add(t);
                t.name = "Target " + i + "." + j;
                t.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 5f), 0);
                s.Join(t.DOMoveX(Random.Range(-1f, 1f), 2).SetRelative()
                    .SetEase(Ease.Linear));
            }
            Sequence subS = DOTween.Sequence();
            for (int j = 0; j < 4; ++j) {
                Transform t = Instantiate(prefab, instancesContainer);
                _sequencesTargets.Add(t);
                t.name = "Target " + i + "." + j;
                t.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 5f), 0);
                subS.Join(t.DOMoveX(Random.Range(-1f, 1f), 2).SetRelative()
                    .SetEase(Ease.Linear));
            }
            s.Join(subS);
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) DeleteTargets(10, 10);
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            _isPlaying = !_isPlaying;
            if (_isPlaying) DOTween.PlayAll();
            else DOTween.PauseAll();
        }
    }
    
    public void DeleteTargets(int tweenTargets, int sequencesTargets)
    {
        int counter = tweenTargets;
        while (counter > 0 && _tweenersTargets.Count > 0) {
            counter--;
            Destroy(_tweenersTargets[0].gameObject);
            _tweenersTargets.RemoveAt(0);
        }
        counter = sequencesTargets;
        while (counter > 0 && _sequencesTargets.Count > 0) {
            counter--;
            Destroy(_sequencesTargets[0].gameObject);
            _sequencesTargets.RemoveAt(0);
        }
    }
}