using UnityEngine;

namespace VRSashimiTanpopo
{
    public class ApplicationQuitKey : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
