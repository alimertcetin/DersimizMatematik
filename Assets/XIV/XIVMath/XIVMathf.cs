namespace XIV.XIVMath
{
	public static class XIVMathf
	{
		public static float Epsilon = float.Epsilon;
		public static float Clamp01(float val)
		{
			return val > 1 ? 1 : val < 0 ? 0 : val;
		}
		
		public static float Clamp(float val, float min, float max)
		{
			return val > max ? max : val < min ? min : val;
		}

		public static float Lerp(float a, float b, float t)
		{
			t = Clamp01(t);
			return a + (b - a) * t;
		}

		public static float LerpUnclamped(float a, float b, float t)
		{
			return a + (b - a) * t;
		}

		public static float Normalize(float val, float min, float max)
		{
			return (val - min) / (max - min);
		}

		public static float Normalize01(float val, float max)
		{
			var normalized = val / max;
			return Clamp01(normalized);
		}

		public static float Remap(float val, float min, float max, float newMin, float newMax)
		{
			return (val - min) / (max - min) * (newMax - newMin) + newMin;
		}

		public static float RemapClamped(float val, float min, float max, float newMin, float newMax)
		{
			return Clamp(Remap(val, min, max, newMin, newMax), newMin, newMax);
		}

		public static float Max(float a, float b)
		{
			return a > b ? a : b;
		}

		public static float Min(float a, float b)
		{
			return a < b ? a : b;
		}

		public static float Max(params float[] values)
		{
			var max = float.MinValue;
			int length = values.Length;
			for (int i = 0; i < length; i++)
			{
				if (max < values[i]) max = values[i];
			}

			return max;
		}
		
		public static float Sqrt(float number)
		{
			float precision = 0.0000001f;
			float low = 0;
			float high = number;
			float mid = 0;
			while ((high - low) > precision)
			{
				mid = (low + high) / 2;
				if ((mid - precision) >= mid * mid && mid * mid <= (precision + mid))
				{
					break;
				}
				else if (mid * mid < number)
				{
					low = mid;
				}
				else
				{
					high = mid;
				}
			}

			return mid;
		}

		public static double Sqrt(double number)
		{
			double precision = 0.0000001;
			double low = 0;
			double high = number;
			double mid = 0;
			while ((high - low) > precision)
			{
				mid = (double)((low + high) / 2);
				if ((mid - precision) >= mid * mid && mid * mid <= (precision + mid))
				{
					break;
				}
				else if (mid * mid < number)
				{
					low = mid;
				}
				else
				{
					high = mid;
				}
			}

			return mid;
		}
		
		/*
		 * Created using OpenAI Assistant
		 */
		public static float Abs(float x)
		{
			return (x < 0) ? -x : x;
		}
		
		/*
		 * Created using OpenAI Assistant - Newton-Raphson method
		 */
		public static float SqrtV2(float x)
		{
			if (x < 0)
			{
				return float.NaN;
			}

			float result = x;
			float prevResult;
			do
			{
				prevResult = result;
				result = (result + x / result) * 0.5f;
			} while (Abs(result - prevResult) > 0.00001f);

			return result;
		}

		public static float Repeat(float value, float length)
		{
			return value < 0 ? (value % length) + length : value % length;
		}

	}
}