﻿using System;
using UnityEngine;
using LessonIsMath.DoorSystems;

namespace LessonIsMath.ScriptableObjects.ChannelSOs
{
    [CreateAssetMenu(menuName = "Events/Arithmetic Door Event Channel")]
    public class ArithmeticDoorEventChannelSO : EventChannelBaseSO
    {
        public Action<ArithmeticOperationDoor, bool> OnEventRaised;

        public void RaiseEvent(ArithmeticOperationDoor arithmeticOperationDoor, bool value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(arithmeticOperationDoor, value);
            else
            {
                Debug.LogWarning("A ShowLockedDoorUI was requested, but nobody picked it up. " + "Check why there is no LockedDoor_UI_Manager already present, " + "and make sure it's listening on this Event channel.");
            }
        }
    }
}