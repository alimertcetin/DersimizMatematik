using System;
using UnityEngine;
using XIV.InventorySystem;

namespace XIV.InventorySystem.ScriptableObjects.ChannelSOs
{
    [CreateAssetMenu(menuName = "ChannelSOs/ItemBaseChannelSO")]
    public class ItemBaseChannelSO : ScriptableObject
    {
        public Action<ItemBase> action;
        
        public void RaiseEvent(ItemBase item)
        {
            action?.Invoke(item);
        }
    }
}