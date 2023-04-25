using System.Collections.Generic;
using UnityEngine;

namespace XIV.XIVMath
{
	/// <summary>
	/// Spline Math for Cubic Bezier Curves
	/// </summary>
	public static class SplineMath
	{
		/// <summary>
		/// Returns the point at giving <paramref name="t"/> time
		/// </summary>
		/// <param name="points">Spline points</param>
		/// <param name="t">Time between 0 and 1</param>
		/// <returns>The point at giving <paramref name="t"/> time</returns>
		public static Vector3 GetPoint(IList<Vector3> points, float t)
		{
			int curveCount = (points.Count - 1) / 3;
			int index;
			if (t >= 1f)
			{
				t = 1f;
				index = points.Count - 4;
			}
			else
			{
				t = Mathf.Clamp01(t) * curveCount;
				index = (int)t;
				t -= index;
				index *= 3;
			}

			return BezierMath.GetPoint(points[index], points[index + 1], points[index + 2], points[index + 3], t);
		}
        
		/// <summary>
		/// Returns the Velocity of spline at <paramref name="t"/> point
		/// </summary>
		/// <param name="points">Spline points</param>
		/// <param name="t">Time between 0 and 1</param>
		/// <returns>The Velocity of spline at <paramref name="t"/> point</returns>
		public static Vector3 GetVelocity(IList<Vector3> points, float t)
		{
			int curveCount = (points.Count - 1) / 3;
			int index;
			if (t >= 1f)
			{
				t = 1f;
				index = points.Count - 4;
			}
			else
			{
				t = Mathf.Clamp01(t) * curveCount;
				index = (int)t;
				t -= index;
				index *= 3;
			}

			return BezierMath.GetFirstDerivative(points[index], points[index + 1], points[index + 2], points[index + 3], t);
		}
        
		/// <summary>
		/// Returns control point index of anchor
		/// </summary>
		/// <param name="anchorIndex">Anchor index</param>
		/// <returns>Control point index of anchor</returns>
		public static int IndexOfControlPoint(int anchorIndex)
		{
			int mod = anchorIndex % 3;
			if (mod == 0) return anchorIndex;
            
			// mod == 2 previous anchor, mod == 1 next anchor
			return mod == 2 ? anchorIndex + 1 : anchorIndex - 1;
		}

		/// <summary>
		/// Returns true if index is an anchor point, false otherwise
		/// </summary>
		/// <param name="index">The index of point</param>
		/// <returns>True if index is anchor point, false otherwise</returns>
		public static bool IsAnchorPoint(int index)
		{
			return IndexOfControlPoint(index) != index;
		}

		public static float GetLength(IList<Vector3> points, int stepsPerCurve = 10)
		{
			int steps = stepsPerCurve * ((points.Count - 1) / 3);
			var p0 = GetPoint(points, 0);
			float length = 0f;
			for (int i = 1; i <= steps; i++)
			{
				float t = i / (float)steps;
				var p1 = GetPoint(points, t);
				length += (p0 - p1).magnitude;
				p0 = p1;
			}

			return length;
		}
	}
}