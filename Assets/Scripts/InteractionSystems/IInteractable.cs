namespace LessonIsMath.InteractionSystems
{
    public interface IInteractable
    {
        bool CanInteract();
        void Interact(IInteractor interactor);
        string GetInteractionString();
    }
}
