using UnityEngine;

namespace XIV.InventorySystem
{
    [System.Serializable]
    public readonly struct ReadOnlyInventoryItem : IInventoryItem
    {
        [field: SerializeField] public int Index { get; }
        [field: SerializeField] public int Amount { get; }
        [field: SerializeField] public ItemBase Item { get; }
        public bool IsEmpty => Amount <= 0;

        public ReadOnlyInventoryItem(InventoryItem inventoryItem)
        {
            this.Index = inventoryItem.Index;
            this.Amount = inventoryItem.Amount;
            this.Item = inventoryItem.Item;
        }
    }
}