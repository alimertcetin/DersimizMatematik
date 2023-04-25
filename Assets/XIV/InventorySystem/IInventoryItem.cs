namespace XIV.InventorySystem
{
    public interface IInventoryItem
    {
        public int Index { get; }
        public int Amount { get; }
        public ItemBase Item { get; }
        public bool IsEmpty { get; }
    }
}