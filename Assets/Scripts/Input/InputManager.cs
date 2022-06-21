using System;

public static class InputManager
{
    public static PlayerControls PlayerControls = new PlayerControls();
    public static string InteractionKeyName { get => PlayerControls.Gameplay.Interact.name; }

    public static void DisableAllInput()
    {
        GamePlay.Disable();
        SayiAlUI.Disable();
        IslemYapUI.Disable();
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

    public static class IslemYapUI
    {
        public static Action IslemYapUI_Enabled = delegate { };
        public static Action IslemYapUI_Disabled = delegate { };

        public static void Enable()
        {
            CursorManager.Instance.UnlockCursor();
            PlayerControls.IslemYapUI.Enable();
            IslemYapUI_Enabled.Invoke();
        }

        public static void Disable()
        {
            PlayerControls.IslemYapUI.Disable();
            IslemYapUI_Disabled.Invoke();
        }
    }

    public static class SayiAlUI
    {
        public static Action SayiAlUI_Enabled = delegate { };
        public static Action SayiAlUI_Disabled = delegate { };

        public static void Enable()
        {
            CursorManager.Instance.UnlockCursor();
            PlayerControls.SayiAlUI.Enable();
            SayiAlUI_Enabled.Invoke();
        }

        public static void Disable()
        {
            PlayerControls.SayiAlUI.Disable();
            SayiAlUI_Disabled.Invoke();
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
