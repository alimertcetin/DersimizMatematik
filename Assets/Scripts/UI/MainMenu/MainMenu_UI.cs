using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.ScriptableObjects.SceneSOs;
using UnityEngine;
using UnityEngine.UI;

namespace LessonIsMath.UI
{
    public class MainMenu_UI : GameUI
    {
        [SerializeField] GameSceneSO locationToLoad;
        [SerializeField] bool showLoadingScreen;
        [SerializeField] LoadEventChannelSO loadLocationChannel;
        [SerializeField] Button btn_StartNewGame;
        [SerializeField] Button btn_Exit;

        void OnEnable()
        {
            btn_StartNewGame.onClick.AddListener(StartNewGame);
            btn_Exit.onClick.AddListener(ExitGame);
        }

        void OnDisable()
        {
            btn_StartNewGame.onClick.RemoveListener(StartNewGame);
            btn_Exit.onClick.RemoveListener(ExitGame);
        }

        void StartNewGame()
        {
            loadLocationChannel.RaiseEvent(locationToLoad, showLoadingScreen);
            InputManager.GameState.Enable();
            InputManager.CharacterMovement.Enable();
        }

        void ExitGame()
        {
            Application.Quit();
        }
    }
}