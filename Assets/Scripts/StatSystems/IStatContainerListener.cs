namespace LessonIsMath.StatSystems
{
    public interface IStatContainerListener
    {
        void OnStatContainerChanged(StatContainerChange statContainerChange);
    }
}