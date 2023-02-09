using System;
using UnityEngine;

namespace XIV.InventorySystem
{
    [System.Serializable]
    public abstract class ItemBase
    {
        public string title;
        public string description;
        public Sprite UISprite;
        
        [Min(1)]
        public int StackableAmount = 1;

        public abstract bool Equals(ItemBase other);
    }
}