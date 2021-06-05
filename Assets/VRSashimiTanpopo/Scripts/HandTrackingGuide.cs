using FrameSynthesis.XR;
using VContainer;

namespace VRSashimiTanpopo
{
    public class HandTrackingGuide
    {
        [Inject] VoicePlayer voicePlayer;
        [Inject] ICameraRig cameraRig;

        bool isFirstPlay = true;

        public void PlayGuidance()
        {
#if PLATFORM_QUEST
            if (!isFirstPlay) return;
            if (cameraRig.UsingHandTracking) return;
            
            voicePlayer.Play(Voice.HandTrackingGuide, delaySec: 3.5);
            isFirstPlay = false;
#endif
        }
    }
}