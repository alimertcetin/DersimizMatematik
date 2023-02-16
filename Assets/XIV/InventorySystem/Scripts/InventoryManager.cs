using System;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using XIV.InventorySystem.ScriptableObjects;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;

namespace XIV.InventorySystem
{
    public class InventoryManager : MonoBehaviour, IInventoryListener
    {
        [SerializeField] InventoryItemChannelSO useItemRequestChannel;
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        [SerializeField] InventoryChangeChannelSO inventoryChangedChannel;
        [SerializeField] VoidEventChannelSO onSceneReady;

        [SerializeField] InventorySO inventorySO;
        
        Inventory inventory;

        void Awake() => inventory = inventorySO.GetInventory();
        void Start() => inventoryLoadedChannel.RaiseEvent(inventory);
        
        void OnEnable()
        {
            useItemRequestChannel.Register(UseItem);
            onSceneReady.OnEventRaised += OnSceneReady;
            inventory.AddListener(this);
        }

        void OnDisable()
        {
            useItemRequestChannel.Unregister(UseItem);
            onSceneReady.OnEventRaised -= OnSceneReady;
            inventory.RemoveListener(this);
        }

        void OnSceneReady()
        {
            inventoryLoadedChannel.RaiseEvent(inventory);
        }

        void UseItem(IInventoryItem inventoryItem, int amount)
        {
            if (inventory.CanRemove(inventoryItem, amount) == false) return;

            inventory.RemoveAt(inventoryItem.Index, ref amount);
        }

        void IInventoryListener.OnInventoryChanged(InventoryChange inventoryChange)
        {
            inventoryChangedChannel.RaiseEvent(inventoryChange);
        }
    }
}