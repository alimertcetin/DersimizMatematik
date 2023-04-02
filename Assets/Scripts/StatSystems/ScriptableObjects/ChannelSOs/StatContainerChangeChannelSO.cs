using System;
using UnityEngine;

namespace LessonIsMath.StatSystems.ScriptableObjects.ChannelSOs
{
    [CreateAssetMenu(menuName = "ChannelSOs/Stats/StatContainerChangeChannelSO")]
    public class StatContainerChangeChannelSO : ScriptableObject
    {
        Action<StatContainerChange> action;

        public void Register(Action<StatContainerChange> action)
        {
            this.action += action;
        }

        public void Unregister(Action<StatContainerChange> action)
        {
            this.action -= action;
        }
        
        public void RaiseEvent(StatContainerChange change)
        {
            action?.Invoke(change);
        }
    }
}