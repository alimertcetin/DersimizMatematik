using System;

namespace XIV.Extensions
{
    public static class ArrayExtensions
    {
        public static bool Contains<T>(this T[] array, T item)
        {
            var lenght = array.Length;
            for (int i = 0; i < lenght; i++)
            {
                if (item.Equals(array[i])) return true;
            }

            return false;
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