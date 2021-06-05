using UnityEngine;
using WebXR;

namespace FrameSynthesis.XR.Platform.WebXR
{
    public class HandAnimation : MonoBehaviour
    {
        WebXRController controller;
        Animator anim;

        void Awake()
        {
            anim = GetComponent<Animator>();
            controller = GetComponent<WebXRController>();
        }

        void Update()
        {
            var normalizedTime = controller.GetButton("Trigger") ? 1 : controller.GetAxis("Grip");

            // Use the controller button or axis position to manipulate the playback time for hand model.
            anim.Play("Take", -1, normalizedTime);
        }
    }
}
