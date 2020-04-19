// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2017/10/28 20:13
// License Copyright (c) Daniele Giardini

using System.Collections.Generic;

namespace DG.Tweening
{
    /// <summary>
    /// Extra DOTween methods not included in public engine (for consistency reasons)
    /// </summary>
    public static class DOTweenExtras
    {
        static readonly List<Tween> _RecyclableList = new List<Tween>();

        #region Public Methods

        #region Info Getters

        /// <summary>
        /// Returns TRUE if there are tweens with the given id, and they're all currently playing forward
        /// </summary>
        public static bool IsPlayForwardById(object id)
        {
            List<Tween> tweens = DOTween.TweensById(id, false, _RecyclableList);
            return AreAllPlayingForward(tweens);
        }
        /// <summary>
        /// Returns TRUE if there are tweens with the given target, and they're all currently playing forward
        /// </summary>
        public static bool IsPlayForwardByTarget(object target)
        {
            List<Tween> tweens = DOTween.TweensByTarget(target, false, _RecyclableList);
            return AreAllPlayingForward(tweens);
        }

        /// <summary>
        /// Returns TRUE if there are tweens with the given id, and they're all currently playing backwards
        /// </summary>
        public static bool IsPlayBackwardsById(object id)
        {
            List<Tween> tweens = DOTween.TweensById(id, false, _RecyclableList);
            return AreAllPlayingBackwards(tweens);
        }
        /// <summary>
        /// Returns TRUE if there are tweens with the given target, and they're all currently playing backwards
        /// </summary>
        public static bool IsPlayBackwardsByTarget(object target)
        {
            List<Tween> tweens = DOTween.TweensByTarget(target, false, _RecyclableList);
            return AreAllPlayingBackwards(tweens);
        }

        /// <summary>
        /// Returns TRUE if there are tweens with the given id, and they're all currently paused
        /// </summary>
        public static bool IsPausedById(object id)
        {
            List<Tween> tweens = DOTween.TweensById(id, false, _RecyclableList);
            return AreAllPaused(tweens);
        }
        /// <summary>
        /// Returns TRUE if there are tweens with the given target, and they're all currently paused
        /// </summary>
        public static bool IsPausedByTarget(object target)
        {
            List<Tween> tweens = DOTween.TweensByTarget(target, false, _RecyclableList);
            return AreAllPaused(tweens);
        }

        #endregion

        #endregion

        #region Methods

        static bool AreAllPlayingForward(List<Tween> tweens)
        {
            if (tweens == null) return false;
            foreach (Tween t in tweens) {
                if (!t.IsPlaying() || t.isBackwards) return false;
            }
            return true;
        }

        static bool AreAllPlayingBackwards(List<Tween> tweens)
        {
            if (tweens == null) return false;
            foreach (Tween t in tweens) {
                if (!t.IsPlaying() || !t.isBackwards) return false;
            }
            return true;
        }

        static bool AreAllPaused(List<Tween> tweens)
        {
            if (tweens == null) return false;
            foreach (Tween t in tweens) {
                if (t.IsPlaying()) return false;
            }
            return true;
        }

        #endregion
    }
}