using System;
using UnityEngine;

namespace XIV.InventorySystem.ScriptableObjects.ChannelSOs
{
    [CreateAssetMenu(menuName = "ChannelSOs/InventoryItemChannelSO")]
    public class InventoryItemChannelSO : ScriptableObject
    {
        Action<IInventoryItem, int> action;

        public void Register(Action<IInventoryItem, int> action)
        {
            this.action += action;
        }

        public void Unregister(Action<IInventoryItem, int> action)
        {
            this.action -= action;
        }
        
        public void RaiseEvent(IInventoryItem inventoryItem, int amount)
        {
            action?.Invoke(inventoryItem, amount);
        }
    }
}