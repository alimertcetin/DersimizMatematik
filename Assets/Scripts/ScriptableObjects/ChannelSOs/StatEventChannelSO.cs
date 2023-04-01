using UnityEngine;
using UnityEngine.Events;
using LessonIsMath.StatSystems;

namespace LessonIsMath.ScriptableObjects.ChannelSOs
{
    [CreateAssetMenu(menuName = "Events/Stat Event ChannelSO")]
    public class StatEventChannelSO : EventChannelBaseSO
    {
        public UnityAction<IStat> OnEventRaised;
        
        public void RaiseEvent(IStat stat)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(stat);
            else Debug.LogWarning("An IStat event raised but nobody picked up.");
        }
        
    }
}