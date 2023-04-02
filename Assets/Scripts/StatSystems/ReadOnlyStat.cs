using UnityEngine;

namespace LessonIsMath.StatSystems
{
    public readonly struct ReadOnlyStat
    {
        public static readonly ReadOnlyStat InvalidReadOnlyStat = new ReadOnlyStat(Stat.InvalidStat);
        [field: SerializeField] public int Index { get; }
        [field: SerializeField] public StatItemBase StatItem { get; }

        public ReadOnlyStat(Stat inventoryItem)
        {
            this.Index = inventoryItem.Index;
            this.StatItem = inventoryItem.statItem;
        }
    }
}