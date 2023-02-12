using LessonIsMath.Input;
using LessonIsMath.PlayerSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;

namespace LessonIsMath.World.Interactables.BlackboardSystems
{
    public class Blackboard : MonoBehaviour
    {
        [SerializeField] private StringEventChannelSO notificationChannel = default;
        [SerializeField] private BoolEventChannelSO blackBoardUIChannel = default;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out _) == false) return;

            InputManager.PlayerControls.Gameplay.Interact.performed += Interact_performed;
            blackBoardUIChannel.OnEventRaised += OnBlackBoardInteract;
            notificationChannel.RaiseEvent("Press " + InputManager.InteractionKeyName + " to interact with BlackBoard");
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out _) == false) return;

            InputManager.PlayerControls.Gameplay.Interact.performed -= Interact_performed;
            blackBoardUIChannel.OnEventRaised -= OnBlackBoardInteract;
            notificationChannel.RaiseEvent("", false);
        }

        private void OnBlackBoardInteract(bool value)
        {
            if (value)
            {
                notificationChannel.RaiseEvent("", false);
                return;
            }

            notificationChannel.RaiseEvent("Press " + InputManager.InteractionKeyName + " to interact with BlackBoard");
        }

        private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            blackBoardUIChannel.RaiseEvent(true);
        }
    }
}