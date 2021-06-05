using UnityEngine;

namespace FrameSynthesis
{
    public static class Vector3Extension
    {
        public static string ToStringF2(this Vector3 v) => $"({v.x:F2}, {v.y:F2}, {v.z:F2})";
    }
}
