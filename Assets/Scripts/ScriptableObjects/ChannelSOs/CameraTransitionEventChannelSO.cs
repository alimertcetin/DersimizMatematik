using System;
using UnityEngine;

namespace LessonIsMath.ScriptableObjects.ChannelSOs
{
    [CreateAssetMenu(menuName = "Events/Camera Transition Event Channel")]
    public class CameraTransitionEventChannelSO : EventChannelBaseSO
    {
        public Action<LessonIsMath.CameraSystems.CameraType> OnEventRaised;

        public void RaiseEvent(LessonIsMath.CameraSystems.CameraType cameraType)
        {
            if (OnEventRaised != null) OnEventRaised.Invoke(cameraType);
            else Debug.LogWarning("A camera transition was requested, but nobody picked it up.");
        }
    }
}