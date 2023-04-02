using UnityEngine;

namespace LessonIsMath.StatSystems
{
    [System.Serializable]
    public struct Stat : IStat
    {
        public static readonly Stat InvalidStat = new Stat(-1, null);
        [field: SerializeField] public int Index { get; set; }
        [field: SerializeField] public StatItemBase statItem { get; set; }

        public Stat(int index, StatItemBase statItem)
        {
            this.Index = index;
            this.statItem = statItem;
        }
    }
}