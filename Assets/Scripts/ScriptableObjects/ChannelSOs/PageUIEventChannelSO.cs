using UnityEngine;
using UnityEngine.Events;
using LessonIsMath.UI;
using XIV.ScriptableObjects.Channels;

namespace LessonIsMath.ScriptableObjects.ChannelSOs
{
    [CreateAssetMenu(menuName = "Events/PageUI Event ChannelSO")]
    public class PageUIEventChannelSO : EventChannelBaseSO
    {
        public UnityAction<PageUI> OnEventRaised;
        
        public void RaiseEvent(PageUI pageUI)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(pageUI);
            else Debug.LogWarning("A PageUI event raised but nobody picked up.");
        }
    }
}