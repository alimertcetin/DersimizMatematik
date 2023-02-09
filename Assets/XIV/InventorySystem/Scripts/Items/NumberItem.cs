using UnityEngine;

namespace XIV.InventorySystem.Items
{
    [System.Serializable]
    public class NumberItem : ItemBase
    {
        public int Value;

        public override bool Equals(ItemBase other)
        {
            if (other is not NumberItem otherItem) return false;

            return Object.Equals(otherItem, this);
        }
    }
}