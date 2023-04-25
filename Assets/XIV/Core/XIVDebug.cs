using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XIV.Core
{
    using Collections;
    using Utils;
    using XIVMath;
    
#if UNITY_EDITOR
    
    public static class XIVDebug
    {
        const float TAU = 6.283185307179586f;
        
        static readonly Color DefaultBezierColor = new Color(1f, 1f, 1f, 1f); // Same as Color.white
        const int DEFAULT_BEZIER_DETAIL = 20;
        
        static readonly Color DefaultCircleColor = new Color(0f, 0f, 1f, 1f); // Same as Color.blue
        const int DEFAULT_CIRCLE_DETAIL = 10;

        static readonly Color DefaultSphereColor = new Color(1f, 0f, 0f, 1f); // Same as Color.red
        const int DEFAULT_SPHERE_DETAIL = 20;
        
        // Line
        public static void DrawLine(Vector3 from, Vector3 to, float duration = 0f)
        {
            Debug.DrawLine(from, to, Color.white, duration);
        }
        
        public static void DrawLine(Vector3 from, Vector3 to, Color color, float duration = 0f)
        {
            Debug.DrawLine(from, to, color, duration);
        }
        
        public static void DrawLine(Vector3 from, Vector3 to, Color color, bool depthTest, float duration = 0f)
        {
            Debug.DrawLine(from, to, color, duration, depthTest);
        }

        // Bezier
        public static void DrawBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Color color, int detail, float duration = 0f)
        {
            var point1 = p0;
            for (int i = 1; i <= detail; i++)
            {
                float t = i / (float)detail;
                var point2 = BezierMath.GetPoint(p0, p1, p2, p3, t);
                Debug.DrawLine(point1, point2, color, duration);
                point1 = point2;
            }
        }
        
        public static void DrawBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Color color, float duration = 0f)
        {
            DrawBezier(p0, p1, p2, p3, color, DEFAULT_BEZIER_DETAIL, duration);
        }
        
        public static void DrawBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float duration = 0f)
        {
            DrawBezier(p0, p1, p2, p3, DefaultBezierColor, DEFAULT_BEZIER_DETAIL, duration);
        }
        
        public static void DrawBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t, float duration = 0f)
        {
            DrawBezier(p0, p1, p2, p3, DefaultBezierColor, DEFAULT_BEZIER_DETAIL, duration);
            var current = BezierMath.GetPoint(p0, p1, p2, p3, t);
            DrawSphere(current, 0.2f, Color.red, duration);
        }
        
        // Spline
        public static void DrawSpline(IList<Vector3> points, Color color, int detail, float duration = 0f)
        {
            var p1 = points[0];
            for (int i = 1; i <= detail; i++)
            {
                float t = i / (float)detail;
                var p2 = SplineMath.GetPoint(points, t);
                Debug.DrawLine(p1, p2, color, duration);
                p1 = p2;
            }
        }
        
        // Sphere
        public static void DrawSphere(Vector3 position, float radius, Color color, int detail, int circleDetail, float duration = 0)
        {
            for (int i = 0; i < detail; i++)
            {
                var angle = i * (TAU / detail);
                var axis = Vector3.RotateTowards(Vector3.forward, Vector3.back, angle, 180f);
                DrawCircle(position, radius, axis, color, circleDetail, duration);
            }
        }
        
        public static void DrawSphere(Vector3 position, float radius, float duration = 0)
        {
            DrawSphere(position, radius, DefaultSphereColor, DEFAULT_SPHERE_DETAIL, DEFAULT_CIRCLE_DETAIL, duration);
        }
        
        public static void DrawSphere(Vector3 position, float radius, Color color, float duration = 0)
        {
            DrawSphere(position, radius, color, DEFAULT_SPHERE_DETAIL, DEFAULT_CIRCLE_DETAIL, duration);
        }
        
        // Circle
        public static void DrawCircle(Vector3 position, float radius, Vector3 axis, Color color, int detail, float duration = 0)
        {
            radius *= 0.5f;
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
        
        public static void DrawCircle(Vector3 position, float radius, float duration = 0)
        {
            DrawCircle(position, radius, Vector3.forward, DefaultCircleColor, DEFAULT_CIRCLE_DETAIL, duration);
        }

        public static void DrawCircle(Vector3 position, float radius, Vector3 axis, float duration = 0)
        {
            DrawCircle(position, radius, axis, DefaultCircleColor, DEFAULT_CIRCLE_DETAIL, duration);
        }

        public static void DrawCircle(Vector3 position, float radius, Vector3 axis, Color color, float duration = 0)
        {
            DrawCircle(position, radius, axis, color, DEFAULT_CIRCLE_DETAIL, duration);
        }
        
        // Bounds
        public static void DrawBounds(Bounds bounds, float duration = 0f)
        {
            // bottom
            var p1 = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
            var p2 = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            var p3 = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            var p4 = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);

            Debug.DrawLine(p1, p2, Color.blue, duration);
            Debug.DrawLine(p2, p3, Color.red, duration);
            Debug.DrawLine(p3, p4, Color.yellow, duration);
            Debug.DrawLine(p4, p1, Color.magenta, duration);

            // top
            var p5 = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            var p6 = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
            var p7 = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
            var p8 = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);

            Debug.DrawLine(p5, p6, Color.blue, duration);
            Debug.DrawLine(p6, p7, Color.red, duration);
            Debug.DrawLine(p7, p8, Color.yellow, duration);
            Debug.DrawLine(p8, p5, Color.magenta, duration);

            // sides
            Debug.DrawLine(p1, p5, Color.white, duration);
            Debug.DrawLine(p2, p6, Color.gray, duration);
            Debug.DrawLine(p3, p7, Color.green, duration);
            Debug.DrawLine(p4, p8, Color.cyan, duration);
        }
        
        // Text
        class TextHelper : MonoBehaviour
        {
            public struct TextData
            {
                public Vector3 position;
                public string text;
                public int size;
                public Color color;
                public Timer timer;
            }

            public DynamicArray<TextData> textDatas = new DynamicArray<TextData>(8);

            static TextHelper instance;
            public static TextHelper Instance => instance == null ? instance = new GameObject("XIVDebug - TextHelper").AddComponent<TextHelper>() : instance;

            void OnDrawGizmos()
            {
                for (int i = textDatas.Count - 1; i >= 0; i--)
                {
                    ref var textData = ref textDatas[i];
                    var style = new GUIStyle();
                    style.fontSize = textData.size;
                    style.normal.textColor = textData.color;
                    Handles.Label(textData.position, textData.text, style);
                    if (textData.timer.Update(Time.deltaTime))
                    {
                        textDatas.RemoveAt(i);
                    }
                }
            }

            void OnDestroy()
            {
                instance = null;
            }
        }

        
        public static void DrawText(Vector3 position, string text, int size, Color color, float duration = 0f)
        {
            TextHelper.Instance.textDatas.Add() = new TextHelper.TextData
            {
                position = position, 
                text = text,
                color = color,
                size = size,
                timer = new Timer(duration),
            };
        }
        
        public static void DrawText(Vector3 position, string text, int size, float duration = 0f)
        {
            DrawText(position, text, size, Color.black, duration);
        }
        
        public static void DrawText(Vector3 position, string text, float duration = 0f)
        {
            var size = (int)HandleUtility.GetHandleSize(position);
            DrawText(position, text, size, Color.black, duration);
        }

        public static void DrawTextOnLine(Vector3 from, Vector3 to, string text, int size, Color color, float t, float duration)
        {
            var position = from + (to - from) * t;
            DrawText(position, text, size, color, duration);
        }
        
        public static void DrawTextOnLine(Vector3 from, Vector3 to, string text, int size, Color color, float duration = 0f)
        {
            DrawTextOnLine(from, to, text, size, color, 0.5f, duration);
        }
        
        public static void DrawTextOnLine(Vector3 from, Vector3 to, string text, int size, float duration = 0f)
        {
            DrawTextOnLine(from, to, text, size, Color.black, 0.5f, duration);
        }
        
        public static void DrawTextOnLine(Vector3 from, Vector3 to, string text, float duration = 0f)
        {
            var position = from + (to - from) * 0.5f;
            var size = (int)HandleUtility.GetHandleSize(position);
            DrawText(position, text, size, Color.black, duration);
        }

    }
    
#endif
}