using UnityEngine;

namespace LessonIsMath.InteractionSystems
{
    public interface IInteractable
    {
        bool IsAvailable();
        void Interact(IInteractor interactor);
        string GetInteractionString();
        Vector3 GetInteractionStayPosition(IInteractor interactor);
        Vector3 GetReachPosition(IInteractor interactor);
    }

    public struct InteractionPositionData
    {
        public Vector3 stayPosition;
        public Vector3 forward;
        
    }
}
