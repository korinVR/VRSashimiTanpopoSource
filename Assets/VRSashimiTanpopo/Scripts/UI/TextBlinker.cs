using TMPro;
using UnityEngine;

namespace VRSashimiTanpopo.UI
{
    public class TextBlinker : MonoBehaviour
    {
        [SerializeField] TMP_Text text;

        [SerializeField] float intervalSec;
        [SerializeField] float activeRate;

        float startTime;

        void OnEnable()
        {
            startTime = Time.time;
        }

        void OnDisable()
        {
            text.enabled = true;
        }

        void Update()
        {
            text.enabled = (Time.time - startTime) % intervalSec > intervalSec * (1 - activeRate);
        }
    }
}