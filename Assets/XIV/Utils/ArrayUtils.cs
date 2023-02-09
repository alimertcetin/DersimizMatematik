using System;
using UnityEngine;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;

namespace XIV.Utils
{
    public static class ArrayUtils
    {
        // https://en.wikipedia.org/wiki/Row-_and_column-major_order
        public static int Get1DIndex(int x, int y, int width)
        {
            // 2,0 (6) - 2,1 (7) - 2,2 (8)
            // 1,0 (3) - 1,1 (4) - 1,2 (5)
            // 0,0 (0) - 0,1 (1) - 0,2 (2)
            
            return x * width + y; // row major
            // return y * height + x; // column major
        }
        
        public static Vector2Int Get2DIndex(int index, int width)
        {
            return new Vector2Int(index / width, index % width);
            // return new Vector2Int(index / height, index % height);
        }

        public static bool Contains<T>(T item, T[] array)
        {
            var lenght = array.Length;
            for (int i = 0; i < lenght; i++)
            {
                if (item.Equals(array[i])) return true;
            }

            return false;
        }

        public static T[] Split<T>(T[] items, Func<T, bool> condition)
        {
            int length = items.Length;
            T[] arr = new T[length];

            for (int i = 0, j = 0; i < length; i++)
            {
                if (condition.Invoke(items[i]))
                {
                    arr[j++] = items[i];
                }
            }
            return arr;
        }

        public static int Count<T>(T[] items, Func<T, bool> condition)
        {
            int length = items.Length;
            int count = 0;
            for (int i = 0; i < length; i++)
            {
                if (condition.Invoke(items[i])) count++;
            }
            return count;
        }
    }
}