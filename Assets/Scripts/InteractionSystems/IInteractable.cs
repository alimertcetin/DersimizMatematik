using UnityEngine;

namespace LessonIsMath.InteractionSystems
{
    public interface IInteractable
    {
        bool IsInInteraction { get; }
        InteractionSettings GetInteractionSettings();
        bool IsAvailableForInteraction();
        void Interact(IInteractor interactor);
        string GetInteractionString();
        InteractionPositionData GetInteractionPositionData(IInteractor interactor);
    }

    public struct InteractionPositionData
    {
        public Vector3 startPos; // Start position of interactor
        public Vector3 targetPosition; // target position of interactable in order to be able to interact with the object
        public Vector3 targetForwardDirection;
    }

    [System.Serializable]
    public struct InteractionSettings
    {
        public bool disableInteractionKey;
        public bool suspendMovement;
    }
}
