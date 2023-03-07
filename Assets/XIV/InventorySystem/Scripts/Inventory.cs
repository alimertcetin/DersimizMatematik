using System;
using System.Collections.Generic;
using XIV.Collections;

namespace XIV.InventorySystem
{
    public delegate bool FindItemCondition<in T>(T item) where T : ItemBase;
    
    public class Inventory
    {
        public readonly int SlotCount;

        readonly InventoryItem[] items;
        readonly bool[] emptySlots;
        readonly List<IInventoryListener> listeners;
        public int Count { get; private set; }
        public ReadOnlyInventoryItem this[int index] => index < 0 || index >= SlotCount ? 
            ReadOnlyInventoryItem.InvalidReadonlyInventoryItem : new ReadOnlyInventoryItem(items[index]);

        readonly DynamicArray<InventoryItemChange> itemChanges;

        public Inventory(int slotCount = 8)
        {
            this.SlotCount = slotCount;
            items = new InventoryItem[slotCount];
            listeners = new List<IInventoryListener>(4);
            itemChanges = new DynamicArray<InventoryItemChange>(slotCount);
            emptySlots = new bool[slotCount];
            for (int i = 0; i < slotCount; i++)
            {
                emptySlots[i] = true;
                items[i] = new InventoryItem(i, 0, null);
            }
        }

        public void AddListener(IInventoryListener listener)
        {
            if (listeners.IndexOf(listener) > -1) return;
            listeners.Add(listener);
        }

        public void RemoveListener(IInventoryListener listener)
        {
            int index = listeners.IndexOf(listener);
            if (index < 0) return;
            listeners.RemoveAt(index);
        }

        void InformListeners()
        {
            if (itemChanges.Count == 0) return;
            
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
            {
                listeners[i].OnInventoryChanged(new InventoryChange(itemChanges));
            }
            itemChanges.Clear();
        }

        IList<InventoryItem> GetAllTypeOf<T>(FindItemCondition<T> condition) where T : ItemBase
        {
            var items = new DynamicArray<InventoryItem>(SlotCount);
            for (var i = 0; i < SlotCount; i++)
            {
                ref InventoryItem inventoryItem = ref this.items[i];
                if (emptySlots[i] ||
                    inventoryItem.Item is not T item ||
                    condition.Invoke(item) == false) continue;
                
                items.Add() = inventoryItem;
            }

            return items;
        }

        public static InventoryItem GetMinAmount(IList<InventoryItem> items)
        {
            int min = int.MaxValue;
            InventoryItem current = default;
            int itemCount = items.Count;
            for (var i = 0; i < itemCount; i++)
            {
                InventoryItem item = items[i];
                if (item.Amount >= min) continue;
                
                min = item.Amount;
                current = item;
            }
            return current;
        }

        public bool TryGetMinAmountOfType<T>(out T item) where T : ItemBase
        {
            return TryGetMinAmountOfType<T>(out item, (_) => true);
        }

        public bool TryGetMinAmountOfType<T>(out T item, FindItemCondition<T> condition) where T : ItemBase
        {
            var items = GetAllTypeOf(condition);
            item = GetMinAmount(items).Item as T;
            return items.Count > 0;
        }

        public bool Contains(ItemBase item)
        {
            return Contains(item, out _);
        }

        public bool Contains(ItemBase item, out int index)
        {
            for (var i = 0; i < SlotCount; i++)
            {
                if (emptySlots[i] || items[i].Item.Equals(item) == false) continue;
                
                index = i;
                return true;
            }
            index = -1;
            return false;
        }

        public bool CanAdd(ItemBase item, int amount)
        {
            for (var i = 0; i < SlotCount; i++)
            {
                if (amount <= 0) break;
                if (emptySlots[i] || items[i].Item.Equals(item) == false) continue;

                ref InventoryItem inventoryItem = ref items[i];
                int stackLeft = inventoryItem.Item.StackableAmount - inventoryItem.Amount;
                amount -= Math.Min(stackLeft, amount);
            }
            
            for (int i = 0; i < SlotCount; i++)
            {
                if (amount <= 0) break;
                if (emptySlots[i]) amount -= Math.Min(item.StackableAmount, amount);
            }

            return amount <= 0;
        }

        /// <summary>
        /// Tries to add as much as possible item at giving amount
        /// </summary>
        /// <returns>True if amount reaches 0 after add operation</returns>
        public bool TryAdd(ItemBase item, ref int amount)
        {
            for (var i = 0; i < SlotCount; i++)
            {
                if (amount <= 0) break;
                if (emptySlots[i] || items[i].Item.Equals(item) == false) continue;
                
                AddExisting(i, ref amount);
            }

            if (amount > 0) AddNew(item, ref amount);
            
            InformListeners();
            return amount <= 0;
        }

        void AddExisting(int index, ref int amount)
        {
            ref InventoryItem inventoryItem = ref items[index];
            int stackLeft = inventoryItem.Item.StackableAmount - inventoryItem.Amount;
            int addAmount = Math.Min(stackLeft, amount);
            inventoryItem.Amount += addAmount;
            amount -= addAmount;
            
            itemChanges.Add() = new InventoryItemChange(index, this[index]);
        }

        void AddNew(ItemBase item, ref int amount)
        {
            for (int i = 0; i < SlotCount; i++)
            {
                if (amount <= 0) break;
                if (emptySlots[i] == false) continue;
                emptySlots[i] = false;
                
                int addAmount = Math.Min(item.StackableAmount, amount);
                ref InventoryItem inventoryItem = ref items[i];
                inventoryItem.Amount = addAmount;
                inventoryItem.Item = item;
                amount -= addAmount;
                Count++;
                itemChanges.Add() = new InventoryItemChange(i, this[i]);
            }
        }
        
        /// <returns>True if amount reaches 0 when remove operation is done</returns>
        public bool CanRemove(ItemBase item, int amount)
        {
            for (var i = 0; i < SlotCount; i++)
            {
                if (amount <= 0) break;
                if (emptySlots[i] || items[i].Item.Equals(item) == false) continue;
                
                amount -= Math.Min(items[i].Amount, amount);
            }
            
            return amount <= 0;
        }
        
        public bool CanRemove(IInventoryItem inventoryItem, int amount)
        {
            return items[inventoryItem.Index].Amount - amount >= 0;
        }

        public void Remove(ItemBase item, ref int amount)
        {
            for (var i = 0; i < SlotCount; i++)
            {
                if (amount <= 0) break;
                if (emptySlots[i] || items[i].Item.Equals(item) == false) continue;
                
                Internal_RemoveAt(i, ref amount);
            }
            
            InformListeners();
        }

        public void RemoveAt(int index, ref int amount)
        {
            Internal_RemoveAt(index, ref amount);
            InformListeners();
        }

        void Internal_RemoveAt(int index, ref int amount)
        {
            ref InventoryItem inventoryItem = ref items[index];
            var removeAmount = Math.Min(inventoryItem.Amount, amount);
            inventoryItem.Amount -= removeAmount;
            amount -= removeAmount;
            
            itemChanges.Add() = new InventoryItemChange(index, this[index]);
            if (inventoryItem.IsEmpty == false) return;
            
            emptySlots[index] = true;
            Count--;
        }

        public void Swap(int index1, int index2)
        {
            if (index1 == index2) return;
            
            ref InventoryItem item1 = ref items[index1];
            ref InventoryItem item2 = ref items[index2];

            if (item1.Item.Equals(item2.Item))
            {
                int addAmount = item1.Amount;
                AddExisting(index2, ref addAmount);
                int removeAmount = item1.Amount - addAmount;
                Internal_RemoveAt(index1, ref removeAmount);
            }
            else
            {
                InventoryItem temp = item2;
                item2.Amount = item1.Amount;
                item2.Item = item1.Item;
                item1.Amount = temp.Amount;
                item1.Item = temp.Item;
                itemChanges.Add() = new InventoryItemChange(index1, this[index1]);
                itemChanges.Add() = new InventoryItemChange(index2, this[index2]);
            }
            
            InformListeners();
        }
    }
}