using System.IO;
using UnityEngine;
using VContainer;

namespace VRSashimiTanpopo.Debug
{
    public class ScreenShotButton : MonoBehaviour
    {
        const string BaseDirectory = "ScreenShots";
        const string BaseFilename = "ScreenShot";

        [Inject] DebugSettings debugSettings;

        int captureCount;

        void Update()
        {
            if (!debugSettings.EnableScreenShotButton) return;
            
#if PLATFORM_OCULUS            
            if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
                Directory.CreateDirectory(BaseDirectory);

                var filename = Path.Combine(BaseDirectory, $"{BaseFilename}{captureCount++:00}.png");
                ScreenCapture.CaptureScreenshot(filename);
                
                UnityEngine.Debug.Log("Saved " + filename);
            }
#endif
        }
    }
}
