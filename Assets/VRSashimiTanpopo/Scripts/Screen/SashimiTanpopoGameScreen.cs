using System.Linq;
using Cysharp.Threading.Tasks;
using FrameSynthesis;
using UniRx;
using UnityEngine;
using VContainer.Unity;
using VRSashimiTanpopo.Debug;
using VRSashimiTanpopo.Tanpopo;

namespace VRSashimiTanpopo.Screen
{
    public class SashimiTanpopoGameScreen : TanpopoGameScreen
    {
        readonly LifetimeScope lifetimeScope;
        readonly LifetimeScope sashimiTanpopoScoreBoardPrefab;
        readonly DebugSettings debugSettings;
        readonly HandTrackingGuide handTrackingGuide;
        
        const float TimeLimitSec = 60f;
        
        float sashimiSpawnTime;
        const float MinSashimiSpawnIntervalSec = 0.8f;
        const float MaxSashimiSpawnIntervalSec = 2f;

        LifetimeScope sashimiTanpopoScoreBoardLifetimeScope;

        readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        int twoTanpoposMountedCount;

        public SashimiTanpopoGameScreen(
            GameObject sashimiPrefab,
            GameObject tanpopoPrefab,
            LifetimeScope lifetimeScope,
            LifetimeScope sashimiTanpopoScoreBoardPrefab,
            DebugSettings debugSettings,
            HandTrackingGuide handTrackingGuide
            ) : base(sashimiPrefab, tanpopoPrefab)
        {
            this.lifetimeScope = lifetimeScope;
            this.sashimiTanpopoScoreBoardPrefab = sashimiTanpopoScoreBoardPrefab;
            
            this.debugSettings = debugSettings;
            this.handTrackingGuide = handTrackingGuide;
        }

        protected override async UniTask InitializeGameAsync()
        {
            ScoreModel.Reset();

            sashimiTanpopoScoreBoardLifetimeScope = lifetimeScope.CreateChildFromPrefab(sashimiTanpopoScoreBoardPrefab);

            TanpopoSupplier.Supply(40);

            // 生成したタンポポの物理シミュレーションが落ちつくまで待ってガードを解除しフェードインする。
            await UniTask.Delay(1000);
            Object.FindObjectsOfType<TanpopoDropGuard>().ToList().ForEach(tanpopoDropGuard => tanpopoDropGuard.Hide());

            DisplayFader.FadeIn();

            var gameStartMessage = ScreenManager.GameStartMessageFactory.Invoke();
            
            CountdownTimerModel.Reset(debugSettings.ShortTimeLimit ? 5f : TimeLimitSec);
            CountdownTimerModel.Finished += OnCountdownTimerFinished;

            VoicePlayer.Play(Voice.Start2, delaySec: 3.8);
            await UniTask.Delay(4500);
            MusicPlayer.Play(Music.ThemeOfSashimiTanpopo);
            await UniTask.Delay(1000);

            Object.Destroy(gameStartMessage.gameObject);
            
            CountdownTimerModel.Start();

            PrepareInGameVoices();
        }
        
        void PrepareInGameVoices()
        {
            VoicePlayer.Play(Voice.TanpopoInstruction, delaySec: 0.7);
            VoicePlayer.Play(Voice.AlmostFinished, delaySec: 48);

            ScoreModel.Score.First(value => value == 3).Subscribe(x => PlayVoice(Voice.WayToGo)).AddTo(compositeDisposable);
            ScoreModel.Score.First(value => value == 9).Subscribe(x => PlayVoice(Voice.GoodJob)).AddTo(compositeDisposable);
            ScoreModel.Score.First(value => value == 17).Subscribe(x => PlayVoice(Voice.WayToGo, 1.01f)).AddTo(compositeDisposable);

            void PlayVoice(Voice voice, float pitch = 1f)
            {
                if (CountdownTimerModel.TimeRemaining.Value < 10f) return;
                VoicePlayer.Play(voice, 0.25, pitch);
            }
        }
        
        protected override void FinishGame()
        {
            compositeDisposable.Dispose();
            VoicePlayer.StopAll();
            
            if (sashimiTanpopoScoreBoardLifetimeScope != null)
            {
                sashimiTanpopoScoreBoardLifetimeScope.Dispose();
            }

            CountdownTimerModel.Finished -= OnCountdownTimerFinished;
        }

        void OnCountdownTimerFinished()
        {
            VoicePlayer.Play(Voice.Finished1);
            handTrackingGuide.PlayGuidance();
            ProcessGameOver();
            
            ScreenManager.AchievementManager.UnlockFirstSashimiTanpopo();
            if (ScoreModel.IsPerfect())
            {
                ScreenManager.AchievementManager.UnlockSashimiTanpopoPerfect();
            }
        }

        protected override void UpdateGame()
        {
            if (CountdownTimerModel.TimeRemaining.Value < 7f) return;

            sashimiSpawnTime -= Time.deltaTime;
            if (sashimiSpawnTime < 0f)
            {
                var difficulty = MathHelper.LinearClampedMap(CountdownTimerModel.TimeRemaining.Value,
                    TimeLimitSec, 0f, 0f, 1f);

                sashimiSpawnTime += MathHelper.LinearClampedMap(difficulty, 0f, 1f, MaxSashimiSpawnIntervalSec,
                    MinSashimiSpawnIntervalSec);
                
                var sashimi = SupplySashimi(true);
                sashimi.ScoreIncremented += IncrementScore;
                sashimi.ScoreDecremented += DecrementScore;
                sashimi.TwoTanpopoMounted += PlayOnlyOneTanpopoVoice;
            }
        }

        void IncrementScore()
        {
            ScoreModel.IncrementScore();
            SoundEffectPlayer.Play(SoundEffect.Score);
        }

        void DecrementScore()
        {
            ScoreModel.DecrementScore();
        }

        void PlayOnlyOneTanpopoVoice()
        {
            twoTanpoposMountedCount++;
            
            if (twoTanpoposMountedCount == 1 || twoTanpoposMountedCount == 3)
            {
                VoicePlayer.Play(Voice.OnlyOneTanpopoPlease);
            }
        }
    }
}
