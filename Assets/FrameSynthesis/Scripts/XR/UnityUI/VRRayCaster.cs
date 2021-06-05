using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrameSynthesis.XR.UnityUI
{
    public class VRRayCaster : BaseRaycaster
    {
        const float HitDistance = 0.05f;

        // コントローラーがCanvasの面を突き抜けていても判定できるようにするためのオフセット
        const float RayOffset = 1;
        
        Canvas canvas;
        
        public override Camera eventCamera => canvas.worldCamera != null ? canvas.worldCamera : Camera.main;

        protected override void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            if (!(eventData is VRPointerEventData vrEventData)) return;

            var rayDirection = canvas.transform.forward;
            var ray = new Ray(vrEventData.WorldPosition - rayDirection * RayOffset, rayDirection);

            var graphics = GraphicRegistry.GetGraphicsForCanvas(canvas).ToList();
            foreach (var graphic in graphics)
            {
                if (!RayIntersectsRectTransform(graphic.rectTransform, ray, out var distance, out var worldPosition))
                    continue;

                resultAppendList.Add(new RaycastResult
                {
                    module = this,
                    distance = distance - RayOffset - HitDistance,
                    gameObject = graphic.gameObject,
                    worldPosition = worldPosition,
                });
            }
        }

        // ref. OVRRaycaster.cs
        static bool RayIntersectsRectTransform(RectTransform rectTransform, Ray ray, out float distance,
            out Vector3 worldPosition)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            var plane = new Plane(corners[0], corners[1], corners[2]);

            if (!plane.Raycast(ray, out var enter))
            {
                worldPosition = Vector3.zero;
                distance = 0f;
                return false;
            }

            var intersection = ray.GetPoint(enter);

            var bottomEdge = corners[3] - corners[0];
            var leftEdge = corners[1] - corners[0];
            var bottomDot = Vector3.Dot(intersection - corners[0], bottomEdge);
            var leftDot = Vector3.Dot(intersection - corners[0], leftEdge);
            if (bottomDot < bottomEdge.sqrMagnitude &&
                leftDot < leftEdge.sqrMagnitude &&
                bottomDot >= 0 &&
                leftDot >= 0)
            {
                worldPosition = corners[0] + leftDot * leftEdge / leftEdge.sqrMagnitude +
                                bottomDot * bottomEdge / bottomEdge.sqrMagnitude;
                distance = enter;
                return true;
            }

            worldPosition = Vector3.zero;
            distance = 0f;
            return false;
        }
    }
}