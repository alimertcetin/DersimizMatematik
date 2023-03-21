using UnityEngine;
using UnityEngine.Events;

namespace LessonIsMath.ScriptableObjects.ChannelSOs
{

    /// <summary>
    /// This class is used for Events that have no arguments (Example: Exit game event)
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Void Event Channel")]
    public class VoidEventChannelSO : EventChannelBaseSO
    {
        public UnityAction OnEventRaised;

#if UNITY_EDITOR
        [ContextMenu(nameof(RaiseEvent))]
#endif
        public void RaiseEvent()
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke();
        }

    }
}