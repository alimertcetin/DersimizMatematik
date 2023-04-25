using UnityEngine;
using UnityEngine.Events;

namespace XIV.ScriptableObjects.Channels
{
    /// <summary>
    /// This class is used for Events that have no arguments (Example: Exit game event)
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Void Event Channel")]
    public class VoidEventChannelSO : EventChannelBaseSO
    {
        public event UnityAction OnEventRaised;

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