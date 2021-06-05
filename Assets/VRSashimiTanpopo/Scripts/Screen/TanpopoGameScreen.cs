using Cysharp.Threading.Tasks;
using UnityEngine;
using VRSashimiTanpopo.Logic;
using VRSashimiTanpopo.Sashimi;
using VRSashimiTanpopo.Tanpopo;
using VRSashimiTanpopo.UI;

namespace VRSashimiTanpopo.Screen
{
    public abstract class TanpopoGameScreen : Screen
    {
        readonly GameObject sashimiPrefab;
        readonly GameObject tanpopoPrefab;

        const float ConveyorSpeed = 0.25f;

        ResetButtonCanvas resetButtonCanvas;

        Transform sashimiSpawnPoint;

        protected TanpopoSupplier TanpopoSupplier;
        protected SashimiSupplier SashimiSupplier;
        protected Conveyor.Conveyor Conveyor;

        protected TanpopoGameScreen(GameObject sashimiPrefab, GameObject tanpopoPrefab)
        {
            this.sashimiPrefab = sashimiPrefab;
            this.tanpopoPrefab = tanpopoPrefab;
        }
        
        public override async UniTask InitializeAsync()
        {
            switch (this)
            {
                case SashimiTanpopoGameScreen _:
                    await ScreenManager.SceneLoader.LoadAdditiveSceneAsync(SceneName.SashimiTanpopoFactory);
                    break;
                case InfiniteTanpopoGameScreen _:
                    await ScreenManager.SceneLoader.LoadAdditiveSceneAsync(SceneName.InfiniteTanpopoFactory);
                    break;
            }

            Conveyor = Object.FindObjectOfType<Conveyor.Conveyor>();
            sashimiSpawnPoint = Object.FindObjectOfType<SashimiSpawnPoint>().transform;
            
            TanpopoSupplier = new TanpopoSupplier(tanpopoPrefab);
            SashimiSupplier = new SashimiSupplier(sashimiPrefab);

            GameModel.IsGameOver.Value = false;

            await InitializeGameAsync();
            
            resetButtonCanvas = ScreenManager.ResetButtonCanvasFactory.Invoke(ResetGame);

            GameModel.Playing.Value = true;
            Conveyor.Speed = ConveyorSpeed;
        }

        protected virtual async UniTask InitializeGameAsync()
        {
        }

        protected abstract void UpdateGame();
        protected abstract void FinishGame();

        public override async UniTask FinishAsync()
        {
            GameModel.Playing.Value = false;
            DisplayFader.FadeOut();

            await UniTask.Delay(1000);
            
            TanpopoSupplier.DisposeAllTanpopos();
            SashimiSupplier.DisposeAllSashimis();
            
            FinishGame();
            
            Object.Destroy(resetButtonCanvas.gameObject);

            await ScreenManager.SceneLoader.UnloadSceneAsync();
        }

        protected Sashimi.Sashimi SupplySashimi(bool scoreEnabled)
        {
            return SashimiSupplier.Supply(sashimiSpawnPoint, scoreEnabled);
        }

        public override void Update()
        {
            if (Input.GetButtonDown("Reset"))
            {
                ResetGame();
            }
            
            UpdateGame();
        }

        protected void ProcessGameOver()
        {
            MusicPlayer.Play(Music.GameOver);

            GameModel.Playing.Value = false;
            GameModel.IsGameOver.Value = true;

            SoundEffectPlayer.Play(SoundEffect.GameOver);
        }

        void ResetGame()
        {
            SoundEffectPlayer.Play(SoundEffect.MenuSelect);
            ScreenManager.ChangeScreen(ScreenManager.TitleScreenFactory.Invoke()).Forget();
        }
    }
}