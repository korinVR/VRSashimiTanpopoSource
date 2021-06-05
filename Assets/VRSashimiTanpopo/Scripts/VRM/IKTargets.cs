using FrameSynthesis.XR;
using UnityEngine;
using VContainer;

namespace VRSashimiTanpopo
{
    public class IKTargets : MonoBehaviour
    {
        public Transform HeadTarget => headTarget;
        public Transform LeftFootTarget => leftFootTarget;
        public Transform RightFootTarget => rightFootTarget;

        [SerializeField] Transform headTarget;
        [SerializeField] Transform leftFootTarget;
        [SerializeField] Transform rightFootTarget;

        [Inject] ICameraRig cameraRig;

        Vector3 firstPersonOffset;

        public void SetFirstPersonOffset(Vector3 firstPersonOffset)
        {
            this.firstPersonOffset = firstPersonOffset;
        }
        
        void Update()
        {
            var cameraTransform = cameraRig.GetTransform(TrackingPoint.Head);
            
            headTarget.position = cameraTransform.position;
            headTarget.rotation = cameraTransform.rotation;
            headTarget.Translate(-firstPersonOffset, Space.Self);
        }
    }
}
