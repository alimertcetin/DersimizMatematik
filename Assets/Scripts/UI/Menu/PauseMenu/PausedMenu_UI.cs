﻿using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.SaveSystems;

namespace LessonIsMath.UI
{
    public class PausedMenu_UI : GameUI, PlayerControls.IGameUIActions
    {
        [SerializeField] BoolEventChannelSO pauseMenuUIChannel;
        [Header("Broadcasting To")]
        [SerializeField] StringEventChannelSO warningUIChannel;
        [SerializeField] LoadEventChannelSO locationLoadChannel;

        [Header("Scene To Unload")]
        [Tooltip("If exit pressed, unload this scene")]
        [SerializeField] GameSceneSO gamePlayScene;

        [Header("Scenes To Load")]
        [SerializeField] GameSceneSO mainMenu;

        [SerializeField] GameObject mainPage;
        [SerializeField] SettingsUIPage settingsUIPage;
        [SerializeField] CustomButton btn_Resume;
        [SerializeField] CustomButton btn_Load;
        [SerializeField] CustomButton btn_Save;
        [SerializeField] CustomButton btn_Settings;
        [SerializeField] CustomButton btn_Exit;

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
            InputManager.GameState.Disable();
            InputManager.GameUI.SetCallbacks(this);
            InputManager.GameUI.Enable();
            InputManager.Interaction.Disable();
            base.Show();
            mainPage.SetActive(true);
        }

        public override void Hide()
        {
            InputManager.GameState.Enable();
            InputManager.GameUI.Disable();
            InputManager.Interaction.Enable();
            base.Hide();
        }

        void Resume()
        {
            pauseMenuUIChannel.RaiseEvent(false);
        }

        void Save()
        {
            SaveSystem.Save();
            warningUIChannel.RaiseEvent("Saved : " + Application.persistentDataPath);
        }

        void Load()
        {
            SaveSystem.Load();
        }

        void ShowSettings()
        {
            InputManager.GameUI.Disable();
            settingsUIPage.Show();
            mainPage.SetActive(false);
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

