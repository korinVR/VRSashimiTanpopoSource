using UnityEngine;

namespace VRSashimiTanpopo
{
    public class TanpopoAlertSound : MonoBehaviour
    {
        bool playing;

        [SerializeField] AudioSource audioSource;

        public void Play()
        {
            if (playing) return;

            audioSource.Play();
            playing = true;
        }

        public void Stop()
        {
            if (!playing) return;

            audioSource.Stop();
            playing = false;
        }
    }
}
