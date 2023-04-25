using UnityEngine;

namespace XIV.Core.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 OnXZ(this Vector3 vec3)
        {
            return new Vector3(vec3.x, 0f, vec3.z);
        }
        
        public static Vector3 SetX(this Vector3 vec3, float value)
        {
            return new Vector3(value, vec3.y, vec3.z);
        }
        
        public static Vector3 SetY(this Vector3 vec3, float value)
        {
            return new Vector3(vec3.x, value, vec3.z);
        }
        
        public static Vector3 SetZ(this Vector3 vec3, float value)
        {
            return new Vector3(vec3.x, vec3.y, value);
        }

        public static bool IsSameDirection(this Vector3 vec3, Vector3 other, float threshold = 0f)
        {
            return Vector3.Dot(vec3, other) > threshold;
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

        public static Vector3 Abs(this Vector3 vec3)
        {
            return new Vector3(Mathf.Abs(vec3.x), Mathf.Abs(vec3.y), Mathf.Abs(vec3.z));
        }

        public static Vector3 ClampMagnitude(this Vector3 vec3, float min, float max)
        {
            var magnitude = vec3.magnitude;
            if (magnitude < min) magnitude = min;
            if (magnitude > max) magnitude = max;
            return vec3.normalized * magnitude;
        }
    }
}