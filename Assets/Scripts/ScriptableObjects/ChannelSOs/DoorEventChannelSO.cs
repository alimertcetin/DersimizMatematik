using System;
using UnityEngine;
using LessonIsMath.DoorSystems;

namespace LessonIsMath.ScriptableObjects.ChannelSOs
{
    [CreateAssetMenu(menuName = "Events/Door Event Channel")]
    public class DoorEventChannelSO : EventChannelBaseSO
    {
        public Action<Door, bool> OnEventRaised;

        public void RaiseEvent(Door door, bool value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(door, value);
            else
            {
                Debug.LogWarning("A ShowLockedDoorUI was requested, but nobody picked it up. " + "Check why there is no LockedDoor_UI_Manager already present, " + "and make sure it's listening on this Event channel.");
            }
        }
    }
}