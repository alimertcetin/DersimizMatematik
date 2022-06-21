using System.Collections.Generic;
using UnityEngine;

public enum DoorStatus
{
    Locked,
    JustOpenAndClose,
    KeycardRequired
}

[RequireComponent(typeof(SaveableEntity))]
[RequireComponent(typeof(Door_Notification))]
[RequireComponent(typeof(Door_Animation))]
[DisallowMultipleComponent]
public class Door_Status : MonoBehaviour
{
    [Header("Select a status for this door.")]
    public List<DoorStatus> Door_Status_List;
}
