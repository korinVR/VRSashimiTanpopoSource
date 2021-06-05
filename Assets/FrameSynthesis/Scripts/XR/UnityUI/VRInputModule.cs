using System.Collections.Generic;
using FrameSynthesis.XR.Debug;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace FrameSynthesis.XR.UnityUI
{
    public class VRInputModule : BaseInputModule
    {
        [Inject] DebugLogWindow debugLogWindow;
        [Inject] ICameraRig cameraRig;
        
        readonly List<RaycastResult> raycastResults = new List<RaycastResult>();

        enum State
        {
            Up,
            Hover,
            Down
        }

        Dictionary<GameObject, State> raycastedObjects;
        Dictionary<GameObject, State> prevRaycastedObjects = new Dictionary<GameObject, State>();

        public override void Process()
        {
            raycastedObjects = new Dictionary<GameObject, State>();

            if (!cameraRig.UsingHandTracking)
            {
                AddPointerDownObjects(cameraRig.GetTransform(TrackingPoint.LeftHand));
                AddPointerDownObjects(cameraRig.GetTransform(TrackingPoint.RightHand));
            }
            else
            {
                AddPointerDownObjects(cameraRig.GetHandBoneTransform(Hand.Left, HandBone.IndexTip));
                AddPointerDownObjects(cameraRig.GetHandBoneTransform(Hand.Right, HandBone.IndexTip));
            }
            
            UpdateDebugLogWindow();

            var pointerEventData = new VRPointerEventData(EventSystem.current, gameObject.transform.position);
            
            foreach (var raycastedObject in raycastedObjects)
            {
                var gameObject = raycastedObject.Key;
                var state = raycastedObject.Value;

                if (!prevRaycastedObjects.ContainsKey(gameObject)) continue;
                
                if (state == State.Down && prevRaycastedObjects[gameObject] == State.Hover)
                {
                    ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerDownHandler);
                    ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
                }
            }

            // foreach (var prevRaycastedObject in prevRaycastedObjects)
            // {
            //     var gameObject = prevRaycastedObject.Key;
            //     var state = prevRaycastedObject.Value;
            //     
            //     if (!raycastedObjects.ContainsKey(gameObject))
            //     {
            //         ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerUpHandler);
            //         continue;
            //     }
            //     
            //     if (state == State.Down && raycastedObjects[gameObject] == State.Hover)
            //     {
            //         ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerUpHandler);
            //         ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
            //     }
            // }

            prevRaycastedObjects = raycastedObjects;
        }

        void AddPointerDownObjects(Transform transform)
        {
            var pointerEventData = new VRPointerEventData(EventSystem.current, transform.position);

            eventSystem.RaycastAll(pointerEventData, raycastResults);

            foreach (var raycastResult in raycastResults)
            {
                if (raycastResult.gameObject.GetComponent<UnityEngine.UI.Button>() == null) continue;
                raycastedObjects[raycastResult.gameObject] = GetStateFromDistance(raycastResult.distance);
            }
        }

        static State GetStateFromDistance(float distance)
        {
            if (distance < 0f) return State.Down;
            if (distance > 0.1f) return State.Up;
            return State.Hover;
        }
 
        void UpdateDebugLogWindow()
        {
            if (debugLogWindow == null) return;
            
            debugLogWindow.Clear();
            foreach (var pointerDownObject in raycastedObjects)
            {
                debugLogWindow.Println(pointerDownObject.Key.name);
                debugLogWindow.Println(pointerDownObject.Value.ToString());
            }
        }
    }
}