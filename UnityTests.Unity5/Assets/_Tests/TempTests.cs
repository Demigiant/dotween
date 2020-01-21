using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempTests : BrainBase
{
    public Transform target;
    float m_currentAngle;
    Tween m_tweener;
    float m_targetAngle = 0;
    float m_duration = 0;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        Move(false);
    }

    public void Move(bool isReversed)
    {
        if (m_tweener == null)
        {
            m_currentAngle = 0.0f;
            m_tweener = DOTween.To(() => m_currentAngle, newAngle => m_currentAngle = newAngle, m_targetAngle, m_duration)
                .SetUpdate(UpdateType.Fixed)
                .SetEase(Ease.Linear)
                .SetAutoKill(false);

            float anglePerSec = m_targetAngle / m_duration;
            float startTime = m_currentAngle / anglePerSec;
            m_tweener.Goto(startTime , true);
            Debug.Log("GOTO DONE");
        }

        if (!isReversed)
        {
            m_tweener.Restart();
        }
        else
        {
            m_tweener.PlayBackwards();
        }
    }
}