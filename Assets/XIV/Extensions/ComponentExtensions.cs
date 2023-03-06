using UnityEngine;
using XIV.Utils;

namespace XIV.Extensions
{
    public static class ComponentExtensions
    {
        public static T GetClosest<T>(this T[] searchArray, Vector3 currentPosition, out float distance) where T : Component
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
        
        public static T GetClosest<T>(this T[] searchArray, Vector3 currentPosition) where T : Component
        {
            return GetClosest(searchArray, currentPosition, out _);
        }
        
        public static T GetClosestOnXZPlane<T>(this T[] searchArray, Vector3 currentPosition) where T : Component
        {
            var length = searchArray.Length;
            var distance = float.MaxValue;
            if (length == 0) return default;

            T selected = default;

            currentPosition = currentPosition.OnXZ();
            for (int i = 0; i < length; i++)
            {
                var current = searchArray[i];
                var dis = currentPosition.DistanceTo(current.transform.position.OnXZ());
                if (dis < distance)
                {
                    distance = dis;
                    selected = current;
                }
            }
            
            return selected;
        }

        public static T GetClosest<T>(this T[] searchArray, Vector3 currentPosition, out float distance, params T[] exclude) where T : Component
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

        public static T GetClosest<T>(this T[] searchArray, Vector3 currentPosition, params T[] exclude) where T : Component
        {
            return GetClosest(searchArray, currentPosition, out _, exclude);
        }
    }
}