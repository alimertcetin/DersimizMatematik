using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LessonIsMath
{
    public class GameManager : MonoBehaviour, PlayerControls.IGameStateActions
    {
        [SerializeField] BoolEventChannelSO pauseMenuUIChannel;

        void Awake()
        {
            InputManager.GameState.SetCallbacks(this);
        }

        void OnEnable()
        {
            InputManager.GameState.Enable();
        }

        void OnDisable()
        {
            InputManager.GameState.Disable();
        }

        void PlayerControls.IGameStateActions.OnEscape(InputAction.CallbackContext context)
        {
            if (context.performed == false) return;
        
            pauseMenuUIChannel.RaiseEvent(true);
        }

    }

}