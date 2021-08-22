// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2021/08/09

using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoJumpScene : MonoBehaviour
{
    // Serialized
    [Range(1, 20)]
    [SerializeField] float _worldSpeed = 3; // Used to increase also jump speed
    [SerializeField] bool _autoplayOnStartup = true;
    [SerializeField] Transform _movingWorld;

    public static float speed { get { return I._worldSpeed; } }

    static AutoJumpScene I;
    Vector3 _worldStartP;
    bool _isPlaying;
    Renderer _lastPlatformRenderer;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        _worldStartP = _movingWorld.localPosition;
        
        // Find last platform to the furthest positive X
        Renderer[] platformsRenderers = _movingWorld.GetComponentsInChildren<Renderer>();
        _lastPlatformRenderer = platformsRenderers[0];
        for (int i = 1; i < platformsRenderers.Length; ++i) {
            if (platformsRenderers[i].transform.position.x > _lastPlatformRenderer.transform.position.x) {
                _lastPlatformRenderer = platformsRenderers[i];
            }
        }

        if (_autoplayOnStartup) TogglePlay();
    }

    void Update()
    {
        // Play/pause game, reload scene controls
        if (Input.GetKeyDown(KeyCode.Space)) TogglePlay();
        else if (Input.GetKeyDown(KeyCode.F5)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (!_isPlaying) return;

        // Move world
        Vector3 toWorldP = _movingWorld.localPosition;
        toWorldP.x -= _worldSpeed * Time.deltaTime;
        _movingWorld.localPosition = toWorldP;

        // Check if world is not visible anymore, in which case reset its X so it can loop
        // (dirty trick but ok for this case)
        if (_lastPlatformRenderer.transform.position.x < 0 && !_lastPlatformRenderer.isVisible) {
            _movingWorld.localPosition = _worldStartP;
        }
    }

    // Play/pause game
    void TogglePlay()
    {
        _isPlaying = !_isPlaying;
        if (_isPlaying) DOTween.PlayAll();
        else DOTween.PauseAll();
    }
}