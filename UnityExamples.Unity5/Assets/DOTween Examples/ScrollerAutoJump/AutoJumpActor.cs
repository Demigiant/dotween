// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2021/08/09

using System;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Auto-jumping actor
/// </summary>
public class AutoJumpActor : MonoBehaviour
{
    // Serialized
    [Range(0.2f, 2f)]
    [SerializeField] float _jumpDuration = 0.75f;
    [Range(0.1f, 1f)]
    [SerializeField] float _jumpPower = 0.5f;
    [SerializeField] bool _distortAnimation = true;

    Transform _parent;
    Tween _jumpTween;
    Sequence _distortTween;

    void Awake()
    {
        _parent = this.transform.parent;
    }

    void Start()
    {
        // Create distortion tween to give impression of "bending and pushing upwards, then landing"
        // (this is not the actual jump movement, just the "body distortion")
        float defScaleY = _parent.localScale.y;
        _distortTween = DOTween.Sequence().SetAutoKill(false).Pause()
            .Append(_parent.DOScaleY(defScaleY * 0.25f, 0.05f))
            .Append(_parent.DOScaleY(defScaleY * 1.25f, 0.15f))
            .Insert(_jumpDuration * 0.5f, _parent.DOScaleY(defScaleY * 0.5f, _jumpDuration * 0.5f - 0.1f).SetEase(Ease.InQuad))
            .Append(_parent.DOScaleY(defScaleY, 0.1f))
            .Append(_parent.DOPunchScale(new Vector3(0, 0.2f), 0.45f, 3))
            .Insert(0, _parent.DOLocalRotate(new Vector3(0, 0, -20), _jumpDuration * 0.2f))
            .Insert(_jumpDuration * 0.4f, _parent.DOLocalRotate(new Vector3(0, 0, 10), _jumpDuration * 0.35f))
            .Insert(_jumpDuration * 0.75f, _parent.DOLocalRotate(new Vector3(0, 0, 0), _jumpDuration * 0.25f));
    }

    void OnTriggerEnter(Collider other)
    {
        // Find the platform we triggered
        AutoJumpPlatform platform = other.GetComponentInParent<AutoJumpPlatform>();
        if (platform == null) return; // Not a platform collider, ignore

        bool isAlreadyJumping = _jumpTween.IsActive();
        if (isAlreadyJumping) return; // Don't jump again while already jumping

        // Determine if it's a jump-to or a jump-out-of platform
        // by direction-of-motion (considering we move left-to-right)
        bool isJumpToPlatform = other.transform.position.x < platform.transform.position.x;
        // Jump
        if (isJumpToPlatform) JumpTo(platform.transform.position.y);
        else JumpTo(0);
    }

    void OnDestroy()
    {
        // Clear tweens (always a good place to do that)
        _jumpTween.Kill();
        _distortTween.Kill();
    }

    void JumpTo(float toY)
    {
        _jumpTween.Kill();
        if (_distortAnimation) {
            _distortTween.timeScale = AutoJumpScene.speed; // Set timeScale of tween based on world speed
            _distortTween.Restart();
        }
        Vector3 currP = _parent.position;
        Vector3 to = currP;
        to.y = toY;
        // Find a harmonic jumpPower
        // (so that even when going down we always jump a little higher than where we are)
        float diffY = to.y - currP.y;
        float jumpPower = diffY < 0 ? Mathf.Abs(diffY) + _jumpPower * 0.5f : _jumpPower;
        // Jump
        _jumpTween = _parent.DOJump(to, jumpPower, 1, _jumpDuration);
        _jumpTween.timeScale = AutoJumpScene.speed; // Set timeScale of tween based on world speed
    }
}
