using System.Collections.Generic;
using UnityEngine;

namespace LessonIsMath.StatSystems.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Stats/StatContainerSO")]
    public class StatContainerSO : ScriptableObject
    {
        public List<StatSO> items;

        public StatContainer GetStatContainer()
        {
            var statContainer = new StatContainer();

            int count = items.Count;
            for (var i = 0; i < count; i++)
            {
                StatSO statSO = items[i];
#if UNITY_EDITOR
                if (statContainer.Contains(statSO.GetBaseItem()))
                {
                    Debug.LogError("This stat is already exists. Duplicates are not allowed. Will override previous stat. Index : " + i);
                }
#endif
                statContainer.Add(statSO.GetBaseItem());
            }
            
            return statContainer;
        }
    }
}