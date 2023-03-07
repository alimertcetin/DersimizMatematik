using UnityEngine;

namespace LessonIsMath.InteractionSystems
{
    public interface IInteractable
    {
        bool IsAvailableForInteraction();
        void Interact(IInteractor interactor);
        string GetInteractionString();
        InteractionTargetData GetInteractionTargetData(IInteractor interactor);
    }

    public struct InteractionTargetData
    {
        public Vector3 startPos; // Start position of interactor
        public Vector3 targetPosition; // target position of interactable in order to be able to interact with the object
        public Vector3 targetForwardDirection;
    }
}
