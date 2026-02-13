using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class UIToolkitTests : BrainBase
{
    enum TweenType
    {
        By, To
    }

    enum MoveType
    {
        Vector3, Vector2,
        X, Y, Z
    }
    
    enum ScaleType
    {
        Vector3, Float
    }
    
    [Header("Settings")]
    [Range(0, 5)]
    [SerializeField] float _startupDelay = 0.5f;
    [Range(0.5f, 10f)]
    [SerializeField] float _duration = 1;
    [SerializeField] Ease _ease = Ease.InOutSine;
    [SerializeField] LoopType _loopType = LoopType.Yoyo;
    [Range(-1, 20)]
    [SerializeField] int _loops = -1;
    [Space]
    [SerializeField] TweenType tweenType = TweenType.To;
    
    [Header("Move")]
    [SerializeField] bool _moveEnabled = true;
    [SerializeField] MoveType _moveType = MoveType.Vector3;
    [SerializeField] Vector3[] _moveValsVector3;
    [SerializeField] Vector2[] _moveValsVector2;
    [SerializeField] float[] _moveValsFloat;
    
    [Header("Scale")]
    [SerializeField] bool _scaleEnabled = true;
    [SerializeField] ScaleType _scaleType = ScaleType.Vector3;
    [SerializeField] Vector2 _scaleToVector2 = new Vector3(2, 2);
    [SerializeField] float _scaleToFloat = 2;
    
    [Header("Rotate")]
    [SerializeField] bool _rotateEnabled = true;
    [Range(-360, 360)]
    [SerializeField] int _rotateTo = 45;
    
    [Header("References")]
    [SerializeField] UIDocument _uiDoc;
    
    IEnumerator Start()
    {
        VisualElement root = _uiDoc.rootVisualElement;
        
        VisualElement img = root.Q<VisualElement>("img");
        VisualElement punchImg = root.Q<VisualElement>("punchImg01");
        VisualElement shakeImg01 = root.Q<VisualElement>("shakeImg01");
        VisualElement shakeImg02 = root.Q<VisualElement>("shakeImg02");
        if (_startupDelay > 0) yield return new WaitForSeconds(_startupDelay);

        Sequence s = DOTween.Sequence().SetLoops(_loops, _loopType);
        if (_moveEnabled)
        {
            switch (_moveType)
            {
                case MoveType.Vector2:
                    foreach (Vector2 v in _moveValsVector2)
                        s.Append(img.DOMove(v, _duration).SetRelative(tweenType == TweenType.By).SetEase(_ease));
                    break;
                case MoveType.X:
                    foreach (float v in _moveValsFloat)
                        s.Append(img.DOMoveX(v, _duration).SetRelative(tweenType == TweenType.By).SetEase(_ease));
                    break;
                case MoveType.Y:
                    foreach (float v in _moveValsFloat)
                        s.Append(img.DOMoveY(v, _duration).SetRelative(tweenType == TweenType.By).SetEase(_ease));
                    break;
                case MoveType.Z:
                    foreach (float v in _moveValsFloat)
                        s.Append(img.DOMoveZ(v, _duration).SetRelative(tweenType == TweenType.By).SetEase(_ease));
                    break;
                default:
                    foreach (Vector3 v in _moveValsVector3)
                        s.Append(img.DOMove(v, _duration).SetRelative(tweenType == TweenType.By).SetEase(_ease));
                    break;
            }
        }
        if (_scaleEnabled)
        {
            switch (_scaleType)
            {
                case ScaleType.Float:
                    s.Insert(0, img.DOScale(_scaleToFloat, _duration).SetRelative(tweenType == TweenType.By).SetEase(_ease));
                    break;
                default:
                    s.Insert(0, img.DOScale(_scaleToVector2, _duration).SetRelative(tweenType == TweenType.By).SetEase(_ease));
                    break;
            }
        }
        if (_rotateEnabled)
        {
            s.Insert(0, img.DORotate(_rotateTo, _duration).SetRelative(tweenType == TweenType.By).SetEase(_ease));
        }
        
        // Punch
        s.Insert(0, punchImg.DOPunch(new Vector3(10, 10, 0), _duration));
        // Shake
        s.Insert(0, shakeImg01.DOShake(_duration, new Vector2(10, 10)));
        s.Insert(0, shakeImg02.DOShake(_duration, 10));
    }
}
