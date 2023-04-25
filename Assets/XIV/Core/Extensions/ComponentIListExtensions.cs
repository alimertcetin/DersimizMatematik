using System.Collections.Generic;
using UnityEngine;

namespace XIV.Core.Extensions
{
    public static class ComponentIListExtensions
    {
        public static T GetClosest<T>(this IList<T> searchArray, Vector3 currentPosition, out float distance) where T : Component
        {
            var length = searchArray.Count;
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
        
        public static T GetClosest<T>(this IList<T> searchArray, Vector3 currentPosition) where T : Component
        {
            return GetClosest(searchArray, currentPosition, out _);
        }

        public static T GetClosest<T>(this IList<T> searchArray, Vector3 currentPosition, out float distance, params T[] exclude) where T : Component
        {
            var length = searchArray.Count;
            distance = float.MaxValue;
            if (length == 0) return default;

            T selected = default;

            for (int i = 0; i < length; i++)
            {
                var current = searchArray[i];
                var dis = Vector3.Distance(currentPosition, current.transform.position);
                if (dis < distance && exclude.Contains(current) == false)
                {
                    distance = dis;
                    selected = current;
                }
            }
            
            return selected;
        }

        public static T GetClosest<T>(this IList<T> searchArray, Vector3 currentPosition, params T[] exclude) where T : Component
        {
            return GetClosest(searchArray, currentPosition, out _, exclude);
        }
        
        public static T GetClosestOnXZPlane<T>(this IList<T> searchArray, Vector3 currentPosition, out float distance) where T : Component
        {
            var length = searchArray.Count;
            distance = float.MaxValue;
            if (length == 0) return default;

            T selected = default;

            currentPosition = currentPosition.OnXZ();
            for (int i = 0; i < length; i++)
            {
                var current = searchArray[i];
                var dis = Vector3.Distance(currentPosition, current.transform.position.OnXZ());
                if (dis < distance)
                {
                    distance = dis;
                    selected = current;
                }
            }
            
            return selected;
        }
        
        public static T GetClosestOnXZPlane<T>(this IList<T> searchArray, Vector3 currentPosition) where T : Component
        {
            return GetClosestOnXZPlane(searchArray, currentPosition, out _);
        }
    }
}