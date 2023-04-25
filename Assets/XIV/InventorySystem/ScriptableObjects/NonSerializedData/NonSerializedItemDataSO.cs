using UnityEngine;
using XIV.InventorySystem.ScriptableObjects;

namespace XIV.InventorySystem.ScriptableObjects.NonSerializedData
{
    public abstract class NonSerializedItemDataSO : ScriptableObject
    {
        public ItemSO itemSO;
        public Sprite uiSprite;
    }
}