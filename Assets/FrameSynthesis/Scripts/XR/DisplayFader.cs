using VContainer;

namespace FrameSynthesis.XR
{
    public class DisplayFader : IDisplayFader
    {
        [Inject] IDisplayFader vrDisplayFader;
        [Inject] ExternalDisplayFader externalDisplayFader;
        
        public void ShowDisplay()
        {
            vrDisplayFader.ShowDisplay();
            externalDisplayFader.ShowDisplay();
        }

        public void HideDisplay()
        {
            vrDisplayFader.HideDisplay();
            externalDisplayFader.HideDisplay();
        }

        public void FadeIn(float durationSec = 0.5f)
        {
            vrDisplayFader.FadeIn(durationSec);
            externalDisplayFader.FadeIn(durationSec);
        }

        public void FadeOut(float durationSec = 0.5f)
        {
            vrDisplayFader.FadeOut(durationSec);
            externalDisplayFader.FadeOut(durationSec);
        }
    }
}
