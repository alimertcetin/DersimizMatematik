using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, PlayerControls.IGameManagerActions
{
    [SerializeField] private GamePlayCanvasManager gamePlayCanvasManager = default;
    [SerializeField] private BoolEventChannelSO PauseMenuUIChannel = default;

    private void Awake()
    {
        InputManager.PlayerControls.GameManager.SetCallbacks(this);
    }

    private void OnEnable()
    {
        InputManager.GameManager.Enable();

        //InputManager.BlackBoardUIManagement.enabled += CursorManager.Instance.UnlockCursor;
        //InputManager.LockedDoorUI.enabled += CursorManager.Instance.UnlockCursor;

        //InputManager.BlackBoardUIManagement.disabled += CursorManager.Instance.LockCursor;
        //InputManager.LockedDoorUI.disabled += CursorManager.Instance.LockCursor;
    }

    private void OnDisable()
    {
        InputManager.GameManager.Disable();

        //InputManager.BlackBoardUIManagement.enabled -= CursorManager.Instance.UnlockCursor;
        //InputManager.LockedDoorUI.enabled -= CursorManager.Instance.UnlockCursor;

        //InputManager.BlackBoardUIManagement.disabled -= CursorManager.Instance.LockCursor;
        //InputManager.LockedDoorUI.disabled -= CursorManager.Instance.LockCursor;
    }

    private void OnApplicationQuit()
    {
        CursorManager.Instance.UnlockCursor(CursorLockMode.None);
    }

    /// <summary>
    /// Tanımlanan menüler açıksa true döner
    /// </summary>
    private bool MenuIsOpen()
    {
        if (InputManager.PlayerControls.IslemYapUI.enabled)
        {
            return true;
        }
        else if (InputManager.PlayerControls.LockedDoorUI.enabled)
        {
            return true;
        }
        else if (InputManager.PlayerControls.SayiAlUI.enabled)
        {
            return true;
        }
        else if (InputManager.PlayerControls.BlackBoardUIManagement.enabled)
        {
            return true;
        }
        else if (gamePlayCanvasManager.pauseMenu_acitveSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!gamePlayCanvasManager.pauseMenu_acitveSelf)
            {
                CursorManager.Instance.UnlockCursor();
                InputManager.GamePlay.Disable();
                PauseMenuUIChannel.RaiseEvent(true);
            }
            else
            {
                CursorManager.Instance.LockCursor();
                InputManager.GamePlay.Enable();
                PauseMenuUIChannel.RaiseEvent(false);
            }
        }
    }

    //TODO : Consider removing this method and also the input.
    public void OnReloadScene(InputAction.CallbackContext context)
    {
        //if (context.performed)
        //{
        //    //TODO : Use loading system
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}
    }

}
