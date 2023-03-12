using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;
using LessonIsMath.Input;
using LessonIsMath.Tween;
using LessonIsMath.UI.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.EventSystem;

namespace LessonIsMath.UI
{
    public class Keypad : MonoBehaviour, PlayerControls.IKeypadActions
    {
        [SerializeField] CustomButton btn_Enter;
        [SerializeField] CustomButton btn_Delete;
        [SerializeField] CustomButton[] btn_Numbers;
        IKeypadListener listener;
        bool isActive;
        bool hasOpeningTween;

        public void SetListener(IKeypadListener listener)
        {
            this.listener = listener;
        }

        public void Enable()
        {
            isActive = true;
            hasOpeningTween = true;
            XIVEventSystem.SendEvent(new XIVInvokeUntilEvent().AddCondition(() =>
            {
                hasOpeningTween = TryGetComponent<ITween>(out _);
                return hasOpeningTween == false;
            }));
            Register();
            InputManager.Keypad.SetCallbacks(this);
        }

        public void Disable()
        {
            isActive = false;
            Unregister();
            btn_Delete.TweenCancelAll();
            btn_Enter.TweenCancelAll();
            for (int i = 0; i < btn_Numbers.Length; i++)
            {
                btn_Numbers[i].TweenCancelAll();
            }
        }

        void OnEnter()
        {
            if (isActive == false) return;
            listener?.OnEnter();
            if (hasOpeningTween) return;
            btn_Enter.ClickTween(0.1f);
        }

        void OnDeleteStarted()
        {
            if (isActive == false) return;
            listener?.OnDeleteStarted();
            if (hasOpeningTween) return;
            btn_Delete.ClickTween(0.1f);
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
            if (hasOpeningTween) return;
            btn_Numbers[val].ClickTween(0.1f);
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