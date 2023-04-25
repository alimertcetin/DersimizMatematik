using System;
using System.Collections;
using System.Collections.Generic;

namespace XIV.Core.Collections
{
    public class DynamicArray<T> : IList<T>
    {
        T[] values;
        public ref T this[int index] => ref values[index];
        public int Count { get; private set; }
        
        T IList<T>.this[int index]
        {
            get => values[index];
            set => values[index] = value;
        }
        bool ICollection<T>.IsReadOnly => values.IsReadOnly;
        const int DEFAULT_SIZE = 8;

        public DynamicArray(int size)
        {
            if (size <= 0) size = DEFAULT_SIZE;
            
            values = new T[size];
            Count = 0;
        }

        public ref T Add()
        {
            if (Count >= values.Length)
            {
                Array.Resize(ref values, values.Length * 2);
            }

            return ref values[Count++];
        }

        public void RemoveAt(int index)
        {
            if (index < 0) return;
            
            for (int i = index; i < Count - 1; i++)
            {
                values[i] = values[i + 1];
            }
            Count--;
        }

        public int IndexOf(ref T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (values[i].Equals(item)) return i;
            }
            return -1;
        }

        /// <summary>
        /// Use <see cref="IndexOf(ref T)"/> for performance reasons
        /// </summary>
        int IList<T>.IndexOf(T item) => IndexOf(ref item);

        /// <summary>
        /// Use <see cref="Add()"/> for performance reasons
        /// <example><code>Add() = item</code></example>
        /// </summary>
        void ICollection<T>.Add(T item) => this.Add() = item;

        public void Clear() => Count = 0;

        public bool Contains(ref T item) => IndexOf(ref item) != -1;

        /// <summary>
        /// Use <see cref="Contains(ref T)"/> for performance reasons
        /// </summary>
        bool ICollection<T>.Contains(T item) => Contains(ref item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(values, 0, array, arrayIndex, Count);
        }

        public bool Remove(ref T item)
        {
            int index = IndexOf(ref item);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Use <see cref="Remove(ref T)"/> for performance reasons
        /// </summary>
        bool ICollection<T>.Remove(T item) => Remove(ref item);

        public void Insert(int index, ref T item)
        {
            this.Add();
            for (int i = Count - 1; i >= index; i--)
            {
                values[i] = values[i - 1];
            }
            
            values[index] = item;
        }

        /// <summary>
        /// Use <see cref="Insert(int, ref T)"/> for performance reasons
        /// </summary>
        void IList<T>.Insert(int index, T item) => Insert(index, ref item);

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        struct Enumerator : IEnumerator<T>
        {
            DynamicArray<T> array;

            public T Current => array[currentIndex];

            object IEnumerator.Current => Current;
            int currentIndex;
            
            public Enumerator(DynamicArray<T> dynamicArray)
            {
                this.array = dynamicArray;
                currentIndex = -1;
            }
            
            public bool MoveNext() => ++currentIndex < array.Count;

            public void Reset() => currentIndex = -1;

            public void Dispose()
            {
                array = null;
                currentIndex = -1;
            }
        }
    }
}