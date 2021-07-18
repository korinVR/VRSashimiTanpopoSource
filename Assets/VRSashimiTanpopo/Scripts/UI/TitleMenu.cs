using System;
using System.Linq;
using UnityEngine;
using VRSashimiTanpopo.VRM;

namespace VRSashimiTanpopo.UI
{
    public class TitleMenu : MonoBehaviour
    {
        enum Trigger
        {
            TopMenu,
            GameModeMenu,
            Credits,
            VRMLoad,
            Next,
        }
    
        public event Action<GameMode> Started;

        [SerializeField] Animator animator;

        VoicePlayer voicePlayer;
        SoundEffectPlayer soundEffectPlayer;
        VRMLoader vrmLoader;

        public void Construct(VoicePlayer voicePlayer, SoundEffectPlayer soundEffectPlayer, VRMLoader vrmLoader)
        {
            this.voicePlayer = voicePlayer;
            this.soundEffectPlayer = soundEffectPlayer;
            this.vrmLoader = vrmLoader;
        }

        void Start()
        {
            // アニメーションのデフォルト値を上書きする。バッドノウハウっぽいが……。
            // ref. https://tsubakit1.hateblo.jp/entry/2017/01/15/233000
            animator.gameObject.SetActive(false);
            GetComponentsInChildren<CanvasGroup>(includeInactive: true).ToList()
                .ForEach(canvasGroup =>
                {
                    canvasGroup.gameObject.SetActive(false);
                    canvasGroup.alpha = 0f;
                });
            animator.gameObject.SetActive(true);
        }

        public void StartSashimiTanpopo()
        {
            PlaySelectSound();
            voicePlayer.StopAll();
            voicePlayer.Play(Voice.SashimiTanpopo);
            Started?.Invoke(GameMode.SashimiTanpopo);
        }

        public void StartInfiniteTanpopo()
        {
            PlaySelectSound();
            voicePlayer.StopAll();
            voicePlayer.Play(Voice.InfiniteTanpopo);
            Started?.Invoke(GameMode.InfiniteTanpopo);
        }

        public void GoToTopMenu()
        {
            PlaySelectSound();
            TriggerAnimation(Trigger.TopMenu);
        }

        public void GoToGameModeMenu()
        {
            PlaySelectSound();
            voicePlayer.Play(Voice.SelectYourJob);
            TriggerAnimation(Trigger.GameModeMenu);
        }

        public void GoToCredits()
        {
            PlaySelectSound();
            TriggerAnimation(Trigger.Credits);
        }

        public void GoToNext()
        {
            PlaySelectSound();
            TriggerAnimation(Trigger.Next);
        }

        public async void GoToVRMLoadProcess()
        {
            PlaySelectSound();
            TriggerAnimation(Trigger.VRMLoad);
            
            vrmLoader.DestroyVRM();
            
            var isSuccess = await vrmLoader.OpenDialog();
            if (!isSuccess)
            {
                TriggerAnimation(Trigger.TopMenu);
                return;
            }
            TriggerAnimation(Trigger.Next);
        }

        public void LoadVRM()
        {
            vrmLoader.LoadVRM();
        }

        void PlaySelectSound() => soundEffectPlayer.Play(SoundEffect.MenuSelect);
        
        void TriggerAnimation(Trigger trigger)
        {
            animator.SetTrigger(trigger.ToString());
        }
    }
}
