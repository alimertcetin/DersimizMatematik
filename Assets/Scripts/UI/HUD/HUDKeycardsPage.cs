using System.Collections.Generic;
using LessonIsMath.UI;
using UnityEngine;
using XIV.InventorySystem;
using XIV.InventorySystem.Items;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;

namespace XIV.UI
{
    public class HUDKeycardsPage : PageUI
    {
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        [SerializeField] InventoryChangeChannelSO inventoryChangedChannel;
        [SerializeField] Transform contentParent;
        [SerializeField] KeycardItemSO[] keycardItems;
        HUDItemSlot[] slots;

        void Awake()
        {
            slots = contentParent.GetComponentsInChildren<HUDItemSlot>();
            for (var i = 0; i < slots.Length; i++)
            {
                var slot = slots[i];
                slot.SetItem(new ReadOnlyInventoryItem(new InventoryItem(i, 0, keycardItems[i].GetItem())));
                slot.gameObject.SetActive(false);
            }
        }

        void OnEnable()
        {
            inventoryLoadedChannel.Register(OnInventoryLoaded);
            inventoryChangedChannel.Register(OnInventoryChanged);
        }

        void OnDisable()
        {
            inventoryLoadedChannel.Unregister(OnInventoryLoaded);
            inventoryChangedChannel.Unregister(OnInventoryChanged);
        }

        void OnInventoryLoaded(Inventory inventory)
        {
            IList<ReadOnlyInventoryItem> keycardItems = inventory.GetItemsOfType<KeycardItem>(_ => true);
            for (var i = 0; i < slots.Length; i++)
            {
                HUDItemSlot slot = slots[i];
                KeycardItem slotItem = (KeycardItem)slot.inventoryItem.Item;
                for (var j = 0; j < keycardItems.Count; j++)
                {
                    ReadOnlyInventoryItem readOnlyInventoryItem = keycardItems[j];
                    KeycardItem inventoryItem = (KeycardItem)readOnlyInventoryItem.Item;
                    if (slotItem.KeycardType == inventoryItem.KeycardType)
                    {
                        slot.SetItem(readOnlyInventoryItem);
                        slot.gameObject.SetActive(readOnlyInventoryItem.Amount > 0);
                        break;
                    }
                }
            }
        }

        void OnInventoryChanged(InventoryChange inventoryChange)
        {
            for (var i = 0; i < inventoryChange.ChangeCount; i++)
            {
                InventoryItemChange inventoryItemChange = inventoryChange.ChangedItems[i];
                if (inventoryItemChange.ChangedItem.Item is not KeycardItem inventoryItem) continue;
                for (var j = 0; j < slots.Length; j++)
                {
                    HUDItemSlot slot = slots[j];
                    KeycardItem slotItem = (KeycardItem)slot.inventoryItem.Item;
                    if (slotItem.KeycardType == inventoryItem.KeycardType)
                    {
                        slot.SetItem(inventoryItemChange.ChangedItem);
                        slot.gameObject.SetActive(inventoryItemChange.ChangedItem.Amount > 0);
                        break;
                    }
                }
            }
        }
        
    }
}