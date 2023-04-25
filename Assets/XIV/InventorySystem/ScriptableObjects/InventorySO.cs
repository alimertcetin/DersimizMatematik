using System;
using System.Collections.Generic;
using UnityEngine;

namespace XIV.InventorySystem.ScriptableObjects
{
    [System.Serializable]
    public struct ItemSOData
    {
        public ItemSO itemSO;
        public int amount;
    }
    
    [CreateAssetMenu(menuName = "Inventory/InventorySO")]
    public class InventorySO : ScriptableObject
    {
        public int SlotCount;
        public List<ItemSOData> items;
#if UNITY_EDITOR
        [SerializeField] List<InventoryItem> runtimeItems;
#endif

        public Inventory GetInventory()
        {
            var inventory = new Inventory(SlotCount);
            
            for (var i = 0; i < items.Count; i++)
            {
                ItemSO itemSO = items[i].itemSO;
                int amount = items[i].amount;
                if (amount <= 0)
                {
                    Debug.LogError(new InvalidOperationException("Amount cant be less than or equal to 0"));
                    break;
                }

                bool isAdded = inventory.TryAdd(itemSO.GetBaseItem(), ref amount);
                if (!isAdded)
                {
                    Debug.LogError("Inventory is full! Couldnt add item at index : " + i);
                    break;
                }
            }
            
#if UNITY_EDITOR
            runtimeItems = new List<InventoryItem>(SlotCount);
            for (var i = 0; i < inventory.Count; i++)
            {
                runtimeItems.Add(inventory[i].NewInventoryItem());
            }
#endif
            
            return inventory;
        }
    }
}