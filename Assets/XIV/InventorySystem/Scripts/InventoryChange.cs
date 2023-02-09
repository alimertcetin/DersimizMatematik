using System.Collections.Generic;

namespace XIV.InventorySystem
{
    // We will be able to add new fields with less effort than directly passing a collection to methods
    public readonly struct InventoryChange
    {
        public readonly InventoryItemChange[] ChangedItems;
        public readonly int ChangeCount;

        public InventoryChange(IList<InventoryItemChange> changedItems)
        {
            this.ChangeCount = changedItems.Count;
            ChangedItems = new InventoryItemChange[ChangeCount];
            changedItems.CopyTo(ChangedItems, 0);
        }
    }
}