using LessonIsMath.Input;
using LessonIsMath.InteractionSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI;
using UnityEngine;

namespace LessonIsMath.World.Interactables.BlackboardSystems
{
    public class Blackboard : MonoBehaviour, IInteractable, IUIEventListener
    {
        [SerializeField] private BoolEventChannelSO blackBoardUIChannel = default;

        IInteractor interactor;
        bool isUIActive;

        void OnEnable() => UIEventSystem.Register<BlackboardUI>(this);

        void OnDisable() => UIEventSystem.Unregister<BlackboardUI>(this);

        bool IInteractable.IsAvailable() => !isUIActive;

        void IInteractable.Interact(IInteractor interactor)
        {
            this.interactor = interactor;

            blackBoardUIChannel.RaiseEvent(true);
            blackBoardUIChannel.OnEventRaised += OnBlackBoardInteract;
        }

        private void OnBlackBoardInteract(bool value)
        {
            blackBoardUIChannel.OnEventRaised -= OnBlackBoardInteract;
            interactor.OnInteractionEnd(this);
        }

        string IInteractable.GetInteractionString() => "Press " + InputManager.InteractionKeyName + " to interact with Blackboard";
        InteractionTargetData IInteractable.GetInteractionTargetData(IInteractor interactor)
        {
            var interactorPos = (interactor as Component).transform.position;
            return new InteractionTargetData
            {
                startPos = interactorPos,
                targetPosition = interactorPos,
                targetForwardDirection = transform.forward, 
            };
        }

        void IUIEventListener.OnShowUI(GameUI ui) => isUIActive = true;
        void IUIEventListener.OnHideUI(GameUI ui) => isUIActive = false;
    }
}