using XIV.XIVMath;

namespace XIV.Spline
{
	using System.Collections.Generic;
	using UnityEngine;

	public static class SplineUtils
	{
		/// <summary>
		/// Returns an array of points for the new curve
		/// </summary>
		/// <returns>An array of points for the new curve</returns>
		public static Vector3[] NewCurve()
		{
			return NewCurveAtPosition(Vector3.zero);
		}
		
		/// <summary>
		/// Returns an array of points for the new curve
		/// </summary>
		/// <returns>An array of points for the new curve</returns>
		public static Vector3[] NewCurveAtPosition(Vector3 point)
		{
			Vector3[] points = new Vector3[3];
			point.x += 2f;
			point.z += 2f;
			points[0] = point;
			point.x += 1f;
			point.z -= 4f;
			points[1] = point;
			point.x += 2f;
			point.z += 2f;
			points[2] = point;
			return points;
		}
		
		/// <summary>
		/// Tries to remove curve from <paramref name="points"/> and returns true if removed. Can't remove first and last curves
		/// </summary>
		/// <param name="points">The spline points</param>
		/// <param name="index">Index of point (Curve will be search depending on it)</param>
		/// <param name="newPoints">The new points of spline</param>
		/// <returns>True if curve removed, false otherwise</returns>
		public static bool RemoveCurve(IList<Vector3> points, int index, out Vector3[] newPoints)
		{
			if (index <= 1 || index >= points.Count - 2)
			{
#if UNITY_EDITOR
				Debug.LogWarning("Removing first and last curves isnt allowed");
#endif
				newPoints = default;
				return false;
			}

			var controlPointIndex = SplineMath.IndexOfControlPoint(index);
            
			var pointList = new List<Vector3>(points);
			pointList.RemoveRange(controlPointIndex - 1, 3);
			newPoints = pointList.ToArray();
			return true;
		}
	}
}