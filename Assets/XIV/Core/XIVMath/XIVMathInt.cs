namespace XIV.Core.XIVMath
{
	public static class XIVMathInt
	{
		public static int Repeat(int value, int length)
		{
			return value < 0 ? (value % length) + length : value % length;
		}
		
		public static int Min(int a, int b)
		{
			return (a < b) ? a : b;
		}
		
	}
}