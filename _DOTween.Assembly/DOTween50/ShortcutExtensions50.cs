// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/03/21 18:43

using UnityEngine.Audio;

namespace DG.Tweening
{
    /// <summary>
    /// Methods that extend known Unity objects and allow to directly create and control tweens from their instances.
    /// These, as all DOTween50 methods, require Unity 5.0 or later.
    /// </summary>
    public static class ShortcutExtensions50
    {
        #region AudioMixerGroup

        /// <summary>Tweens an AudioMixer's exposed float to the given value.
        /// Also stores the AudioMixer as the tween's target so it can be used for filtered operations.
        /// Note that you need to manually expose a float in an AudioMixerGroup in order to be able to tween it from an AudioMixer.</summary>
        /// <param name="floatName">Name given to the exposed float to set</param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOSetFloat(this AudioMixer target, string floatName, float endValue, float duration)
        {
            return DOTween.To(()=> {
                float currVal;
                target.GetFloat(floatName, out currVal);
                return currVal;
            }, x=> target.SetFloat(floatName, x), endValue, duration)
            .SetTarget(target);
        }

        #endregion

        #region Operation Shortcuts

        /// <summary>
        /// Completes all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens completed
        /// (meaning the tweens that don't have infinite loops and were not already complete)
        /// </summary>
        /// <param name="withCallbacks">For Sequences only: if TRUE also internal Sequence callbacks will be fired,
        /// otherwise they will be ignored</param>
        public static int DOComplete(this AudioMixer target, bool withCallbacks = false)
        {
            return DOTween.Complete(target, withCallbacks);
        }

        /// <summary>
        /// Kills all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens killed.
        /// </summary>
        /// <param name="complete">If TRUE completes the tween before killing it</param>
        public static int DOKill(this AudioMixer target, bool complete = false)
        {
            //            int tot = complete ? DOTween.CompleteAndReturnKilledTot(target) : 0;
            //            return tot + DOTween.Kill(target);
            return DOTween.Kill(target, complete);
        }

        /// <summary>
        /// Flips the direction (backwards if it was going forward or viceversa) of all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens flipped.
        /// </summary>
        public static int DOFlip(this AudioMixer target)
        {
            return DOTween.Flip(target);
        }

        /// <summary>
        /// Sends to the given position all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens involved.
        /// </summary>
        /// <param name="to">Time position to reach
        /// (if higher than the whole tween duration the tween will simply reach its end)</param>
        /// <param name="andPlay">If TRUE will play the tween after reaching the given position, otherwise it will pause it</param>
        public static int DOGoto(this AudioMixer target, float to, bool andPlay = false)
        {
            return DOTween.Goto(target, to, andPlay);
        }

        /// <summary>
        /// Pauses all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens paused.
        /// </summary>
        public static int DOPause(this AudioMixer target)
        {
            return DOTween.Pause(target);
        }

        /// <summary>
        /// Plays all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlay(this AudioMixer target)
        {
            return DOTween.Play(target);
        }

        /// <summary>
        /// Plays backwards all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlayBackwards(this AudioMixer target)
        {
            return DOTween.PlayBackwards(target);
        }

        /// <summary>
        /// Plays forward all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlayForward(this AudioMixer target)
        {
            return DOTween.PlayForward(target);
        }

        /// <summary>
        /// Restarts all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens restarted.
        /// </summary>
        public static int DORestart(this AudioMixer target)
        {
            return DOTween.Restart(target);
        }

        /// <summary>
        /// Rewinds all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens rewinded.
        /// </summary>
        public static int DORewind(this AudioMixer target)
        {
            return DOTween.Rewind(target);
        }

        /// <summary>
        /// Smoothly rewinds all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens rewinded.
        /// </summary>
        public static int DOSmoothRewind(this AudioMixer target)
        {
            return DOTween.SmoothRewind(target);
        }

        /// <summary>
        /// Toggles the paused state (plays if it was paused, pauses if it was playing) of all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens involved.
        /// </summary>
        public static int DOTogglePause(this AudioMixer target)
        {
            return DOTween.TogglePause(target);
        }

        #endregion
    }
}