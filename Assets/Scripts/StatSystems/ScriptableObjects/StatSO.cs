using UnityEngine;

namespace LessonIsMath.StatSystems.ScriptableObjects
{
    public abstract class StatSO : ScriptableObject
    {
        public abstract StatItemBase GetBaseItem();
        
#if UNITY_EDITOR
        [ContextMenu(nameof(ResetStat))]
        void ResetStat()
        {
            var item = GetBaseItem();
            for (int i = 0; i < item.levels.Length; i++)
            {
                item.levels[i].statData.current = item.levels[i].statData.max;
            }
            item.currentLevel = 0;
        }
        
        [ContextMenu(nameof(FillCurrent))]
        void FillCurrent()
        {
            var item = GetBaseItem();
            item.statData.current = item.statData.max;
        }
#endif
    }
    
    public class StatSO<T> : StatSO where T : StatItemBase
    {
        [SerializeField] T item;
        
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