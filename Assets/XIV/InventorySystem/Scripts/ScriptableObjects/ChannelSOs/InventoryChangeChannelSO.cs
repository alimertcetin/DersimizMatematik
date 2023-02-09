using System;
using UnityEngine;

namespace XIV.InventorySystem.ScriptableObjects.ChannelSOs
{
    [CreateAssetMenu(menuName = "ChannelSOs/InventoryChangeChannelSO")]
    public class InventoryChangeChannelSO : ScriptableObject
    {
        Action<InventoryChange> action;

        public void Register(Action<InventoryChange> action)
        {
            this.action += action;
        }

        public void Unregister(Action<InventoryChange> action)
        {
            this.action -= action;
        }
        
        public void RaiseEvent(InventoryChange inventoryChange)
        {
            action?.Invoke(inventoryChange);
        }
    }
}