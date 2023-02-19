namespace LessonIsMath.InteractionSystems
{
    public interface IInteractable
    {
        bool IsAvailable();
        void Interact(IInteractor interactor);
        string GetInteractionString();
    }
}
