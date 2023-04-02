namespace LessonIsMath.StatSystems
{
    public interface IStat
    {
        public int Index { get; }
        public StatItemBase statItem { get; }
    }
}