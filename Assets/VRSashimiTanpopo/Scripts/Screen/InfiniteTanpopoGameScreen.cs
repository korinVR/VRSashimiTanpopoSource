using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using VRSashimiTanpopo.Achievements;
using VRSashimiTanpopo.Tanpopo;

namespace VRSashimiTanpopo.Screen
{
    public class InfiniteTanpopoGameScreen : TanpopoGameScreen
    {
        readonly LifetimeScope lifetimeScope;
        readonly LifetimeScope infiniteTanpopoScoreBoardPrefab;

        LifetimeScope infiniteTanpopoScoreBoardLifetimeScope;

        float sashimiSpawnTime;

        TanpopoAlertSound tanpopoAlertSound;

        InfiniteTanpopoAchievementUnlocker infiniteTanpopoAchievementUnlocker;

        public InfiniteTanpopoGameScreen(
            GameObject sashimiPrefab,
            GameObject tanpopoPrefab,
            LifetimeScope lifetimeScope,
            LifetimeScope infiniteTanpopoScoreBoardPrefab
            ) : base(sashimiPrefab, tanpopoPrefab)
        {
            this.lifetimeScope = lifetimeScope;
            this.infiniteTanpopoScoreBoardPrefab = infiniteTanpopoScoreBoardPrefab;
        }
        
        protected override async UniTask InitializeGameAsync()
        {
            tanpopoAlertSound = Object.FindObjectOfType<TanpopoAlertSound>();

            infiniteTanpopoScoreBoardLifetimeScope = lifetimeScope.CreateChildFromPrefab(infiniteTanpopoScoreBoardPrefab);

            TanpopoSupplier.Supply(16);
            
            // 生成したタンポポの物理シミュレーションが落ちつくまで待ってガードを解除しフェードインする。
            await UniTask.Delay(1000);
            Object.FindObjectsOfType<TanpopoDropGuard>().ToList().ForEach(tanpopoDropGuard => tanpopoDropGuard.Hide());

            DisplayFader.FadeIn();

            var gameStartMessage = ScreenManager.GameStartMessageFactory.Invoke();
            
            WorkingTimeModel.Reset();
            infiniteTanpopoAchievementUnlocker = new InfiniteTanpopoAchievementUnlocker(ScreenManager.AchievementManager);
            
            VoicePlayer.Play(Voice.Start1, delaySec:3.8);
            await UniTask.Delay(5500);
            
            Object.Destroy(gameStartMessage.gameObject);
            
            WorkingTimeModel.Start();
            MusicPlayer.Play(Music.ThemeOfInfiniteTanpopo);
        }

        protected override void FinishGame()
        {
            infiniteTanpopoScoreBoardLifetimeScope.Dispose();
        }

        protected override void UpdateGame()
        {
            if (GameModel.IsGameOver.Value) return;

            sashimiSpawnTime -= Time.deltaTime;
            if (sashimiSpawnTime < 0f)
            {
                if (TanpopoSupplier.TanpopoCount < 12)
                {
                    SupplyTanpopos().Forget();
                }

                sashimiSpawnTime += 1.1f;
                SupplySashimi(false);
            }

            UpdateTanpopoAlert();
            CheckGameOver();

            infiniteTanpopoAchievementUnlocker.UpdateTime(WorkingTimeModel);
        }

        async UniTask SupplyTanpopos()
        {
            // タンポポを一度に生成するとQuest 2で6msくらいかかるので複数フレームに渡って生成する。
            for (var i = 0; i < 3; i++)
            {
                TanpopoSupplier.Supply(4);
                await UniTask.DelayFrame(1);
            }
        }

        void UpdateTanpopoAlert()
        {
            var missingSashimi = false;
            
            foreach (var sashimi in SashimiSupplier.Sashimis)
            {
                if (sashimi.transform.position.x < -0.2f && sashimi.HasNoTanpopo)
                {
                    sashimi.StartAlert();
                    missingSashimi = true;
                }
                else
                {
                    sashimi.StopAlert();
                }
            }

            if (missingSashimi)
            {
                tanpopoAlertSound.Play();
            }
            else
            {
                tanpopoAlertSound.Stop();
            }
        }

        void CheckGameOver()
        {
            if (!SashimiSupplier.Sashimis.Any(sashimi =>
                sashimi.HasNoTanpopo && sashimi.transform.position.x < -1.45f)) return;
            
            tanpopoAlertSound.Stop();
                
            Conveyor.Stop();
            WorkingTimeModel.Stop();

            VoicePlayer.Play(Voice.Finished2);

            ProcessGameOver();
            
            ScreenManager.AchievementManager.UnlockFirstInfiniteTanpopo();
        }
    }
}