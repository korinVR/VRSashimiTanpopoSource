using System;
using Cysharp.Threading.Tasks;
using FrameSynthesis.XR;
using UnityEngine.Assertions;
using VContainer;
using VContainer.Unity;
using VRSashimiTanpopo.Achievements;
using VRSashimiTanpopo.Model;
using VRSashimiTanpopo.UI;

namespace VRSashimiTanpopo.Screen
{
    public class ScreenManager : IStartable, ITickable
    {
        public Func<TitleScreen> TitleScreenFactory { get; }
        public Func<SashimiTanpopoGameScreen> SashimiTanpopoGameScreenFactory { get; }
        public Func<InfiniteTanpopoGameScreen> InfiniteTanpopoGameScreenFactory { get; }

        public Func<GameStartMessage> GameStartMessageFactory { get; }
        public Func<Action, ResetButtonCanvas> ResetButtonCanvasFactory { get; }

        public SceneLoader SceneLoader { get; }

        public IAchievementManager AchievementManager { get; }
        
        readonly IDisplayFader displayFader;

        readonly MusicPlayer musicPlayer;
        readonly VoicePlayer voicePlayer;
        readonly SoundEffectPlayer soundEffectPlayer;

        readonly GameModel gameModel;
        readonly ScoreModel scoreModel;
        readonly CountdownTimerModel countdownTimerModel;
        readonly WorkingTimeModel workingTimeModel;

        Screen currentScreen;

        bool tickable;

        [Inject]
        public ScreenManager(
            Func<TitleScreen> titleScreenFactory,
            Func<SashimiTanpopoGameScreen> sashimiTanpopoGameScreenFactory,
            Func<InfiniteTanpopoGameScreen> infiniteTanpopoGameScreenFactory,

            Func<GameStartMessage> gameStartMessageFactory,
            Func<Action, ResetButtonCanvas> resetButtonCanvasFactory,
            
            SceneLoader sceneLoader,
            IDisplayFader displayFader,
            
            MusicPlayer musicPlayer,
            VoicePlayer voicePlayer,
            SoundEffectPlayer soundEffectPlayer,
            
            GameModel gameModel,
            ScoreModel scoreModel,
            CountdownTimerModel countdownTimerModel,
            WorkingTimeModel workingTimeModel,
            
            IAchievementManager achievementManager)
        {
            TitleScreenFactory = titleScreenFactory;
            SashimiTanpopoGameScreenFactory = sashimiTanpopoGameScreenFactory;
            InfiniteTanpopoGameScreenFactory = infiniteTanpopoGameScreenFactory;

            GameStartMessageFactory = gameStartMessageFactory;
            ResetButtonCanvasFactory = resetButtonCanvasFactory;

            SceneLoader = sceneLoader;
            
            this.musicPlayer = musicPlayer;
            this.voicePlayer = voicePlayer;
            this.soundEffectPlayer = soundEffectPlayer;
            
            this.displayFader = displayFader;

            this.gameModel = gameModel;
            this.scoreModel = scoreModel;
            this.countdownTimerModel = countdownTimerModel;
            this.workingTimeModel = workingTimeModel;

            AchievementManager = achievementManager;
        }

        public void Start()
        {
            ChangeScreen(TitleScreenFactory.Invoke()).Forget();
        }

        public void Tick()
        {
            if (!tickable) return;
            currentScreen.Update();
        }

        public async UniTask ChangeScreen(Screen screen)
        {
            Assert.IsNotNull(screen);

            tickable = false;

            if (currentScreen != null)
            {
                await currentScreen.FinishAsync();
            }
            
            screen.ScreenManager = this;

            // 使用頻度の高いもの
            screen.DisplayFader = displayFader;
            
            screen.MusicPlayer = musicPlayer;
            screen.VoicePlayer = voicePlayer;
            screen.SoundEffectPlayer = soundEffectPlayer;

            screen.GameModel = gameModel;
            screen.ScoreModel = scoreModel;
            screen.CountdownTimerModel = countdownTimerModel;
            screen.WorkingTimeModel = workingTimeModel;
            
            currentScreen = screen;
            await currentScreen.InitializeAsync();

            tickable = true;
        }
    }
}
