using System.Collections;
using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI.Components;
using LessonIsMath.ScriptableObjects.SceneSOs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using XIV.SaveSystems;

namespace LessonIsMath.UI
{
    public class PauseUI : ParentGameUI, PlayerControls.IGameUIActions
    {
        [SerializeField] BoolEventChannelSO pauseMenuUIChannel;
        [SerializeField] BoolEventChannelSO showSaveIndicatorChannel;
        [SerializeField] BoolEventChannelSO showLoadingScreenChannel;
        
        [SerializeField] LoadEventChannelSO locationLoadChannel;

        [Tooltip("If exit pressed, unload this scene")]
        [SerializeField] GameSceneSO gamePlayScene;
        [SerializeField] GameSceneSO mainMenu;

        [SerializeField] SettingsUIPage settingsUIPage;
        [SerializeField] CustomButton btn_Resume;
        [SerializeField] CustomButton btn_Load;
        [SerializeField] CustomButton btn_Save;
        [SerializeField] CustomButton btn_Settings;
        [SerializeField] CustomButton btn_Exit;

        static readonly WaitForSeconds waitSaveSystem = new WaitForSeconds(2f);

        void OnEnable()
        {
            btn_Resume.RegisterOnClick(Resume);
            btn_Save.RegisterOnClick(Save);
            btn_Load.RegisterOnClick(Load);
            btn_Settings.RegisterOnClick(ShowSettings);
            btn_Exit.RegisterOnClick(OnExit);
        }

        void OnDisable()
        {
            btn_Resume.UnregisterOnClick();
            btn_Save.UnregisterOnClick();
            btn_Load.UnregisterOnClick();
            btn_Settings.UnregisterOnClick();
            btn_Exit.UnregisterOnClick();
        }

        public override void Show()
        {
            InputManager.CharacterMovement.Disable();
            InputManager.GameState.Disable();
            InputManager.GameUI.SetCallbacks(this);
            InputManager.GameUI.Enable();
            ButtonSetActive(btn_Load, SaveSystem.IsSaveExists());
            base.Show();
        }

        public override void Hide()
        {
            InputManager.CharacterMovement.Enable();
            InputManager.GameState.Enable();
            InputManager.GameUI.Disable();
            base.Hide();
        }

        static void ButtonSetActive(Button button, bool value)
        {
            button.interactable = value;
            button.targetGraphic.raycastTarget = value;
        }

        public override void ComeBack(PageUI from)
        {
            InputManager.GameUI.Enable();
            base.ComeBack(from);
        }

        void Resume()
        {
            pauseMenuUIChannel.RaiseEvent(false);
        }

        void Save()
        {
            StartCoroutine(HandleSave());
        }

        void Load()
        {
            StartCoroutine(HandleLoad());
        }

        IEnumerator HandleSave()
        {
            showSaveIndicatorChannel.RaiseEvent(true);
            yield return SaveSystem.SaveAsync();
            yield return waitSaveSystem;
            showSaveIndicatorChannel.RaiseEvent(false);
        }

        IEnumerator HandleLoad()
        {
            showLoadingScreenChannel.RaiseEvent(true);
            yield return SaveSystem.LoadAsync();
            Resume();
            yield return waitSaveSystem;
            showLoadingScreenChannel.RaiseEvent(false);
        }

        void ShowSettings()
        {
            InputManager.GameUI.Disable();
            OpenPage(settingsUIPage);
        }

        void OnExit()
        {
            locationLoadChannel.RaiseEvent(mainMenu, true);
            InputManager.DisableAllInput();
            gamePlayScene.sceneReference.UnLoadScene();
            gameObject.SetActive(false);
        }

        void PlayerControls.IGameUIActions.OnExit(InputAction.CallbackContext context)
        {
            if (context.performed == false) return;
            Resume();
        }
    }
}

