namespace LessonIsMath.StatSystems
{
    public struct StatChange
    {
        public readonly int ChangedIndex; // same as ChangedItem.Index
        public readonly ReadOnlyStat changedStat;
        public readonly bool IsRemoved;

        public StatChange(int changedIndex, ReadOnlyStat changedStat, bool isRemoved)
        {
            this.ChangedIndex = changedIndex;
            this.changedStat = changedStat;
            this.IsRemoved = isRemoved;
        }
    }
}