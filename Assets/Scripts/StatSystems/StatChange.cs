namespace LessonIsMath.StatSystems
{
    public struct StatChange
    {
        public readonly int ChangedIndex; // same as ChangedItem.Index
        public readonly ReadOnlyStat ChangedStat;
        public readonly bool IsRemoved;
        public readonly bool IsLevelUp;

        public StatChange(int changedIndex, ReadOnlyStat changedStat, bool isRemoved, bool isLevelUp = false)
        {
            this.ChangedIndex = changedIndex;
            this.ChangedStat = changedStat;
            this.IsRemoved = isRemoved;
            this.IsLevelUp = isLevelUp;
        }
    }
}