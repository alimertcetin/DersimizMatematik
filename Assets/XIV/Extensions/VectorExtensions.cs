using System.Collections.Generic;
using UnityEngine;
using XIV.XIVMath;

namespace XIV.Extensions
{
    public static class VectorExtensions
	{
		/*
		 * Created using OpenAI Assistant
		 */
		public static Vector3[] ToVector3<T>(this T vector2Collection) where T : IList<Vector2>
		{
			Vector3[] vector3Array = new Vector3[vector2Collection.Count];
			for (int i = 0; i < vector2Collection.Count; i++)
			{
				vector3Array[i] = new Vector3(vector2Collection[i].x, vector2Collection[i].y, 0);
			}
			return vector3Array;
		}
		
		/*
		 * Created using OpenAI Assistant
		 */
		public static Vector2[] ToVector2<T>(this T vector3Collection) where T : IList<Vector3>
		{
			Vector2[] vector2Array = new Vector2[vector3Collection.Count];
			for (int i = 0; i < vector3Collection.Count; i++)
			{
				vector2Array[i] = new Vector2(vector3Collection[i].x, vector3Collection[i].y);
			}
			return vector2Array;
		}
		
		/*
		 * Created using OpenAI Assistant
		 */
		public static void ToVector2NonAlloc<T>(this T vector3Collection, Vector2[] vector2Array) where T : IList<Vector3>
		{
			int count = XIVMathInt.Min(vector2Array.Length, vector3Collection.Count);

			for (int i = 0; i < count; i++)
			{
				vector2Array[i] = vector3Collection[i];
			}
		}
		
		/*
		 * Created using OpenAI Assistant
		 */
		public static void ToVector3NonAlloc<T>(this T vector2Collection, Vector3[] vector3Array) where T : IList<Vector2>
		{
			int count = XIVMathInt.Min(vector3Array.Length, vector2Collection.Count);

			for (int i = 0; i < count; i++)
			{
				vector3Array[i] = vector2Collection[i];
			}
		}
	}
}