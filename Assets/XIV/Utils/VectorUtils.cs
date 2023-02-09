using UnityEngine;

namespace XIV.Utils
{
    public static class VectorUtils
    {
        public static T GetClosest<T>(Vector3 currentPosition, out float distance, T[] searchArray) where T : Component
        {
            var length = searchArray.Length;
            distance = float.MaxValue;
            if (length == 0) return default;

            T selected = default;

            for (int i = 0; i < length; i++)
            {
                var current = searchArray[i];
                var dis = Vector3.Distance(currentPosition, current.transform.position);
                if (dis < distance)
                {
                    distance = dis;
                    selected = current;
                }
            }
            
            return selected;
        }
        
        public static T GetClosest<T>(Vector3 currentPosition, out float distance,
            T[] searchArray, params T[] exclude) where T : Component
        {
            var length = searchArray.Length;
            distance = float.MaxValue;
            if (length == 0) return default;

            T selected = default;

            for (int i = 0; i < length; i++)
            {
                var current = searchArray[i];
                var dis = Vector3.Distance(currentPosition, current.transform.position);
                if (dis < distance && ArrayUtils.Contains(current, exclude) == false)
                {
                    distance = dis;
                    selected = current;
                }
            }
            
            return selected;
        }
    }
}