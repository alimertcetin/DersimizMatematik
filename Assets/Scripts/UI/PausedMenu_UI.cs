using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using XIV.SaveSystems;

namespace LessonIsMath.UI
{
    public class PausedMenu_UI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject Settings = default;
        [SerializeField] private GameObject Main = default;

        [Header("Broadcasting To")]
        [SerializeField] private StringEventChannelSO WarningUIChannel = default;
        [SerializeField] private LoadEventChannelSO onExitPressed = default;

        [Header("Scene To Unload")]
        [Tooltip("If exit pressed, unload this scene")]
        [SerializeField] private GameSceneSO gamePlayScene = default;

        [Header("Scenes To Load")]
        [SerializeField] private GameSceneSO mainMenu = default;

        private void OnEnable()
        {
            Main.SetActive(true);
        }

        private void OnDisable()
        {
            Settings.SetActive(false);
        }

        public void btn_Load()
        {
            SaveSystem.Load();
        }

        public void btn_Save()
        {
            WarningUIChannel.RaiseEvent("Kayıt Edildi : " + Application.persistentDataPath);
            SaveSystem.Save();
        }

        public void btn_Settings()
        {
            Settings.SetActive(true);
            Main.SetActive(false);
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

