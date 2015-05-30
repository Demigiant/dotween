// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/02/05 10:28

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core
{
    public class DOTweenSettings : ScriptableObject
    {
        public const string AssetName = "DOTweenSettings";

        public bool useSafeMode = true;
        public float timeScale = 1;
        public bool useSmoothDeltaTime;
        public bool showUnityEditorReport;
        public LogBehaviour logBehaviour = LogBehaviour.ErrorsOnly;
        public bool drawGizmos = true;
        public bool defaultRecyclable;
        public AutoPlay defaultAutoPlay = AutoPlay.All;
        public UpdateType defaultUpdateType;
        public bool defaultTimeScaleIndependent;
        public Ease defaultEaseType = Ease.OutQuad;
        public float defaultEaseOvershootOrAmplitude = 1.70158f;
        public float defaultEasePeriod = 0;
        public bool defaultAutoKill = true;
        public LoopType defaultLoopType = LoopType.Restart;

        // Editor-only
        public enum SettingsLocation
        {
            AssetsDirectory,
            DOTweenDirectory,
            DemigiantDirectory
        }
        public SettingsLocation storeSettingsLocation = SettingsLocation.AssetsDirectory;
    }
}