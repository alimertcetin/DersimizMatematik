﻿using System.Collections.Generic;
using UnityEngine;
using XIV.XIVMath;

namespace XIV
{
#if UNITY_EDITOR
    
    public static class XIVDebug
    {
        const float TAU = 6.283185307179586f;
        
        static readonly Color DefaultCircleColor = new Color(0f, 0f, 1f, 1f); // Same as Color.blue
        static readonly int DefaultCircleDetail = 10;

        static readonly Color DefaultSphereColor = new Color(1f, 0f, 0f, 1f); // Same as Color.red
        static readonly int DefaultSphereDetail = 20;
        
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
            DrawBezier(points[0],points[1],points[2],points[3], color, duration, steps);
        }
        
        public static void DrawSpline(IList<Vector3> points, Color color, float duration, int steps)
        {
            var p1 = points[0];
            for (int i = 1; i <= steps; i++)
            {
                float t = i / (float)steps;
                var p2 = SplineMath.GetPoint(points, t);
                Debug.DrawLine(p1, p2, color, duration);
                p1 = p2;
            }
        }
        
        // Sphere
        public static void DrawSphere(Vector3 position, float radius, float duration = 0)
        {
            DrawSphere(position, radius, DefaultSphereColor, DefaultSphereDetail, DefaultCircleDetail, duration);
        }
        
        public static void DrawSphere(Vector3 position, float radius, Color color, float duration = 0)
        {
            DrawSphere(position, radius, color, DefaultSphereDetail, DefaultCircleDetail, duration);
        }
        
        public static void DrawSphere(Vector3 position, float radius, Color color, int detail, int circleDetail, float duration = 0)
        {
            for (int i = 0; i < detail; i++)
            {
                var angle = i * (TAU / detail);
                var axis = Vector3.RotateTowards(Vector3.forward, Vector3.back, angle, 180f);
                DrawCircle(position, radius, axis, color, circleDetail, duration);
            }
        }

        // Circle
        public static void DrawCircle(Vector3 position, float radius, float duration = 0)
        {
            DrawCircle(position, radius, Vector3.forward, DefaultCircleColor, DefaultCircleDetail, duration);
        }

        public static void DrawCircle(Vector3 position, float radius, Vector3 axis, float duration = 0)
        {
            DrawCircle(position, radius, axis, DefaultCircleColor, DefaultCircleDetail, duration);
        }

        public static void DrawCircle(Vector3 position, float radius, Vector3 axis, Color color, float duration = 0)
        {
            DrawCircle(position, radius, axis, color, DefaultCircleDetail, duration);
        }

        public static void DrawCircle(Vector3 position, float radius, Vector3 axis, Color color, int detail, float duration = 0)
        {
            var rotation = Quaternion.FromToRotation(Vector3.forward, axis);
            var startPoint = position + rotation * Vector3.right * radius;
            var p1 = startPoint;
            for (int i = 1; i <= detail; i++)
            {
                float angle = i * (360f / detail);
                var p2 = position + rotation * Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right * radius;
                /*
                 * Using TAU
                 * float angle = i * (TAU / detail);
                 * var p2 = position + rotation * Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward) * Vector3.right * radius;
                 * Conversion is necessary
                 */
                Debug.DrawLine(p1, p2, color, duration);
                p1 = p2;
            }
            
            Debug.DrawLine(p1, startPoint, color, duration);
        }
    }
    
#endif
}