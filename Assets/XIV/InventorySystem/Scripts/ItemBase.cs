using UnityEngine;

namespace XIV.InventorySystem
{
    [System.Serializable]
    public abstract class ItemBase
    {
        [DisplayWithoutEdit, SerializeField] string id;
        public string Id => id;
        public string title;
        public string description;
        
        [Min(1)]
        public int StackableAmount = 1;

#if UNITY_EDITOR
        [ContextMenu(nameof(GenerateID))]
        public void GenerateID() => id = System.Guid.NewGuid().ToString();
#endif

        public abstract bool Equals(ItemBase other);
    }
}