using System;
using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, PlayerControls.IGameStateActions
{
    [SerializeField] private BoolEventChannelSO PauseMenuUIChannel = default;
    bool isPaused = false;

    private void Awake()
    {
        InputManager.GameState.SetCallbacks(this);
    }

    private void OnEnable()
    {
        InputManager.GameState.Enable();
    }

    private void OnDisable()
    {
        InputManager.GameState.Disable();
    }

    private void OnApplicationQuit()
    {
        CursorManager.UnlockCursor(CursorLockMode.None);
    }

    void PlayerControls.IGameStateActions.OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed == false) return;

        if (!isPaused)
        {
            CursorManager.UnlockCursor();
            InputManager.CharacterMovement.Disable();
            PauseMenuUIChannel.RaiseEvent(true);
        }
        else
        {
            CursorManager.LockCursor();
            InputManager.CharacterMovement.Enable();
            PauseMenuUIChannel.RaiseEvent(false);
        }
        isPaused = !isPaused;
    }

}
