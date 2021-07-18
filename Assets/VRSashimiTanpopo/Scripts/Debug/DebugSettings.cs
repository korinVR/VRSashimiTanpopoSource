using UnityEngine;

namespace VRSashimiTanpopo.Debug
{
    [CreateAssetMenu(menuName = "Frame Synthesis/DebugSettings")]
    public class DebugSettings : ScriptableObject
    {
        public bool EnableScreenShotButton;
        public bool ShortTimeLimit;

        [ContextMenu("初期状態に戻す")]
        public void Reset()
        {
            EnableScreenShotButton = false;
            ShortTimeLimit = false;
        }
    }
}
