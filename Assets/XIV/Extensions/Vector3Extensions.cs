using UnityEngine;

namespace XIV.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 OnXZ(this Vector3 vec3)
        {
            return new Vector3(vec3.x, 0f, vec3.z);
        }

        public static float DistanceTo(this Vector3 vec3, Vector3 other)
        {
            return Vector3.Distance(vec3, other);
        }

        public static bool IsSameDirection(this Vector3 vec3, Vector3 other, float threshold = 0f)
        {
            return Vector3.Dot(vec3, other) > threshold;
        }

        public static float Dot(this Vector3 vec3, Vector3 other)
        {
            return Vector3.Dot(vec3, other);
        }
        
        public static Vector3 RotateAroundAxis(this Vector3 point, Vector3 axis, float angle, Vector3 pivot)
        {
            var translatedPoint = point - pivot;
            var rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, axis.normalized);
            var rotatedTranslatedPoint = rotation * translatedPoint;
            var rotatedPoint = rotatedTranslatedPoint + pivot;

            return rotatedPoint;
        }

        public static Vector3 RotateAroundX(this Vector3 point, float angle, Vector3 pivot)
        {
            return RotateAroundAxis(point, Vector3.right, angle, pivot);
        }

        public static Vector3 RotateAroundY(this Vector3 point, float angle, Vector3 pivot)
        {
            return RotateAroundAxis(point, Vector3.up, angle, pivot);
        }

        public static Vector3 RotateAroundZ(this Vector3 point, float angle, Vector3 pivot)
        {
            return RotateAroundAxis(point, Vector3.forward, angle, pivot);
        }
    }
}