using LessonIsMath.Input;
using LessonIsMath.InteractionSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI;
using UnityEngine;

namespace LessonIsMath.World.Interactables.BlackboardSystems
{
    public class Blackboard : MonoBehaviour, IInteractable, IUIEventListener
    {
        [SerializeField] BoolEventChannelSO blackBoardUIChannel;

        IInteractor interactor;
        public bool IsInInteraction { get; private set; }

        void OnEnable() => UIEventSystem.Register<BlackboardUI>(this);

        void OnDisable() => UIEventSystem.Unregister<BlackboardUI>(this);

        void OnBlackBoardInteract(bool value)
        {
#if UNITY_EDITOR
            if (value)
            {
                Debug.LogError("Blackboard registered UI event after UI opened. But something requested UI to open even it already is");
                return;
            }
#endif
            blackBoardUIChannel.OnEventRaised -= OnBlackBoardInteract;
            interactor.OnInteractionEnd(this);
        }

        InteractionSettings IInteractable.GetInteractionSettings()
        {
            return new InteractionSettings
            {
                disableInteractionKey = true,
                suspendMovement = true,
            };
        }

        bool IInteractable.IsAvailableForInteraction() => !IsInInteraction;

        void IInteractable.Interact(IInteractor interactor)
        {
            this.interactor = interactor;
            blackBoardUIChannel.RaiseEvent(true);
            blackBoardUIChannel.OnEventRaised += OnBlackBoardInteract;
        }

        string IInteractable.GetInteractionString() => "Press " + InputManager.InteractionKeyName + " to interact with Blackboard";
        InteractionPositionData IInteractable.GetInteractionPositionData(IInteractor interactor)
        {
            var interactorPos = (interactor as Component).transform.position;
            return new InteractionPositionData
            {
                startPos = interactorPos,
                targetPosition = interactorPos,
                targetForwardDirection = transform.forward, 
            };
        }

        void IUIEventListener.OnShowUI(GameUI ui) => IsInInteraction = true;
        void IUIEventListener.OnHideUI(GameUI ui) => IsInInteraction = false;
    }
}