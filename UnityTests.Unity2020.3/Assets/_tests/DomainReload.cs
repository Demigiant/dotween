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
    [SerializeField] float _delayedCallDelay = 5;
    [SerializeField] GameObject _delayedCallTarget;
    [SerializeField] Button _delayedCallButton;
    
    void Start()
    {
        _delayedCallButton.onClick.AddListener(() => PersistentCORunner.StartCoroutine(CO_DelayedCall()));
    }

    #region DelayedCall Tests

    IEnumerator CO_DelayedCall()
    {
        Debug.Log("Starting Delayed Call");
        DOVirtual.DelayedCall(_delayedCallDelay, () => {
            Debug.Log("DelayedCall > Will set the target's position");
            _delayedCallTarget.transform.position = new Vector3(-2, 0, 0);
        });
        yield return new WaitForSeconds(_delayedCallDelay * 0.5f);
        Debug.Log("Reloading scene");
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded");
        SceneManager.sceneLoaded -= OnSceneLoaded;
        // PersistentCORunner.StartCoroutine(CO_DelayedCall_SceneLoaded());
    }

    static IEnumerator CO_DelayedCall_SceneLoaded()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Stopping play mode");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    #endregion
}
