using UnityEngine;
using VContainer;

namespace FrameSynthesis.XR
{
    public class PauseOnInputFocusLost : MonoBehaviour
    {
        [Inject] IPlatform platform;

        void Start()
        {
            platform.InputFocusLost += OnInputFocusLost;
            platform.InputFocusAcquired += OnInputFocusAcquired;
        }

        void OnInputFocusLost()
        {
            Time.timeScale = 0f;
        }

        void OnInputFocusAcquired()
        {
            Time.timeScale = 1f;
        }
    }
}
