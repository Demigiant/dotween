using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TempPro : MonoBehaviour
{
    public Transform target;
	public Vector3 damageShakeStrength = new Vector3( 0.4f, 0.2f, 0 );
    public int damageShakeVibration = 7;
    public float damageShakeRandomness = 15;
    public float damageShakeDuration = 1.5f;
    private Tweener screenShake;

    private Vector3 original;

    // Use this for initialization
    void Start()
    {
        original = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.K ) )
            Time.timeScale = Time.timeScale < 1 ? Time.timeScale = 1 : 0;

        if ( Input.GetKeyDown( KeyCode.S ) )
        {
            Vector3 shakeOffset = new Vector3();
            screenShake = DOTween.Shake( () => shakeOffset, x => { shakeOffset = x; target.position += x; }, damageShakeDuration, damageShakeStrength, damageShakeVibration, damageShakeRandomness, false )
                .SetUpdate( false );
//            screenShake = target.DOMoveX(10, damageShakeDuration);
//            screenShake = target.DOShakePosition(damageShakeDuration, damageShakeStrength, damageShakeVibration, damageShakeRandomness, false);
            screenShake.OnComplete( () =>
            {
                screenShake = target.DOMove( original, 0.5f ).OnComplete( () => screenShake = null ).SetUpdate( false );
            } );
        }
    }
}