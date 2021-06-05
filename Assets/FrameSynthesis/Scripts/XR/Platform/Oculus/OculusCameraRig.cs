using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FrameSynthesis.XR.Platform.Oculus
{
    public class OculusCameraRig : MonoBehaviour, ICameraRig
    {
        [SerializeField] Transform head;
        [SerializeField] Transform leftController;
        [SerializeField] Transform rightController;
        [SerializeField] OVRHand leftHand;
        [SerializeField] OVRHand rightHand;

        readonly Dictionary<Hand, OVRInput.Controller> handToController = new Dictionary<Hand, OVRInput.Controller>
        {
            { Hand.Left, OVRInput.Controller.LTouch },
            { Hand.Right, OVRInput.Controller.RTouch },
        };

        readonly Dictionary<HandBone, OVRSkeleton.BoneId> handBoneToBoneId = new Dictionary<HandBone, OVRSkeleton.BoneId>
        {
            { HandBone.IndexTip, OVRSkeleton.BoneId.Hand_IndexTip },
            { HandBone.ThumbTip, OVRSkeleton.BoneId.Hand_ThumbTip },
        };

        readonly Dictionary<Button, OVRInput.RawButton> buttonToRawButton = new Dictionary<Button, OVRInput.RawButton>
        {
            { Button.Menu, OVRInput.RawButton.Start },
        };

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
            const float IgnorePinchingFlex = 0.2f;
            const float HandTrackingPinchBase = 0.7f;

            if (UsingController)
            {
                if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, handToController[hand]) >= IgnorePinchingFlex)
                {
                    return 0f;
                }
                return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, handToController[hand]);
            }

            if (UsingHandTracking)
            {
                var ovrHand = ToOVRHand(hand);

                return MathHelper.LinearClampedMap(
                    Mathf.Min(
                        ovrHand.GetFingerPinchStrength(OVRHand.HandFinger.Index),
                        ovrHand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb)),
                    HandTrackingPinchBase, 1f,
                    0f, 1f);
            }

            return 0f;
        }

        public float GetFlex(Hand hand) => OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, handToController[hand]);

        public bool GetButtonDown(Button button) => OVRInput.GetDown(buttonToRawButton[button]);

        public bool UsingController => OVRInput.IsControllerConnected(OVRInput.Controller.Touch);
        public bool UsingHandTracking => OVRInput.IsControllerConnected(OVRInput.Controller.Hands);

        public Transform GetHandBoneTransform(Hand hand, HandBone handBone)
        {
            if (!handBoneToBoneId.ContainsKey(handBone))
            {
                throw new NotImplementedException();
            }

            var indexTipBone = ToOVRHand(hand).GetComponent<OVRSkeleton>().Bones
                .FirstOrDefault(bone => bone.Id == handBoneToBoneId[handBone]);
            return indexTipBone?.Transform;
        }

        public bool GetNearTouch(Hand hand, NearTouchPoint nearTouchPoint)
        {
            switch (nearTouchPoint)
            {
                case NearTouchPoint.IndexTrigger:
                    return OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger, handToController[hand]);
                case NearTouchPoint.ThumbButtons:
                    return OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons, handToController[hand]);
            }

            throw new NotImplementedException();
        }

        public void PlayClickHaptics(Hand hand, float amplitude)
        {
            StartCoroutine(PlayClickHapticsCoroutine(handToController[hand], amplitude));
        }

        IEnumerator PlayClickHapticsCoroutine(OVRInput.Controller controller, float amplitude)
        {
            OVRInput.SetControllerVibration(0f, amplitude, controller);
            yield return null;
            yield return null;
            OVRInput.SetControllerVibration(0f, 0f, controller);
        }

        OVRHand ToOVRHand(Hand hand) => hand == Hand.Left ? leftHand : rightHand;
    }
}
