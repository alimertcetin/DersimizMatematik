using System;
using System.Collections;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    [SerializeField] private StringEventChannelSO notificationChannel = default;
    [SerializeField] private BoolEventChannelSO BlackBoardUIChannel = default;
    private GamePlayCanvasManager gamePlayCanvas = default;

    private bool triggered = false;

    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();
        gamePlayCanvas = FindObjectOfType<GamePlayCanvasManager>();
    }

    private void OnEnable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed += Interact_performed;
    }

    private void OnDisable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed -= Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (triggered)
        {
            if (gamePlayCanvas.BlackBoardUI_acitveSelf == false)
            {
                BlackBoardUIChannel.RaiseEvent(true);
            }
            else
            {
                BlackBoardUIChannel.RaiseEvent(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;
            notificationChannel.RaiseEvent("Press " + InputManager.InteractionKeyName + " to interact with BlackBoard");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggered)
        {
            if (gamePlayCanvas.BlackBoardUI_acitveSelf)
            {
                notificationChannel.RaiseEvent("", false);
            }
            else
            {
                notificationChannel.RaiseEvent("Press " + InputManager.InteractionKeyName + " to interact with BlackBoard");
            }
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
