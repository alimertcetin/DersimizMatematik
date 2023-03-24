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
            public static Action OnEnable = delegate { };
            public static Action OnDisable = delegate { };

            public static void Enable()
            {
                // TODO : Should not call any unrelated functions
                CursorManager.UnlockCursor();
                PlayerControls.GameState.Enable();
                OnEnable.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.GameState.Disable();
                OnDisable.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IGameStateActions instance)
            {
                PlayerControls.GameState.SetCallbacks(instance);
            }
        }

        public static class CharacterMovement
        {
            public static Action OnEnable = delegate { };
            public static Action OnDisable = delegate { };

            public static void Enable()
            {
                // TODO : Should not call any unrelated functions
                CursorManager.LockCursor();
                PlayerControls.CharacterMovement.Enable();
                OnEnable.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.CharacterMovement.Disable();
                OnDisable.Invoke();
            }

            public static void SetCallbacks(PlayerControls.ICharacterMovementActions instance)
            {
                PlayerControls.CharacterMovement.SetCallbacks(instance);
            }
        }

        public static class Interaction
        {
            public static Action OnEnable = delegate { };
            public static Action OnDisable = delegate { };

            public static void Enable()
            {
                PlayerControls.Interaction.Enable();
                OnEnable.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.Interaction.Disable();
                OnDisable.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IInteractionActions instance)
            {
                PlayerControls.Interaction.SetCallbacks(instance);
            }
        }

        public static class GameUI
        {
            public static Action OnEnable = delegate { };
            public static Action OnDisable = delegate { };

            public static void Enable()
            {
                PlayerControls.GameUI.Enable();
                OnEnable.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.GameUI.Disable();
                OnDisable.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IGameUIActions instance)
            {
                PlayerControls.GameUI.SetCallbacks(instance);
            }
        }

        public static class PageUI
        {
            public static Action OnEnable = delegate { };
            public static Action OnDisable = delegate { };

            public static void Enable()
            {
                PlayerControls.PageUI.Enable();
                OnEnable.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.PageUI.Disable();
                OnDisable.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IPageUIActions instance)
            {
                PlayerControls.PageUI.SetCallbacks(instance);
            }
        }

        public static class MakeOperationUI
        {
            public static Action OnEnable = delegate { };
            public static Action OnDisable = delegate { };

            public static void Enable()
            {
                PlayerControls.MakeOperationUI.Enable();
                OnEnable.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.MakeOperationUI.Disable();
                OnDisable.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IMakeOperationUIActions instance)
            {
                PlayerControls.MakeOperationUI.SetCallbacks(instance);
            }
        }

        public static class EarnNumberUI
        {
            public static Action OnEnable = delegate { };
            public static Action OnDisable = delegate { };

            public static void Enable()
            {
                PlayerControls.EarnNumberUI.Enable();
                OnEnable.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.EarnNumberUI.Disable();
                OnDisable.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IEarnNumberUIActions instance)
            {
                PlayerControls.EarnNumberUI.SetCallbacks(instance);
            }
        }

        public static class Keypad
        {
            public static Action OnEnable = delegate { };
            public static Action OnDisable = delegate { };

            public static void Enable()
            {
                PlayerControls.Keypad.Enable();
                OnEnable.Invoke();
            }

            public static void Disable()
            {
                PlayerControls.Keypad.Disable();
                OnDisable.Invoke();
            }

            public static void SetCallbacks(PlayerControls.IKeypadActions instance)
            {
                PlayerControls.Keypad.SetCallbacks(instance);
            }
        }

    }
}