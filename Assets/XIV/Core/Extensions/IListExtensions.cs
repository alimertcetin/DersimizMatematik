using System.Collections.Generic;

namespace XIV.Core.Extensions
{
    public static class IListExtensions
    {
        public static T PickRandom<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}