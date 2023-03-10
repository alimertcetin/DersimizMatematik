using System;

namespace XIV.Extensions
{
    public static class ArrayExtensions
    {
        public static bool Contains<T>(this T[] array, T item, out int index)
        {
            index = -1;
            var lenght = array.Length;
            for (int i = 0; i < lenght; i++)
            {
                if (item.Equals(array[i]))
                {
                    index = i;
                    return true;
                }
            }

            return false;
        }

        public static bool Contains<T>(this T[] array, T item)
        {
            return Contains(array, item, out _);
        }

        public static T[] Split<T>(this T[] array, Func<T, bool> condition)
        {
            int length = array.Length;
            T[] arr = new T[length];

            for (int i = 0, j = 0; i < length; i++)
            {
                if (condition.Invoke(array[i]))
                {
                    arr[j++] = array[i];
                }
            }
            return arr;
        }

        public static int Count<T>(this T[] array, Func<T, bool> condition)
        {
            int length = array.Length;
            int count = 0;
            for (int i = 0; i < length; i++)
            {
                if (condition.Invoke(array[i])) count++;
            }
            return count;
        }
    }
}