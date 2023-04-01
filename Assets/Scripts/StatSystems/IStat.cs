namespace LessonIsMath.StatSystems
{
    public interface IStat
    {
        StatData GetStatData();
        void SetStatData(StatData newStatData);
        void Register(IStatListener statListener);
        void Unregister(IStatListener statListener);
    }
}