using System;
using System.Collections;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    [SerializeField] private StringEventChannelSO notificationChannel = default;
    [SerializeField] private BoolEventChannelSO BlackBoardUIChannel = default;

    private bool triggered = false;

    private void OnEnable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed += Interact_performed;
        BlackBoardUIChannel.OnEventRaised += OnBlackBoardInteract;
    }

    private void OnDisable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed -= Interact_performed;
        BlackBoardUIChannel.OnEventRaised -= OnBlackBoardInteract;
    }

    private void OnBlackBoardInteract(bool value)
    {
        if (triggered == false) return;

        if (value)
        {
            notificationChannel.RaiseEvent("", false);
        }
        else
        {
            notificationChannel.RaiseEvent("Press " + InputManager.InteractionKeyName + " to interact with BlackBoard");
        }
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (triggered == false) return;

        BlackBoardUIChannel.RaiseEvent(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;
            notificationChannel.RaiseEvent("Press " + InputManager.InteractionKeyName + " to interact with BlackBoard");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = false;
            notificationChannel.RaiseEvent("", false);
        }
    }
}
