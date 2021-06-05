#if PLATFORM_STEAMVR
using Valve.VR;
#endif
using VContainer.Unity;

namespace FrameSynthesis.XR.Platform.SteamVR
{
    public class SteamVRDisplayFader : IDisplayFader, ITickable
    {
        public void ShowDisplay()
        {
#if PLATFORM_STEAMVR
            OpenVR.Compositor.FadeToColor(0f, 0f, 0f, 0f, 0f, false);
#endif
        }

        public void HideDisplay()
        {
#if PLATFORM_STEAMVR
            OpenVR.Compositor.FadeToColor(0f, 0f, 0f, 0f, 1f, false);
#endif
        }

        public void FadeIn(float durationSec)
        {
#if PLATFORM_STEAMVR
            OpenVR.Compositor.FadeToColor(durationSec, 0f, 0f, 0f, 0f, false);
#endif
        }

        public void FadeOut(float durationSec)
        {
#if PLATFORM_STEAMVR
            OpenVR.Compositor.FadeToColor(durationSec, 0f, 0f, 0f, 1f, false);
#endif
        }

        public void Tick()
        {
        }
    }
}
