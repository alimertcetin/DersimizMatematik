using System;
using UnityEngine.InputSystem;

public static class InputManager
{
    public static PlayerControls PlayerControls = new PlayerControls();
    public static string InteractionKeyName => PlayerControls.Gameplay.Interact.GetBindingDisplayString();

    public static void DisableAllInput()
    {
        GamePlay.Disable();
        EarnNumberUI.Disable();
        MakeOperationUI.Disable();
        GameManager.Disable();
        LockedDoorUI.Disable();
        BlackBoardUIManagement.Disable();
    }

    public static class GamePlay
    {
        public static Action enabled = delegate { };
        public static Action disabled = delegate { };

        public static void Enable()
        {
            CursorManager.Instance.LockCursor();
            PlayerControls.Gameplay.Enable();
            enabled.Invoke();
        }

        public static void Disable()
        {
            PlayerControls.Gameplay.Disable();
            disabled.Invoke();
        }
    }

    public static class LockedDoorUI
    {
        public static Action enabled = delegate { };
        public static Action disabled = delegate { };

        public static void Enable()
        {
            CursorManager.Instance.UnlockCursor();
            PlayerControls.LockedDoorUI.Enable();
            enabled.Invoke();
        }

        public static void Disable()
        {
            PlayerControls.LockedDoorUI.Disable();
            disabled.Invoke();
        }

    }

    public static class MakeOperationUI
    {
        public static Action MakeOperationUI_Enabled = delegate { };
        public static Action MakeOperationUI_Disabled = delegate { };

        public static void Enable()
        {
            CursorManager.Instance.UnlockCursor();
            PlayerControls.MakeOperationUI.Enable();
            MakeOperationUI_Enabled.Invoke();
        }

        public static void Disable()
        {
            PlayerControls.MakeOperationUI.Disable();
            MakeOperationUI_Disabled.Invoke();
        }
    }

    public static class EarnNumberUI
    {
        public static Action EarnNumberUI_Enabled = delegate { };
        public static Action EarnNumberUI_Disabled = delegate { };

        public static void Enable()
        {
            CursorManager.Instance.UnlockCursor();
            PlayerControls.EarnNumberUI.Enable();
            EarnNumberUI_Enabled.Invoke();
        }

        public static void Disable()
        {
            PlayerControls.EarnNumberUI.Disable();
            EarnNumberUI_Disabled.Invoke();
        }
    }

    public static class GameManager
    {
        public static Action GameManagerEnabled = delegate { };
        public static Action GameManagerDisabled = delegate { };

        public static void Enable()
        {
            CursorManager.Instance.UnlockCursor();
            PlayerControls.GameManager.Enable();
            GameManagerEnabled.Invoke();
        }

        public static void Disable()
        {
            PlayerControls.GameManager.Disable();
            GameManagerDisabled.Invoke();
        }
    }

    public static class BlackBoardUIManagement
    {
        public static Action enabled = delegate { };
        public static Action disabled = delegate { };

        public static void Enable()
        {
            CursorManager.Instance.UnlockCursor();
            PlayerControls.BlackBoardUIManagement.Enable();
            enabled.Invoke();
        }

        public static void Disable()
        {
            PlayerControls.BlackBoardUIManagement.Disable();
            disabled.Invoke();
        }
    }

}
