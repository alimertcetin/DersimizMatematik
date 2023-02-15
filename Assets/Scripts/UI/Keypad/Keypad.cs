using LessonIsMath.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LessonIsMath.UI
{
    public class Keypad : MonoBehaviour, PlayerControls.IKeypadActions
    {
        [SerializeField] CustomButton btn_Enter;
        [SerializeField] CustomButton btn_Delete;
        [SerializeField] CustomButton[] btn_Numbers;
        IKeypadListener listener;
        bool isActive;

        public void SetListener(IKeypadListener listener)
        {
            this.listener = listener;
        }

        public void Enable()
        {
            isActive = true;
            Register();
            InputManager.PlayerControls.Keypad.SetCallbacks(this);
        }

        public void Disable()
        {
            isActive = false;
            Unregister();
        }

        void OnEnter()
        {
            if (isActive == false) return;
            listener?.OnEnter();
        }

        void OnDeleteStarted()
        {
            if (isActive == false) return;
            listener?.OnDeleteStarted();
        }

        void OnDeleteCanceled()
        {
            if (isActive == false) return;
            listener?.OnDeleteCanceled();
        }

        void OnNumberPressed(int val)
        {
            if (isActive == false) return;
            listener?.OnNumberPressed(val);
        }

        void Register()
        {
            btn_Enter.RegisterOnClick(OnEnter);
            btn_Delete.RegisterOnClick(OnDeleteStarted);
            btn_Delete.RegisterOnPointerUp(OnDeleteCanceled);

            for (int i = 0; i < btn_Numbers.Length; i++)
            {
                int val = i;
                btn_Numbers[i].RegisterOnClick(() => OnNumberPressed(val));
            }
        }

        void Unregister()
        {
            btn_Enter.UnregisterOnClick();
            btn_Delete.UnregisterOnClick();
            btn_Delete.UnregisterOnPointerUp();

            for (int i = 0; i < btn_Numbers.Length; i++)
            {
                btn_Numbers[i].UnregisterOnClick();
            }
        }

        public void OnZero(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberPressed(0);
        }

        public void OnOne(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberPressed(1);
        }

        public void OnTwo(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberPressed(2);
        }

        public void OnThree(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberPressed(3);
        }

        public void OnFour(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberPressed(4);
        }

        public void OnFive(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberPressed(5);
        }

        public void OnSix(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberPressed(6);
        }

        public void OnSeven(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberPressed(7);
        }

        public void OnEight(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberPressed(8);
        }

        public void OnNine(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberPressed(9);
        }

        public void OnEnter(InputAction.CallbackContext context)
        {
            if (context.performed) OnEnter();
        }

        public void OnDelete(InputAction.CallbackContext context)
        {
            if (context.performed) OnDeleteStarted();
            else if (context.canceled) OnDeleteCanceled();
        }
    }
}