using UnityEngine;

namespace VRSashimiTanpopo.UI
{
    public class LoadVRMButton : MonoBehaviour
    {
        void Start()
        {
#if PLATFORM_QUEST
            gameObject.SetActive(false);
#endif
        }
    }
}
