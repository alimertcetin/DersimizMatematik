namespace LessonIsMath.UI
{
    public interface IKeypadListener
    {
        void OnEnter();
        void OnDeleteStarted();
        void OnDeleteCanceled();
        void OnNumberPressed(int value);
    }
}