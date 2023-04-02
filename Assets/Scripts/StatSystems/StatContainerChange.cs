using System.Collections.Generic;

namespace LessonIsMath.StatSystems
{
    public struct StatContainerChange
    {
        public readonly StatChange[] ChangedItems;
        public readonly int ChangeCount;

        public StatContainerChange(IList<StatChange> changedItems)
        {
            this.ChangeCount = changedItems.Count;
            ChangedItems = new StatChange[ChangeCount];
            changedItems.CopyTo(ChangedItems, 0);
        }
    }
}