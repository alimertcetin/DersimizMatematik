namespace LessonIsMath.StatSystems
{
    public interface IStatListener
    {
        void OnStatChanged(IStat stat);
    }
}