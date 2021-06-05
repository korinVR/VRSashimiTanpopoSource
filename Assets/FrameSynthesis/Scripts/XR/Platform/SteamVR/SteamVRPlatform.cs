using System;
using VContainer.Unity;
#if PLATFORM_STEAMVR
using Valve.VR;
#endif

namespace FrameSynthesis.XR.Platform.SteamVR
{
    public class SteamVRPlatform : IPlatform, IStartable, IDisposable
    {
        public event Action InputFocusLost;
        public event Action InputFocusAcquired;

        public bool HasExternalDisplay => true;

        public void Start()
        {
#if PLATFORM_STEAMVR
            SteamVR_Events.InputFocus.Listen(OnInputFocus);
#endif
        }

        public void Dispose()
        {
#if PLATFORM_STEAMVR
            SteamVR_Events.InputFocus.Remove(OnInputFocus);
#endif
        }

        void OnInputFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                InputFocusLost?.Invoke();
            }
            else
            {
                InputFocusAcquired?.Invoke();
            }
        }
    }
}
