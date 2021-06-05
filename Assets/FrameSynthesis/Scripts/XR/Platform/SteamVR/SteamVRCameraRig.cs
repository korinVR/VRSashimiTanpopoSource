using System;
using System.Collections.Generic;
using UnityEngine;
#if PLATFORM_STEAMVR
using Valve.VR;
#endif

namespace FrameSynthesis.XR.Platform.SteamVR
{
    public class SteamVRCameraRig : MonoBehaviour, ICameraRig
    {
        [SerializeField] Transform head;
        [SerializeField] Transform leftController;
        [SerializeField] Transform rightController;

#if PLATFORM_STEAMVR
        readonly Dictionary<Hand, SteamVR_Input_Sources> handToInputSource = new Dictionary<Hand, SteamVR_Input_Sources>
        {
            { Hand.Left, SteamVR_Input_Sources.LeftHand },
            { Hand.Right, SteamVR_Input_Sources.RightHand },
        };
#endif

        public Transform GetTransform(TrackingPoint trackingPoint)
        {
            switch (trackingPoint)
            {
                case TrackingPoint.Head:
                    return head;
                case TrackingPoint.LeftHand:
                    return leftController;
                case TrackingPoint.RightHand:
                    return rightController;
            }

            throw new NotImplementedException();
        }

        public float GetPinch(Hand hand)
        {
#if PLATFORM_STEAMVR
            return SteamVR_Actions.default_Pinch.GetAxis(handToInputSource[hand]);
#else
            return 0;
#endif
        }

        public float GetFlex(Hand hand)
        {
#if PLATFORM_STEAMVR
            return SteamVR_Actions.default_Flex.GetAxis(handToInputSource[hand]);
#else
            return 0;
#endif
        }

        public bool GetButtonDown(Button button)
        {
#if PLATFORM_STEAMVR
            switch (button)
            {
                case Button.Menu:
                    return SteamVR_Actions.default_Menu.GetStateDown(SteamVR_Input_Sources.LeftHand);
            }
#endif

            throw new NotImplementedException();
        }

        public bool UsingController => true;
        public bool UsingHandTracking => false;

        public Transform GetHandBoneTransform(TrackingPoint trackingPoint, HandBone handBone)
        {
            throw new NotImplementedException();
        }

        public Transform GetHandBoneTransform(Hand hand, HandBone handBone)
        {
            throw new NotImplementedException();
        }

        public bool GetNearTouch(Hand hand, NearTouchPoint nearTouchPoint) => true;

        public void PlayClickHaptics(Hand hand, float amplitude)
        {
#if PLATFORM_STEAMVR
            SteamVR_Actions.default_Haptic.Execute(0, 0.01f, 100, amplitude, handToInputSource[hand]);
#endif
        }
    }
}
