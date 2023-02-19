using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using XIV.SaveSystems;

namespace LessonIsMath.UI
{
    public class PausedMenu_UI : GameUI
    {
        [Header("Broadcasting To")]
        [SerializeField] private StringEventChannelSO WarningUIChannel = default;
        [SerializeField] private LoadEventChannelSO onExitPressed = default;

        [Header("Scene To Unload")]
        [Tooltip("If exit pressed, unload this scene")]
        [SerializeField] private GameSceneSO gamePlayScene = default;

        [Header("Scenes To Load")]
        [SerializeField] private GameSceneSO mainMenu = default;

        public void btn_Load()
        {
            SaveSystem.Load();
        }

        public void btn_Save()
        {
            SaveSystem.Save();
            WarningUIChannel.RaiseEvent("Kayıt Edildi : " + Application.persistentDataPath);
        }

        public void btn_Settings()
        {
            UISystem.Show<Settings_UI>();
        }

        public void btn_Exit()
        {
            onExitPressed.RaiseEvent(mainMenu, true);
            InputManager.DisableAllInput();
            gamePlayScene.sceneReference.UnLoadScene();
            gameObject.SetActive(false);
        }
    }
}

