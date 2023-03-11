using UnityEngine;

namespace LessonIsMath.InteractionSystems
{
    public abstract class InteractionHandlerBase : MonoBehaviour
    {
        public abstract void Init(IInteractor interactor);
        public abstract void TriggerEnter(Collider other);
        public abstract void TriggerExit(Collider other);
        public abstract void OnInteractionStart(IInteractable interactable);
        public abstract void OnInteractionEnd(IInteractable interactable);
    }
}