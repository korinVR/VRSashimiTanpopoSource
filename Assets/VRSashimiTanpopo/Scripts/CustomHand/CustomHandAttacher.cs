using FrameSynthesis.XR;
using UnityEngine;
using VContainer;

namespace VRSashimiTanpopo.CustomHand
{
    public class CustomHandAttacher : MonoBehaviour
    {
        [SerializeField] TrackingPoint trackingPoint;
        
        ICameraRig cameraRig;
        
        [Inject]
        void Init(ICameraRig cameraRig)
        {
            this.cameraRig = cameraRig;
        }

        void Start()
        {
            transform.SetParent(cameraRig.GetTransform(trackingPoint), worldPositionStays:false);
        }
    }
}
