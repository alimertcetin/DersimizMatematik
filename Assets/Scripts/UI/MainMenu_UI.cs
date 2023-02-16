using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using UnityEngine.UI;

namespace LessonIsMath.UI
{
    public class MainMenu_UI : GameUI
    {
        [SerializeField] GameSceneSO _locationsToLoad = default;
        [SerializeField] bool _showLoadScreen = default;
        [SerializeField] LoadEventChannelSO _startGameEvent = default;
        [SerializeField] Button btn_StartNewGame;
        [SerializeField] Button btn_Settings;
        [SerializeField] Button btn_Exit;

        void OnEnable()
        {
            btn_StartNewGame.onClick.AddListener(StartNewGame);
            btn_Settings.onClick.AddListener(ShowSettings);
            btn_Exit.onClick.AddListener(ExitGame);
        }

        void OnDisable()
        {
            btn_StartNewGame.onClick.RemoveListener(StartNewGame);
            btn_Settings.onClick.RemoveListener(ShowSettings);
            btn_Exit.onClick.RemoveListener(ExitGame);
        }

        void StartNewGame()
        {
            _startGameEvent.RaiseEvent(_locationsToLoad, _showLoadScreen);
            InputManager.GameManager.Enable();
            InputManager.GamePlay.Enable();
        }

        void ShowSettings()
        {
            // TODO : Settings Page
        }

        void ExitGame()
        {
            // TODO : Save ?
            Application.Quit();
        }
    }
}