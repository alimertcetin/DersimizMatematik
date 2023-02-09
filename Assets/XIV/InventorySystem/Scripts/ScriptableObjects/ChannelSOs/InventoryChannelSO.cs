using System;
using UnityEngine;

namespace XIV.InventorySystem.ScriptableObjects.ChannelSOs
{
    [CreateAssetMenu(menuName = "ChannelSOs/InventoryChannelSO")]
    public class InventoryChannelSO : ScriptableObject
    {
        Action<Inventory> action;

        public void Register(Action<Inventory> action)
        {
            this.action += action;
        }

        public void Unregister(Action<Inventory> action)
        {
            this.action -= action;
        }
        
        public void RaiseEvent(Inventory inventory)
        {
            action?.Invoke(inventory);
        }
    }
}