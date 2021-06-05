using UnityEngine;
using UnityEngine.EventSystems;

namespace FrameSynthesis.XR.UnityUI
{
    public class VRPointerEventData : PointerEventData
    {
        public Vector3 WorldPosition { get; }

        public VRPointerEventData(EventSystem eventSystem, Vector3 worldPosition) : base(eventSystem)
        {
            WorldPosition = worldPosition;
        }
    }
}