using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DomainReload : BrainBase
{
    [Header("Delayed Call Tests")]
    [Range(1, 10)]
    [SerializeField] float _delayedCallDelay = 3;
    [SerializeField] GameObject _delayedCallTarget;
    [SerializeField] Button _delayedCallButton;
    
    void Start()
    {
        _delayedCallButton.onClick.RemoveAllListeners();
        _delayedCallButton.onClick.AddListener(() => PersistentCORunner.StartCoroutine(CO_DelayedCall()));
    }

    void OnDestroy()
    {
        _delayedCallButton.onClick.RemoveAllListeners();
    }

    #region DelayedCall Tests

    IEnumerator CO_DelayedCall()
    {
        Debug.Log("Creating temporary tween");
        Tween tempTween = _delayedCallTarget.transform.DOScale(1.5f, 0.5f);
        Debug.Log("Starting Delayed Call");
        DOVirtual.DelayedCall(_delayedCallDelay, () => {
            Debug.Log("DelayedCall > Will set the target's position");
            _delayedCallTarget.transform.position = new Vector3(-2, 0, 0);
        });
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Killing temporary tween and reloading scene");
        tempTween.Kill();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded");
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PersistentCORunner.StartCoroutine(CO_DelayedCall_SceneLoaded());
    }

    static IEnumerator CO_DelayedCall_SceneLoaded()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Stopping play mode");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    #endregion
}
