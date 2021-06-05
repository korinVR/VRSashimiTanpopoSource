using UnityEngine;
using VContainer.Unity;

namespace FrameSynthesis.XR.Platform.Oculus
{
    public class OculusDisplayFader : IDisplayFader, ITickable
    {

        float brightness;
        float targetBrightness;
        float fadeDurationSec;
        
        public void ShowDisplay()
        {
            brightness = targetBrightness = 1f;
            UpdateColorScale();
        }

        public void HideDisplay()
        {
            brightness = targetBrightness = 0f;
            UpdateColorScale();
        }

        public void FadeIn(float durationSec)
        {
            fadeDurationSec = durationSec;
            targetBrightness = 1f;
        }

        public void FadeOut(float durationSec)
        {
            fadeDurationSec = durationSec;
            targetBrightness = 0f;
        }

        void UpdateColorScale()
        {
            OVRManager.SetColorScaleAndOffset(Color.white * brightness, Color.clear, true);
        }
        
        public void Tick()
        {
            brightness = Mathf.MoveTowards(brightness, targetBrightness, 1f / fadeDurationSec * Time.deltaTime);
            UpdateColorScale();
        }
    }
}
