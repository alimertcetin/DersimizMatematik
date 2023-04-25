namespace XIV.InventorySystem
{
    public interface IInventoryListener
    {
        void OnInventoryChanged(InventoryChange inventoryChange);
    }
}