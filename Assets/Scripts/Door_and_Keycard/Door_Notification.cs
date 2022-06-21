using System;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class Door_Notification : MonoBehaviour
{
    [SerializeField] private StringEventChannelSO notificationChannel = default;
    [SerializeField] private StringEventChannelSO WarningUIChannel = default;

    private Door_Is_Locked doorIsLocked;
    private DoorKeycard_Management keycardManager;
    private Door_Animation doorAnimation;
    private bool triggered;
    private bool doorLocked;
    private bool keycardsAreRemoved;

    private void Awake()
    {
        doorAnimation = GetComponent<Door_Animation>();

        if (TryGetComponent<Door_Is_Locked>(out doorIsLocked))
        {
            doorIsLocked.KapiAcildi += doorLocked_KapiAcildi;
            doorLocked = doorIsLocked.DoorLocked;
        }

        if (TryGetComponent<DoorKeycard_Management>(out keycardManager))
        {
            keycardManager.AllKeycardsRemoved += KeycardsRemovedFromDoor;
            keycardManager.KeycardRemoved += RefreshNotification;
        }
        else
        {
            keycardsAreRemoved = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (doorLocked)
            {
                notificationChannel.RaiseEvent("Door is locked. Press " + InputManager.InteractionKeyName
                    + " button to see the Question.");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggered)
        {
            RefreshNotification();
        }
    }

    private void RefreshNotification()
    {
        if (InputManager.PlayerControls.LockedDoorUI.enabled)
        {
            notificationChannel.RaiseEvent(null, false);
        }
        else if (!doorLocked && !keycardsAreRemoved)
        {
            notificationChannel.RaiseEvent(keycardManager.Door_Keycard_NotificationText());
        }
        else if (!doorLocked && keycardsAreRemoved)
        {
            notificationChannel.RaiseEvent(GiveInfo_DoorIsOpen_OrNot(doorAnimation.DoorIsOpen));
        }
        else if (doorLocked)
        {
            notificationChannel.RaiseEvent("Door is locked. Press " + InputManager.InteractionKeyName
                + " button to see the Question.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = false;
            notificationChannel.RaiseEvent(null, false);
        }
    }

    private void KeycardsRemovedFromDoor()
    {
        keycardManager = null;
        keycardsAreRemoved = true;
    }

    private void doorLocked_KapiAcildi(Door_Is_Locked door)
    {
        doorIsLocked = null;
        doorLocked = false;
    }

    private string GiveInfo_DoorIsOpen_OrNot(bool isDoorOpen)
    {
        if (isDoorOpen)
        {
            return "Press " + InputManager.InteractionKeyName + " to Close";
        }
        else
        {
            return "Press " + InputManager.InteractionKeyName + " to Open";
        }
    }

    public void Warn(string text, bool value = true)
    {
        WarningUIChannel.RaiseEvent(text, value);
    }
}
