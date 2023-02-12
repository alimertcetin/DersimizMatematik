namespace XIV.InventorySystem
{
    public readonly struct InventoryItemChange
    {
        public readonly int ChangedIndex; // same as ChangedItem.Index
        public readonly ReadOnlyInventoryItem ChangedItem;
        public readonly bool IsRemoved => ChangedItem.Amount <= 0;

        public InventoryItemChange(int changedIndex, ReadOnlyInventoryItem changedItem)
        {
            this.ChangedIndex = changedIndex;
            this.ChangedItem = changedItem;
        }
    }
}