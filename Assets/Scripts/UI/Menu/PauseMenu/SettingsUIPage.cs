using ElRaccoone.Tweens;
using LessonIsMath.Input;
using UnityEngine;
using LessonIsMath.UI.Components;
using UnityEngine.InputSystem;

namespace LessonIsMath.UI
{
    public class SettingsUIPage : PageUI, PlayerControls.IPageUIActions
    {
        [SerializeField] CustomButton btn_Back;

        void OnEnable()
        {
            btn_Back.RegisterOnClick(OnBackPressed);
        }

        void OnDisable()
        {
            btn_Back.UnregisterOnClick();
        }

        public override void Show()
        {
            base.Show();
            InputManager.PageUI.SetCallbacks(this);
            InputManager.PageUI.Enable();
        }

        public override void Hide()
        {
            InputManager.PageUI.Disable();
            uiGameObject.transform.localScale = Vector3.one;
            uiGameObject.TweenCancelAll();
            uiGameObject.TweenLocalScale(Vector3.zero, 0.5f)
                .SetEaseExpoInOut()
                .SetOnComplete(() =>
                {
                    uiGameObject.SetActive(false);
                    isActive = false;
                    UISystem.Show<PausedMenu_UI>();
                });
        }

        void OnBackPressed()
        {
            Hide();
        }

        void PlayerControls.IPageUIActions.OnBack(InputAction.CallbackContext context)
        {
            if (context.performed == false) return;
            Hide();
        }
    }
}