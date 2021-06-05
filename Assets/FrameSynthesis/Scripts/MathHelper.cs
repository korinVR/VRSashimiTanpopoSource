using UnityEngine;

namespace FrameSynthesis
{
    public class MathHelper
    {

        public static Vector3 GetRandomPosition(Bounds bounds)
        {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }

        // see http://radiumsoftware.tumblr.com/post/5031889912
        public static Vector3 EaseOut(Vector3 current, Vector3 target, float factor, float deltaTime)
        {
            return current + (target - current) * (1f - Mathf.Exp(factor * deltaTime));
        }

        public static float EaseOut(float current, float target, float factor, float deltaTime)
        {
            return current + (target - current) * (1f - Mathf.Exp(factor * deltaTime));
        }

        public static float Approach(float current, float target, float delta)
        {
            if (current == target && delta >= 0)
            {
                return target;
            }

            var sign = Mathf.Sign(target - current);

            current += delta * sign;

            if (Mathf.Sign(target - current) != sign)
            {
                // Overran
                return target;
            }
            return current;
        }

        public static float LinearMap(float value, float s0, float s1, float d0, float d1)
        {
            return d0 + (value - s0) * (d1 - d0) / (s1 - s0);
        }

        public static float LinearClampedMap(float value, float s0, float s1, float d0, float d1)
        {
            if (d0 < d1)
            {
                return Mathf.Clamp(LinearMap(value, s0, s1, d0, d1), d0, d1);
            }
            return Mathf.Clamp(LinearMap(value, s0, s1, d0, d1), d1, d0);
        }
    }
}
