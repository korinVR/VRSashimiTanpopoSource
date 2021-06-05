using UnityEngine;

namespace VRSashimiTanpopo
{
    public enum SoundEffect
    {
        GameOver,
        Score,
        MenuSelect,
    }
    
    public class SoundEffectPlayer : MonoBehaviour
    {
        [SerializeField] AudioSource[] audioSources;

        public void Play(SoundEffect soundEffect)
        {
            audioSources[(int)soundEffect].Play();
        }
    }
}
