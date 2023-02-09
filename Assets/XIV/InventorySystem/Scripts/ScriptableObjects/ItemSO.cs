using UnityEngine;

namespace XIV.InventorySystem.ScriptableObjects
{
    public abstract class ItemSO : ScriptableObject
    {
        public abstract ItemBase GetBaseItem();
    }
    
    public class ItemSO<T> : ItemSO where T : ItemBase
    {
        public T item;
        
        public override ItemBase GetBaseItem()
        {
            return GetItem();
        }

        public T GetItem()
        {
            return item;
        }
    }
}