using UnityEngine;

namespace XIV.Core.XIVMath
{
    public static class LineMath
    {
        public static bool IsPointOnTheLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point, float distanceThreshold = 0.1f)
        {
            var closestPoint = GetClosestPointOnLineSegment(lineStart, lineEnd, point);
            return Vector3.Distance(closestPoint, point) < distanceThreshold;
        }
    
        public static Vector3 GetClosestPointOnLineSegment(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 lineDirection = lineEnd - lineStart;
            float lineLength = lineDirection.magnitude;
            lineDirection /= lineLength;
 
            float dotProduct = Vector3.Dot(lineDirection, point - lineStart);
            dotProduct = Mathf.Clamp(dotProduct, 0f, lineLength);
 
            return lineStart + lineDirection * dotProduct;
        }
    }
}