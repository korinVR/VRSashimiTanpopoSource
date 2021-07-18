using UnityEngine;

namespace FrameSynthesis.XR
{
    public class ExternalDisplayFader : MonoBehaviour, IDisplayFader
    {
        float brightness;
        float targetBrightness;
        float fadeDurationSec;

        Texture2D fadeTexture;

        void Start()
        {
            fadeTexture = new Texture2D(1, 1);
        }

        public void ShowDisplay()
        {
            brightness = targetBrightness = 1f;
        }

        public void HideDisplay()
        {
            brightness = targetBrightness = 0f;
        }

        public void FadeIn(float durationSec = 0.5f)
        {
            fadeDurationSec = durationSec;
            targetBrightness = 1f;
        }

        public void FadeOut(float durationSec = 0.5f)
        {
            fadeDurationSec = durationSec;
            targetBrightness = 0f;
        }

        void OnGUI()
        {
            brightness = Mathf.MoveTowards(brightness, targetBrightness, 1f / fadeDurationSec * Time.deltaTime);
            
            if (Mathf.Approximately(brightness, 1f)) return;
            
            fadeTexture.SetPixel(0, 0, new Color(0, 0, 0, 1 - brightness));
            fadeTexture.Apply();
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
        }
    }
}
