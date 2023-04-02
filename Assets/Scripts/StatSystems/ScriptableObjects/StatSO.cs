using UnityEngine;

namespace LessonIsMath.StatSystems.ScriptableObjects
{
    public abstract class StatSO : ScriptableObject
    {
        public abstract StatItemBase GetBaseItem();
    }
    
    public class StatSO<T> : StatSO where T : StatItemBase
    {
        public T item;
        
        public override StatItemBase GetBaseItem()
        {
            return GetItem();
        }

        public T GetItem()
        {
            return item;
        }
    }
}