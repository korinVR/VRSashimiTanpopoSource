using FrameSynthesis.XR;
using UnityEngine;
using VContainer;

namespace VRSashimiTanpopo
{
    [RequireComponent(typeof(Grabber))]
    public class GrabberController : MonoBehaviour
    {
        [SerializeField] Hand hand;
        [SerializeField] Vector3 handTrackingPinchOffsetAngle;

        const float HandTrackingPinchThreshold = 0.2f;
        
        ICameraRig cameraRig;

        bool lastPinching;

        Grabber grabber;

        readonly FeatherTouchTrigger featherTouchTrigger = new FeatherTouchTrigger();

        [Inject]
        void Init(ICameraRig cameraRig)
        {
            this.cameraRig = cameraRig;

            grabber = GetComponent<Grabber>();
        }

        void Update()
        {
            var pinching = false;
            
            if (cameraRig.UsingController)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                
                pinching = featherTouchTrigger.UpdateState(cameraRig.GetPinch(hand));
            }
            if (cameraRig.UsingHandTracking)
            {
                // ハンドトラッキングなら親指と人差し指の中間点に追従させる。
                var indexTipTransform = cameraRig.GetHandBoneTransform(hand, HandBone.IndexTip);
                var thumbTipTransform = cameraRig.GetHandBoneTransform(hand, HandBone.ThumbTip);

                transform.position = Vector3.Lerp(indexTipTransform.position, thumbTipTransform.position, 0.5f);
                transform.rotation =
                    Quaternion.Slerp(indexTipTransform.rotation, thumbTipTransform.rotation, 0.5f) *
                    Quaternion.Euler(handTrackingPinchOffsetAngle);

                pinching = cameraRig.GetPinch(hand) > HandTrackingPinchThreshold;
            }

            if (pinching && !lastPinching)
            {
                if (grabber.Grab())
                {
                    cameraRig.PlayClickHaptics(hand, 0.5f);
                }
            }

            if (!pinching && lastPinching)
            {
                grabber.Release();
            }

            lastPinching = pinching;
        }
    }
}
