namespace FrameSynthesis.XR
{
    public interface IDisplayFader
    {
        void ShowDisplay();
        void HideDisplay();

        void FadeIn(float durationSec = 0.5f);
        void FadeOut(float durationSec = 0.5f);
    }
}
