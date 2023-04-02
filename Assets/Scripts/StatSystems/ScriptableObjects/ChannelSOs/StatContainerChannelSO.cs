using System;
using UnityEngine;

namespace LessonIsMath.StatSystems.ScriptableObjects.ChannelSOs
{
    [CreateAssetMenu(menuName = "ChannelSOs/Stats/StatContainerChannelSO")]
    public class StatContainerChannelSO : ScriptableObject
    {
        Action<StatContainer> action;

        public void Register(Action<StatContainer> action)
        {
            this.action += action;
        }

        public void Unregister(Action<StatContainer> action)
        {
            this.action -= action;
        }
        
        public void RaiseEvent(StatContainer inventory)
        {
            action?.Invoke(inventory);
        }
    }
}