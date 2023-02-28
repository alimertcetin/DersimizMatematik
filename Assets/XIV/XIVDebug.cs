using System.Collections.Generic;
using UnityEngine;
using XIV.XIVMath;

namespace XIV
{
    public static class XIVDebug
    {
        public static void DrawBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Color color, float duration, int steps)
        {
            var point1 = p0;
            for (int i = 1; i <= steps; i++)
            {
                float t = i / (float)steps;
                var point2 = BezierMath.GetPoint(p0, p1, p2, p3, t);
                Debug.DrawLine(point1, point2, color, duration);
                point1 = point2;
            }
        }
        
        public static void DrawBezier(IList<Vector3> points, Color color, float duration, int steps)
        {
            int i = 0;
            DrawBezier(points[i++],points[i++],points[i++],points[i], color, duration, steps);
        }
        
        public static void DrawSpline(IList<Vector3> points, Color color, float duration, int steps)
        {
            var point1 = points[0];
            for (int i = 1; i <= steps; i++)
            {
                float t = i / (float)steps;
                var point2 = SplineMath.GetPoint(points, t);
                Debug.DrawLine(point1, point2, color, duration);
                point1 = point2;
            }
        }
    }
}