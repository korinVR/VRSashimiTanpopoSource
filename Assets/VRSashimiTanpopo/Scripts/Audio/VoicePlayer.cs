using System;
using System.Collections.Generic;
using FrameSynthesis.XR;
using UnityEngine;
using VContainer;

namespace VRSashimiTanpopo
{
    public enum Voice
    {
        VRSashimiTanpopo,
        SelectYourJob,
        SashimiTanpopo,
        InfiniteTanpopo,
        Start1,
        Start2,
        TanpopoInstruction,
        WayToGo,
        GoodJob,
        AlmostFinished,
        OnlyOneTanpopoPlease,
        Finished1,
        HandTrackingGuide,
        Finished2,
        FrameSynthesis1,
        FrameSynthesis2,
    }
    
    public class VoicePlayer : MonoBehaviour
    {
        public class VoiceCommand : IComparable<VoiceCommand>
        {
            public double Time { get; }
            public Voice Voice { get; }
            public float Pitch { get; }

            public VoiceCommand(double time, Voice voice, float pitch)
            {
                Time = time;
                Voice = voice;
                Pitch = pitch;
            }

            public int CompareTo(VoiceCommand other) => Time.CompareTo(other.Time);
        }
        
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip[] audioClips;

        readonly List<VoiceCommand> voiceSequence = new List<VoiceCommand>();

        public void Play(Voice voice, double delaySec = 0, float pitch = 1f)
        {
            voiceSequence.Add(new VoiceCommand(Time.timeAsDouble + delaySec, voice, pitch));
            voiceSequence.Sort();
        }

        public void StopAll()
        {
            voiceSequence.Clear();
            audioSource.Stop();
        }

        void Update()
        {
            if (audioSource.isPlaying == false)
            {
                PlayNextVoice();
            }
        }

        void PlayNextVoice()
        {
            if (voiceSequence.Count == 0 || paused) return;
            
            var voiceCommand = voiceSequence[0];
            if (Time.timeAsDouble < voiceCommand.Time) return;
            
            audioSource.clip = audioClips[(int) voiceCommand.Voice];
            audioSource.pitch = voiceCommand.Pitch;
            audioSource.loop = false;
            audioSource.Play();
            
            voiceSequence.RemoveAt(0);
        }

        [Inject] IPlatform platform;

        bool paused;

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

        void OnInputFocusLost()
        {
            paused = true;
            audioSource.Pause();
        }
 
        void OnInputFocusAcquired()
        {
            paused = false;
            audioSource.UnPause();
        }
    }
}