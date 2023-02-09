using System;
using UnityEngine;
using GameCore.DoorSystem;

namespace GameCore.ScriptableObjects.Channels
{
    [CreateAssetMenu(menuName = "Events/LockedDoorUI Event Channel")]
    public class ShowLockedDoorUIEventChannelSO : EventChannelBaseSO
    {
        public Action<Door> ShowLockedDoorUI;

        public void RaiseEvent(Door door)
        {
            if (ShowLockedDoorUI != null)
                ShowLockedDoorUI.Invoke(door);
            else
            {
                Debug.LogWarning("A ShowLockedDoorUI was requested, but nobody picked it up. " +
                    "Check why there is no LockedDoor_UI_Manager already present, " +
                    "and make sure it's listening on this Event channel.");
            }
        }
    }
}