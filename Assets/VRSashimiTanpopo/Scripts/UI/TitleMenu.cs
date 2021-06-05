using System;
using UnityEngine;
using UnityEngine.UI;

namespace VRSashimiTanpopo.UI
{
    public class TitleMenu : MonoBehaviour
    {
        enum Menu
        {
            Start,
            TopMenu,
            GameModeMenu,
            Credits,
        }
    
        public event Action<GameMode> Started;

        [SerializeField] Animator animator;

        [SerializeField] Button startButton;
        [SerializeField] Button creditsButton;

        [SerializeField] Button sashimiTanpopoButton;
        [SerializeField] Button infiniteTanpopoButton;
        [SerializeField] Button gameModeMenuBackButton;
        [SerializeField] Button creditsBackButton;
        
        VoicePlayer voicePlayer;
        SoundEffectPlayer soundEffectPlayer;

        public void Construct(VoicePlayer voicePlayer, SoundEffectPlayer soundEffectPlayer)
        {
            this.voicePlayer = voicePlayer;
            this.soundEffectPlayer = soundEffectPlayer;
        }

        void Start()
        {
            startButton.onClick.AddListener(() =>
            {
                soundEffectPlayer.Play(SoundEffect.MenuSelect);
                voicePlayer.Play(Voice.SelectYourJob);
                ChangeMenu(Menu.GameModeMenu);
            });
            
            creditsButton.onClick.AddListener(() =>
            {
                soundEffectPlayer.Play(SoundEffect.MenuSelect);
                ChangeMenu(Menu.Credits);
            });
            
            gameModeMenuBackButton.onClick.AddListener(() =>
            {
                soundEffectPlayer.Play(SoundEffect.MenuSelect);
                ChangeMenu(Menu.TopMenu);
            });
            
            creditsBackButton.onClick.AddListener(() =>
            {
                soundEffectPlayer.Play(SoundEffect.MenuSelect);
                ChangeMenu(Menu.TopMenu);
            });
            
            sashimiTanpopoButton.onClick.AddListener(() =>
            {
                soundEffectPlayer.Play(SoundEffect.MenuSelect);
                voicePlayer.StopAll();
                voicePlayer.Play(Voice.SashimiTanpopo);
                Started?.Invoke(GameMode.SashimiTanpopo);
            });
            
            infiniteTanpopoButton.onClick.AddListener(() =>
            {
                soundEffectPlayer.Play(SoundEffect.MenuSelect);
                voicePlayer.StopAll();
                voicePlayer.Play(Voice.InfiniteTanpopo);
                Started?.Invoke(GameMode.InfiniteTanpopo);
            });
        }

        public void ShowTopMenu()
        {
            ChangeMenu(Menu.TopMenu);
        }

        void ChangeMenu(Menu menu)
        {
            animator.SetInteger("CurrentMenu", (int) menu);
        }
    }
}
