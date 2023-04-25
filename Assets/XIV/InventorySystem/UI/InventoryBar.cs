using UnityEngine;
using XIV.InventorySystem.ScriptableObjects;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem.ScriptableObjects.NonSerializedData;
using XIV.Utils;

namespace XIV.InventorySystem.UI
{
    public class InventoryBar : MonoBehaviour
    {
        [SerializeField] NonSerializedItemDataContainerSO ItemDataContainerSO;
        [SerializeField] InventoryItemChannelSO useItemRequestChannel;
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        [SerializeField] InventoryChangeChannelSO inventoryChangedChannel;
        
        [SerializeField] Transform contentParent;
        [SerializeField] Transform selectionIndicator;
        [SerializeField] float transitionDuration;
        ScrollSelector scrollSelector;
        InventorySlot[] slots;
        int slotCount;
        Inventory inventory;

        void OnEnable()
        {
            inventoryLoadedChannel.Register(OnInventoryLoaded);
            inventoryChangedChannel.Register(OnInventoryChanged);
            scrollSelector = new ScrollSelector(contentParent, transitionDuration);
            
            slotCount = contentParent.childCount;
            slots = contentParent.GetComponentsInChildren<InventorySlot>();
            for (int i = 0; i < slotCount; i++)
            {
                InventorySlot inventorySlot = slots[i];
                inventorySlot.SetItem(default, null);
            }
        }

        void OnDisable()
        {
            inventoryLoadedChannel.Unregister(OnInventoryLoaded);
            inventoryChangedChannel.Unregister(OnInventoryChanged);
        }

        void Update()
        {
            scrollSelector.Update(Time.deltaTime, Input.mouseScrollDelta);
            selectionIndicator.position = scrollSelector.CurrentPos;
            if (Input.GetKeyDown(KeyCode.F))
            {
                UseSelected();
            }
        }

        void OnInventoryLoaded(Inventory inventory)
        {
            this.inventory = inventory;
            for (int i = 0; i < slotCount; i++)
            {
                InventorySlot inventorySlot = slots[i];
                inventorySlot.SetItem(inventory[i], ItemDataContainerSO.GetSprite(inventory[i].Item));
            }
        }

        void OnInventoryChanged(InventoryChange inventoryChange)
        {
            for (int i = 0; i < inventoryChange.ChangeCount; i++)
            {
                int index = inventoryChange.ChangedItems[i].ChangedIndex;
                if (index >= slotCount) continue;
                
                var inventorySlot = slots[index];
                inventorySlot.SetItem(inventory[index], ItemDataContainerSO.GetSprite(inventory[index].Item));
            }
        }

        void UseSelected()
        {
            if (scrollSelector.IsInTransition()) return;

            var inventorySlot = slots[scrollSelector.CurrentSelection];
            if (inventorySlot.IsEmpty) return;
            var item = inventorySlot.inventoryItem;
            useItemRequestChannel.RaiseEvent(item, 1);
        }

    }
}