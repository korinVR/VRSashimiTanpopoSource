using UnityEngine;
using WebXR;

namespace FrameSynthesis.XR.Platform.WebXR
{
    public class WebXRHand : MonoBehaviour
    {
        [SerializeField]
        Transform grabPoint;

        [SerializeField]
        Transform marker;

        bool lastPinching;

        WebXRController controller;

        void Awake()
        {
            controller = GetComponent<WebXRController>();
        }

        void Update()
        {
            if (marker == null) return;

            marker.transform.position = grabPoint.position;
            marker.transform.rotation = grabPoint.rotation;

            var pinching = controller.GetButton("Trigger") ? true : controller.GetAxis("Grip") > 0.5f;

            if (pinching && !lastPinching)
            {
                marker.GetComponentInChildren<Grabber>().Grab();
            }

            if (!pinching && lastPinching)
            {
                marker.GetComponentInChildren<Grabber>().Release();
            }

            lastPinching = pinching;
        }
    }
}
