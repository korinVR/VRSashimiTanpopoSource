using UnityEngine;

namespace FrameSynthesis.XR
{
    public interface ICameraRig
    {
        Transform GetTransform(TrackingPoint trackingPoint);

        float GetPinch(Hand hand);
        float GetFlex(Hand hand);

        bool GetButtonDown(Button button);

        bool UsingController { get; }
        bool UsingHandTracking { get; }

        Transform GetHandBoneTransform(Hand hand, HandBone handBone);

        bool GetNearTouch(Hand hand, NearTouchPoint nearTouchPoint);

        void PlayClickHaptics(Hand hand, float amplitude);
    }
}
