using System;
using Cysharp.Threading.Tasks;
using FrameSynthesis.XR;
using VRSashimiTanpopo.Model;
using VRSashimiTanpopo.UI;

namespace VRSashimiTanpopo.Screen
{
    public abstract class Screen
    {
        public ScreenManager ScreenManager { get; set; }

        public virtual async UniTask InitializeAsync() { }
        public virtual async UniTask FinishAsync() { }

        public virtual void Update() { }

        public IDisplayFader DisplayFader { get; set; }

        public MusicPlayer MusicPlayer { get; set; }
        public VoicePlayer VoicePlayer { get; set; }
        public SoundEffectPlayer SoundEffectPlayer { get; set; }

        public GameModel GameModel { get; set; }
        public ScoreModel ScoreModel { get; set; }
        public CountdownTimerModel CountdownTimerModel { get; set; }
        public WorkingTimeModel WorkingTimeModel { get; set; }
    }
}
