using FrameSynthesis.XR;
using UnityEngine;
using VContainer;

namespace VRSashimiTanpopo
{
    public enum Music
    {
        Title,
        ThemeOfSashimiTanpopo,
        ThemeOfInfiniteTanpopo,
        GameOver,
    }
    
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip[] audioClips;
        
        public void Play(Music music)
        {
            audioSource.clip = audioClips[(int)music];
            audioSource.loop = music == Music.ThemeOfInfiniteTanpopo;
            
            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.Stop();
        }

        [Inject] IPlatform platform;

        void OnEnable()
        {
            platform.InputFocusLost += OnInputFocusLost;
            platform.InputFocusAcquired += OnInputFocusAcquired;
        }

        void OnDisable()
        {
            platform.InputFocusLost -= OnInputFocusLost;
            platform.InputFocusAcquired -= OnInputFocusAcquired;
        }

        bool playingOnInputFocusLost;

        void OnInputFocusLost()
        {
            playingOnInputFocusLost = audioSource.isPlaying;
            audioSource.Pause();
        }

        void OnInputFocusAcquired()
        {
            if (playingOnInputFocusLost)
            {
                audioSource.Play();
            }
        }
    }
}
