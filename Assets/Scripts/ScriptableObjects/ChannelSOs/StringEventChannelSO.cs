using UnityEngine;
using UnityEngine.Events;

namespace LessonIsMath.ScriptableObjects.ChannelSOs
{
    /// <summary>
    /// This class is used for Events that have a string argument.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/String Event Channel")]
    public class StringEventChannelSO : EventChannelBaseSO
    {
        public UnityAction<string, bool> OnEventRaised;

        /// <param name="strValue">Leave empty "" if boolValue = false</param>
        /// <param name="boolValue">Show the text</param>
        public void RaiseEvent(string strValue = "", bool boolValue = true)
        {
            if (OnEventRaised != null)
            {
                OnEventRaised.Invoke(strValue, boolValue);
            }
            else
            {
                Debug.LogWarning("Nobody picked up the string event.");
            }
        }
    }
}