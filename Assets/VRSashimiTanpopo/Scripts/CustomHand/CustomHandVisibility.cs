using System;
using FrameSynthesis.XR;
using UnityEngine;
using VContainer;

namespace VRSashimiTanpopo.CustomHand
{
    public class CustomHandVisibility : MonoBehaviour
    {
        ICameraRig cameraRig;

        Renderer[] renderers;

        [Inject]
        void Init(ICameraRig cameraRig)
        {
            this.cameraRig = cameraRig;
        }

        void Start()
        {
            renderers = GetComponentsInChildren<Renderer>();
        }

        void LateUpdate()
        {
            Array.ForEach(renderers, r => r.enabled = cameraRig.UsingController);
        }
    }
}
