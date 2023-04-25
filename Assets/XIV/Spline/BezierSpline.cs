using System;
using System.Collections.Generic;
using UnityEngine;
using XIV.Core.XIVMath;
using XIV.Spline.Utils;

namespace XIV.Spline
{
    public class BezierSpline : MonoBehaviour
    {
        public int CurveCount => (points.Length - 1) / 3;

        public int PointCount => points.Length;

        [SerializeField] Vector3[] points;

        public float Length;

        void Awake()
        {
            CalculateSplineLength();
        }

        public void CalculateSplineLength()
        {
            Length = SplineMath.GetLength(points);
        }

        /// <summary>
        /// Returns local space point at giving <paramref name="index"/>
        /// </summary>
        public Vector3 GetPoint(int index)
        {
            return points[index];
        }

        /// <summary>
        /// Sets point at <paramref name="index"/>
        /// </summary>
        /// <param name="index">The index of point</param>
        /// <param name="point">The point in local space</param>
        public void SetPoint(int index, Vector3 point)
        {
            points[index] = point;
            Length = SplineMath.GetLength(points);
        }

        /// <summary>
        /// Adds a new curve to spline points
        /// </summary>
        public void AddCurve()
        {
            Vector3[] newPoints = SplineUtils.NewCurveAtPosition(points[^1]);
            int pointsLength = points.Length;
            Array.Resize(ref points, pointsLength + newPoints.Length);
            for (int i = 0; i < newPoints.Length; i++)
            {
                points[pointsLength + i] = newPoints[i];
            }
            Length = SplineMath.GetLength(points);
        }

        /// <summary>
        /// Tries to remove the curve that belongs to the <paramref name="index"/>, Can't remove first and last curves
        /// </summary>
        /// <param name="index">The index of point to find corresponding curve</param>
        /// <returns>True if curve is removed, false otherwise</returns>
        public bool RemoveCurve(int index)
        {
            var isRemoved = SplineUtils.RemoveCurve(points, index, out var newPoints);
            if (isRemoved)
            {
                int newSize = newPoints.Length;
                Array.Resize(ref points, newSize);
                for (int i = 0; i < newSize; i++)
                {
                    points[i] = newPoints[i];
                }
                Length = SplineMath.GetLength(points);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns point in local space at giving <paramref name="t"/> time
        /// </summary>
        /// <param name="t">Time between 0 and 1</param>
        /// <returns>The point at <paramref name="t"/> time in local space</returns>
        public Vector3 GetPoint(float t)
        {
            return SplineMath.GetPoint(points, t);
        }

        /// <summary>
        /// Returns velocity in local space at giving <paramref name="t"/> time
        /// </summary>
        /// <param name="t">Time between 0 and 1</param>
        /// <returns>The velocity at <paramref name="t"/> time in local space</returns>
        public Vector3 GetVelocity(float t)
        {
            return SplineMath.GetVelocity(points, t);
        }

        /// <summary>
        /// Returns the direction in <paramref name="t"/> time
        /// </summary>
        /// <param name="t">Time between 0 and 1</param>
        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }

        public IEnumerable<Vector3> Points()
        {
            int lenght = points.Length;
            for (int i = 0; i < lenght; i++)
            {
                yield return points[i];
            }
        }

        public void Reset()
        {
            points = new Vector3[]
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, 0f, 4f),
                new Vector3(6f, 0f, 4f),
                new Vector3(6f, 0f, 0f)
            };
            Length = SplineMath.GetLength(points);
        }
    }
}