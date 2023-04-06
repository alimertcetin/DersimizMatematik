using UnityEngine;

namespace XIV.InventorySystem.ScriptableObjects
{
    public abstract class ItemSO : ScriptableObject
    {
#if UNITY_EDITOR
        [ContextMenu(nameof(GenerateID))]
        void GenerateID()
        {
            UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Generate ID");
            GetBaseItem().GenerateID();
        }
#endif

        public abstract ItemBase GetBaseItem();
    }
    
    public class ItemSO<T> : ItemSO where T : ItemBase
    {
        [SerializeField] T item;
     
        public override ItemBase GetBaseItem()
        {
            return GetItem();
        }

        public T GetItem()
        {
            return item;
        }
    }
}