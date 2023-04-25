using UnityEngine;

namespace XIV.InventorySystem
{
    [System.Serializable]
    public struct InventoryItem : IInventoryItem
    {
        public static readonly InventoryItem InvalidInventoryItem = new InventoryItem(-1, -1, null);
        [field: SerializeField] public int Index { get; set; }
        [field: SerializeField] public int Amount { get; set; }
        [field: SerializeField] public ItemBase Item { get; set; }
        public bool IsEmpty => Amount <= 0;

        public InventoryItem(int index, int amount, ItemBase item)
        {
            this.Index = index;
            this.Amount = amount;
            this.Item = item;
        }
    }
}