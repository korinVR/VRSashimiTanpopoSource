using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VRSashimiTanpopo.UI;
using Object = UnityEngine.Object;

namespace VRSashimiTanpopo.Screen
{
    public class TitleScreen : Screen
    {
        readonly Func<TitleMenu> titleMenuFactory;

        TitleMenu titleMenu;

        public TitleScreen(Func<TitleMenu> titleMenuFactory)
        {
            this.titleMenuFactory = titleMenuFactory;
        }

        public override async UniTask InitializeAsync()
        {
            DisplayFader.HideDisplay();

            titleMenu = titleMenuFactory.Invoke();
            titleMenu.Started += StartGame;
            
            await ScreenManager.SceneLoader.LoadAdditiveSceneAsync(SceneName.Title);

            MusicPlayer.Play(Music.Title);
            VoicePlayer.Play(Voice.VRSashimiTanpopo);

            DisplayFader.FadeIn();
            await UniTask.Delay(2000);
            
            titleMenu.ShowTopMenu();
        }

        public override async UniTask FinishAsync()
        {
            DisplayFader.FadeOut();
            
            await UniTask.Delay(1000);
            
            Object.Destroy(titleMenu.gameObject);

            await ScreenManager.SceneLoader.UnloadSceneAsync();
        }

        void StartGame(GameMode gameMode)
        {
            GameModel.GameMode.Value = gameMode;
            
            switch (gameMode)
            {
                case GameMode.SashimiTanpopo:
                    ScreenManager.ChangeScreen(ScreenManager.SashimiTanpopoGameScreenFactory.Invoke()).Forget();
                    break;
                case GameMode.InfiniteTanpopo:
                    ScreenManager.ChangeScreen(ScreenManager.InfiniteTanpopoGameScreenFactory.Invoke()).Forget();
                    break;
            }
        }

        public override void Update()
        {
            if (Input.GetButtonDown("Start"))
            {
                StartGame(GameMode.SashimiTanpopo);
            }
        }
    }
}
