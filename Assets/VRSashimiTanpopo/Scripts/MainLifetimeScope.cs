using System;
using FrameSynthesis.XR;
using FrameSynthesis.XR.Debug;
using FrameSynthesis.XR.UnityUI;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VRSashimiTanpopo.Achievements;
using VRSashimiTanpopo.Debug;
using VRSashimiTanpopo.Model;
using VRSashimiTanpopo.Screen;
using VRSashimiTanpopo.UI;
using VRSashimiTanpopo.VRM;

namespace VRSashimiTanpopo
{
    class MainLifetimeScope : LifetimeScope
    {
        [Header("Settings")]
        [SerializeField] DebugSettings debugSettings;

        [Header("UI")]
        [SerializeField] TitleMenu titleMenuPrefab;
        [SerializeField] ResetButtonCanvas resetButtonCanvasPrefab;
        [SerializeField] GameStartMessage gameStartMessagePrefab;
        
        [SerializeField] LifetimeScope sashimiTanpopoScoreBoardPrefab;
        [SerializeField] LifetimeScope infiniteTanpopoScoreBoardPrefab;

        [Header("In Game")]
        [SerializeField] GameObject tanpopoPrefab;
        [SerializeField] GameObject sashimiPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
#if PLATFORM_OCULUS
            builder.RegisterEntryPoint<FrameSynthesis.XR.Platform.Oculus.OculusPlatform>();
            builder.RegisterEntryPoint<FrameSynthesis.XR.Platform.Oculus.OculusDisplayFader>();
            Instantiate(Resources.Load("OculusCameraRig"));
            builder.RegisterComponentInHierarchy<FrameSynthesis.XR.Platform.Oculus.OculusCameraRig>().As<ICameraRig>();
#if UNITY_EDITOR
            builder.RegisterEntryPoint<NullAchievementManager>().As<IAchievementManager>();
#else
            builder.RegisterEntryPoint<NullAchievementManager>().As<IAchievementManager>();
            // builder.RegisterEntryPoint<OculusAchievementManager>().As<IAchievementManager>();
#endif
#endif
#if PLATFORM_STEAMVR
            builder.RegisterEntryPoint<FrameSynthesis.XR.Platform.SteamVR.SteamVRPlatform>();
            builder.RegisterEntryPoint<FrameSynthesis.XR.Platform.SteamVR.SteamVRDisplayFader>();
            Instantiate(Resources.Load("SteamVRCameraRig"));
            builder.RegisterComponentInHierarchy<FrameSynthesis.XR.Platform.SteamVR.SteamVRCameraRig>().As<ICameraRig>();

            builder.RegisterEntryPoint<NullAchievementManager>().As<IAchievementManager>();
#endif
#if PLATFORM_WEBXR
            builder.RegisterEntryPoint<FrameSynthesis.XR.Platform.WebXR.WebXRPlatform>();
            Instantiate(Resources.Load("WebXRCameraRig"));
            builder.RegisterComponentInHierarchy<FrameSynthesis.XR.Platform.WebXR.WebXRCameraRig>().As<ICameraRig>();

            builder.RegisterEntryPoint<NullAchievementManager>().As<IAchievementManager>();
#endif

            builder.RegisterComponentInHierarchy<MusicPlayer>();
            builder.RegisterComponentInHierarchy<VoicePlayer>();
            builder.RegisterComponentInHierarchy<SoundEffectPlayer>();

            builder.RegisterComponentInHierarchy<DebugLogWindow>();

            builder.RegisterComponentInHierarchy<PauseOnInputFocusLost>();
            builder.RegisterComponentInHierarchy<LiveCamera>();
            builder.RegisterComponentInHierarchy<ScreenShotButton>();
            builder.RegisterComponentInHierarchy<VRInputModule>();

            builder.RegisterComponentInHierarchy<VRMLoader>();
            builder.RegisterComponentInHierarchy<IKTargets>();
            
            builder.RegisterInstance(debugSettings);
            
            builder.Register<SceneLoader>(Lifetime.Singleton);
            builder.RegisterEntryPoint<ScreenManager>();

            builder.Register<HandTrackingGuide>(Lifetime.Singleton);
            
            RegisterModels(builder);
            RegisterScreens(builder);
            RegisterUIs(builder);
        }

        void RegisterModels(IContainerBuilder builder)
        {
            builder.Register<GameModel>(Lifetime.Singleton);
            builder.Register<ScoreModel>(Lifetime.Singleton);
            builder.Register<CountdownTimerModel>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<WorkingTimeModel>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        void RegisterScreens(IContainerBuilder builder)
        {
            builder.RegisterFactory<TitleScreen>(container =>
            {
                var titleMenuFactory = container.Resolve<Func<TitleMenu>>();
                return () => new TitleScreen(titleMenuFactory);
            }, Lifetime.Scoped);

            builder.RegisterFactory<SashimiTanpopoGameScreen>(container =>
            {
                var debugSettings = container.Resolve<DebugSettings>();
                var handTrackingGuide = container.Resolve<HandTrackingGuide>();
                return () => new SashimiTanpopoGameScreen(sashimiPrefab, tanpopoPrefab, this, sashimiTanpopoScoreBoardPrefab, debugSettings, handTrackingGuide);
            }, Lifetime.Scoped);

            builder.RegisterFactory<InfiniteTanpopoGameScreen>(container =>
            {
                return () => new InfiniteTanpopoGameScreen(sashimiPrefab, tanpopoPrefab, this, infiniteTanpopoScoreBoardPrefab);
            }, Lifetime.Scoped);
        }

        void RegisterUIs(IContainerBuilder builder)
        {
            builder.RegisterFactory<TitleMenu>(container =>
            {
                var voicePlayer = container.Resolve<VoicePlayer>();
                var soundEffectPlayer = container.Resolve<SoundEffectPlayer>();
                return () =>
                {
                    var titleMenu = Instantiate(titleMenuPrefab);
                    titleMenu.Construct(voicePlayer, soundEffectPlayer);
                    return titleMenu;
                };
            }, Lifetime.Scoped);

            builder.RegisterFactory<Action, ResetButtonCanvas>(container =>
            {
                return resetButtonPressed =>
                {
                    var resetButtonCanvas = Instantiate(resetButtonCanvasPrefab);
                    resetButtonCanvas.Construct(resetButtonPressed);
                    return resetButtonCanvas;
                };
            }, Lifetime.Scoped);

            builder.RegisterFactory<GameStartMessage>(container =>
            {
                return () =>
                {
                    var gameStartMessage = Instantiate(gameStartMessagePrefab);
                    return gameStartMessage;
                };
            }, Lifetime.Scoped);
        }
    }
}
