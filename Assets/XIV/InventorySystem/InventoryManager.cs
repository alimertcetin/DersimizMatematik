using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using XIV.InventorySystem.ScriptableObjects;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.SaveSystems;

namespace XIV.InventorySystem
{
    public class InventoryManager : MonoBehaviour, IInventoryListener, ISaveable
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

        #region Save

        [System.Serializable]
        struct SaveData
        {
            public ItemBase[] items;
            public int[] amounts;
        }
        
        object ISaveable.CaptureState()
        {
            int count = inventory.Count;
            ItemBase[] items = new ItemBase[count];
            int[] amounts = new int[count];
            for (int i = 0; i < count; i++)
            {
                ReadOnlyInventoryItem readOnlyInventoryItem = inventory[i];
                items[i] = readOnlyInventoryItem.Item;
                amounts[i] = readOnlyInventoryItem.Amount;
            }
            return new SaveData
            {
                items = items,
                amounts = amounts,
            };
        }

        void ISaveable.RestoreState(object state)
        {
            SaveData saveData = (SaveData)state;
            if (saveData.items == null) return;

            for (int i = 0; i < inventory.SlotCount; i++)
            {
                int amount = int.MaxValue;
                inventory.RemoveAt(i, ref amount, false);
            }
            
            for (int i = 0; i < saveData.items.Length - 1; i++)
            {
                inventory.TryAdd(saveData.items[i], ref saveData.amounts[i], false);
            }

            int last = saveData.items.Length - 1;
            inventory.TryAdd(saveData.items[last], ref saveData.amounts[last], true);
        }
        
        #endregion
        
    }
}