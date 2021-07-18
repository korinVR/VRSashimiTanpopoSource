using UnityEngine;
using UnityEngine.XR;
using VContainer;

namespace FrameSynthesis.XR
{
    public class ExternalDisplayCamera : MonoBehaviour
    {
        [Inject] IPlatform platform;

        void Start()
        {
            if (!platform.HasExternalDisplay)
            {
                GetComponent<Camera>().enabled = false;
                return;
            }

            DisableVRViewOnMainDisplay();
        }

        void DisableVRViewOnMainDisplay()
        {
#if PLATFORM_STEAMVR
            // SteamVRプラグインで XRSettings.gameViewRenderMode を使用してビルドするとクラッシュするのでdepthを使う。
            GetComponent<Camera>().depth = 1;
#endif
#if PLATFORM_RIFT
            // Oculus Integrationでは XRSettings.gameViewRenderMode を使用して非表示にする。
            XRSettings.gameViewRenderMode = GameViewRenderMode.None;
            // depthを1にしているとエディタ再生時にヘッドセット内の右目にこのカメラの映像が出てくる場合があるので、明示的に0に設定する。
            GetComponent<Camera>().depth = 0;
#endif
        }
    }
}
