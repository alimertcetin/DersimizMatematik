using System;
using UnityEngine.InputSystem;

namespace LessonIsMath.Input
{
    public static class InputManager
    {
        static PlayerControls PlayerControls = new PlayerControls();
        public static string InteractionKeyName => PlayerControls.Interaction.Interact.GetBindingDisplayString();

        public static void DisableAllInput()
        {
            GameState.Disable();
            CharacterMovement.Disable();
            Interaction.Disable();
            GameUI.Disable();
            PageUI.Disable();
            MakeOperationUI.Disable();
            EarnNumberUI.Disable();
        }

        public static class GameState
        {
            public static Action GameManagerEnabled = delegate { };
            public static Action GameManagerDisabled = delegate { };

            public static void Enable()
            {
                CursorManager.UnlockCursor();
                PlayerControls.GameState.Enable();
                GameManagerEnabled.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.GameState.Disable();
                GameManagerDisabled.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IGameStateActions instance)
            {
                PlayerControls.GameState.SetCallbacks(instance);
            }
        }

        public static class CharacterMovement
        {
            public static Action enabled = delegate { };
            public static Action disabled = delegate { };

            public static void Enable()
            {
                CursorManager.LockCursor();
                PlayerControls.CharacterMovement.Enable();
                enabled.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.CharacterMovement.Disable();
                disabled.Invoke();
            }

            public static void SetCallbacks(PlayerControls.ICharacterMovementActions instance)
            {
                PlayerControls.CharacterMovement.SetCallbacks(instance);
            }
        }

        public static class Interaction
        {
            public static Action enabled = delegate { };
            public static Action disabled = delegate { };

            public static void Enable()
            {
                PlayerControls.Interaction.Enable();
                enabled.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.Interaction.Disable();
                disabled.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IInteractionActions instance)
            {
                PlayerControls.Interaction.SetCallbacks(instance);
            }
        }

        public static class GameUI
        {
            public static Action enabled = delegate { };
            public static Action disabled = delegate { };

            public static void Enable()
            {
                PlayerControls.GameUI.Enable();
                enabled.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.GameUI.Disable();
                disabled.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IGameUIActions instance)
            {
                PlayerControls.GameUI.SetCallbacks(instance);
            }
        }

        public static class PageUI
        {
            public static Action enabled = delegate { };
            public static Action disabled = delegate { };

            public static void Enable()
            {
                PlayerControls.PageUI.Enable();
                enabled.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.PageUI.Disable();
                disabled.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IPageUIActions instance)
            {
                PlayerControls.PageUI.SetCallbacks(instance);
            }
        }

        public static class MakeOperationUI
        {
            public static Action MakeOperationUI_Enabled = delegate { };
            public static Action MakeOperationUI_Disabled = delegate { };

            public static void Enable()
            {
                CursorManager.UnlockCursor();
                Keypad.Enable();
                PageUI.Enable();
                PlayerControls.MakeOperationUI.Enable();
                MakeOperationUI_Enabled.Invoke();
            }

            public static void Disable()
            {
                Keypad.Disable();
                PageUI.Disable();
                PlayerControls.MakeOperationUI.Disable();
                MakeOperationUI_Disabled.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IMakeOperationUIActions instance)
            {
                PlayerControls.MakeOperationUI.SetCallbacks(instance);
            }
        }

        public static class EarnNumberUI
        {
            public static Action EarnNumberUI_Enabled = delegate { };
            public static Action EarnNumberUI_Disabled = delegate { };

            public static void Enable()
            {
                CursorManager.UnlockCursor();
                Keypad.Enable();
                PageUI.Enable();
                PlayerControls.EarnNumberUI.Enable();
                EarnNumberUI_Enabled.Invoke();
            }

            public static void Disable()
            {
                Keypad.Disable();
                PageUI.Disable();
                PlayerControls.EarnNumberUI.Disable();
                EarnNumberUI_Disabled.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IEarnNumberUIActions instance)
            {
                PlayerControls.EarnNumberUI.SetCallbacks(instance);
            }
        }

        public static class Keypad
        {
            public static Action enabled = delegate { };
            public static Action disabled = delegate { };

            public static void Enable()
            {
                PlayerControls.Keypad.Enable();
                enabled.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.Keypad.Disable();
                disabled.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IKeypadActions instance)
            {
                PlayerControls.Keypad.SetCallbacks(instance);
            }
        }

    }
}