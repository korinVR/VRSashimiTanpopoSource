using System;
using UnityEngine;

namespace FrameSynthesis.XR.Platform.WebXR
{
    public class WebXRCameraRig : MonoBehaviour, ICameraRig
    {
        public Transform GetTransform(TrackingPoint trackingPoint)
        {
            throw new NotImplementedException();
        }

        public float GetPinch(Hand hand)
        {
            throw new NotImplementedException();
        }

        public float GetFlex(Hand hand)
        {
            throw new NotImplementedException();
        }

        public bool GetButtonDown(Button button)
        {
            throw new NotImplementedException();
        }

        public bool UsingController => false;
        public bool UsingHandTracking => false;

        public Transform GetHandBoneTransform(Hand hand, HandBone handBone)
        {
            throw new NotImplementedException();
        }

        public bool GetNearTouch(Hand hand, NearTouchPoint nearTouchPoint) => false;

        public void PlayClickHaptics(Hand hand, float durationSec)
        {
            throw new NotImplementedException();
        }
    }
}
