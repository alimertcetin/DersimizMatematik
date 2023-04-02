
namespace LessonIsMath.StatSystems
{
    [System.Serializable]
    public abstract class StatItemBase
    {
        public string title;
        public string description;
        // public Sprite UISprite;
        public StatData statData;

        public abstract bool Equals(StatItemBase other);
    }
}