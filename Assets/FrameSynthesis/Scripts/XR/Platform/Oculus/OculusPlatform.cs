using System;
using System.Linq;
using UnityEngine;
using VContainer.Unity;

namespace FrameSynthesis.XR.Platform.Oculus
{
    public class OculusPlatform : IPlatform, IStartable, ITickable, IDisposable
    {
        public event Action InputFocusLost;
        public event Action InputFocusAcquired;

        public bool HasExternalDisplay
        {
            get
            {
#if PLATFORM_QUEST
                return false;
#else
                return true;
#endif
            }
        }

        public void Start()
        {
            OVRManager.InputFocusLost += OnInputFocusLost;
            OVRManager.InputFocusAcquired += OnInputFocusAcquired;

            if (OVRManager.systemHeadsetType == OVRManager.SystemHeadsetType.Oculus_Quest)
            {
                // Oculus Quest 1
                OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.Medium;
                OVRManager.gpuLevel = 3;
            }
            else
            {
                // Oculus Quest 2以降
                if (OVRManager.display.displayFrequenciesAvailable.Contains(90f))
                {
                    OVRManager.display.displayFrequency = 90f;
                }
            }
        }

        public void Tick()
        {
            Time.fixedDeltaTime = 1f / OVRManager.display.displayFrequency;
        }

        public void Dispose()
        {
            OVRManager.InputFocusLost -= OnInputFocusLost;
            OVRManager.InputFocusAcquired -= OnInputFocusAcquired;
        }

        void OnInputFocusLost()
        {
            InputFocusLost?.Invoke();
        }

        void OnInputFocusAcquired()
        {
            InputFocusAcquired?.Invoke();
        }
    }
}
