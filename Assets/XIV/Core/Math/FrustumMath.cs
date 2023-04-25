using UnityEngine;

namespace XIV.XIVMath
{
    // https://docs.unity3d.com/Manual/FrustumSizeAtDistance.html
    public static class FrustumMath
    {
        public static Vector3 GetFrustum(float distance, float fieldOfView, float aspect)
        {
            var frustumHeight = 2f * distance * Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad);
            var frustumWidth = frustumHeight * aspect;
            return new Vector3(frustumWidth, frustumHeight, 1);
        }

        public static Vector3 GetFrustum(float distance)
        {
            var cam = Camera.main;
            return GetFrustum(distance, cam.fieldOfView, cam.aspect);
        }

        public static Vector3 GetFrustum(Camera cam, float distance)
        {
            return GetFrustum(distance, cam.fieldOfView, cam.aspect);
        }

        public static float GetFrustumDistance(float frustumHeight, float fieldOfView)
        {
            return frustumHeight * 0.5f / Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad);
        }

        public static float GetFrustumHeight(float distance, float fieldOfView)
        {
            return 2f * distance * Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad);
        }

        public static float GetFieldOfView(float frustumHeight, float distance)
        {
            return 2f * Mathf.Atan(frustumHeight * 0.5f / distance) * Mathf.Rad2Deg;
        }
    }
}