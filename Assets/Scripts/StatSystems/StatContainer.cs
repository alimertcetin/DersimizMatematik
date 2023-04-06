using System.Collections.Generic;
using XIV.Collections;

namespace LessonIsMath.StatSystems
{
    public class StatContainer
    {
        readonly DynamicArray<Stat> items;
        readonly DynamicArray<StatChange> itemChanges;
        readonly List<IStatContainerListener> listeners;
        public int Count { get; private set; }
        public ReadOnlyStat this[int index] => index < 0 || index >= items.Count ? 
            ReadOnlyStat.InvalidReadOnlyStat : new ReadOnlyStat(items[index]);

        public StatContainer()
        {
            items = new DynamicArray<Stat>(8);
            itemChanges = new DynamicArray<StatChange>(8);
            listeners = new List<IStatContainerListener>(4);
            for (int i = 0; i < 8; i++)
            {
                items[i] = Stat.InvalidStat;
            }
        }

        public void AddListener(IStatContainerListener listener)
        {
            if (listeners.IndexOf(listener) > -1) return;
            listeners.Add(listener);
        }

        public void RemoveListener(IStatContainerListener listener)
        {
            int index = listeners.IndexOf(listener);
            if (index < 0) return;
            listeners.RemoveAt(index);
        }

        void InformListeners()
        {
            if (itemChanges.Count == 0) return;
            
            int count = listeners.Count;
            var statContainerChange = new StatContainerChange(itemChanges);
            for (int i = 0; i < count; i++)
            {
                listeners[i].OnStatContainerChanged(statContainerChange);
            }
            itemChanges.Clear();
        }

        public StatData GetStatData<T>() where T : StatItemBase
        {
            int index = IndexOf<T>();
            if (index < 0) return new StatData();
            return items[index].statItem.statData;
        }

        public T GetStatItem<T>() where T : StatItemBase
        {
            int index = IndexOf<T>();
            if (index < 0) return default;
            return (T)items[index].statItem;
        }

        public bool Contains(StatItemBase item)
        {
            var index = IndexOf(item);
            return index >= 0;
        }

        public int IndexOf(StatItemBase item)
        {
            for (var i = 0; i < Count; i++)
            {
                if (items[i].statItem.Equals(item) == false) continue;
                
                return i;
            }
            return -1;
        }

        int IndexOf<T>() where T : StatItemBase
        {
            for (var i = 0; i < Count; i++)
            {
                ref Stat stat = ref this.items[i];
                if (stat.statItem is not T) continue;

                return i;
            }

            return -1;
        }

        public void Add(StatItemBase item, bool informListeners = true)
        {
            items.Add() = new Stat(Count, item);
            itemChanges.Add() = new StatChange(Count, this[Count], false);
            Count++;
            if (informListeners) InformListeners();
        }

        public void UpdateStat<T>(StatData statData) where T : StatItemBase
        {
            int index = IndexOf<T>();
            if (index < 0) return;
            items[index].statItem.statData.Update(statData);
            
            itemChanges.Add() = new StatChange(index, this[index], false);
            InformListeners();
        }

        public void UpdateStatItemExperience<T>(float amount) where T : StatItemBase
        {
            int index = IndexOf<T>();
            if (index < 0) return;
            bool levelUp = items[index].statItem.UpdateExperience(amount);
            
            itemChanges.Add() = new StatChange(index, this[index], false, levelUp);
            InformListeners();
        }

        public void Remove(StatItemBase item)
        {
            for (var i = 0; i < Count; i++)
            {
                if (items[i].statItem.Equals(item) == false) continue;
                
                Internal_RemoveAt(i);
            }
            
            InformListeners();
        }

        public void RemoveAt(int index, bool informListeners = true)
        {
            Internal_RemoveAt(index);
            if (informListeners) InformListeners();
        }

        void Internal_RemoveAt(int index)
        {
            items.RemoveAt(index);
            itemChanges.Add() = new StatChange(index, this[index], true);
            Count--;
        }

        public void Swap(int index1, int index2)
        {
            if (index1 == index2) return;
            
            (items[index1], items[index2]) = (items[index2], items[index1]);
            
            itemChanges.Add() = new StatChange(index1, this[index1], false);
            itemChanges.Add() = new StatChange(index2, this[index2], false);
            
            InformListeners();
        }
    }
}