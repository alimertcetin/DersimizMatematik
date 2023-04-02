namespace LessonIsMath.StatSystems.Stats
{
    [System.Serializable]
    public class BrainCoreStatItem : StatItemBase
    {
        public override bool Equals(StatItemBase other)
        {
            if (other is not BrainCoreStatItem otherStat) return false;

            return System.Object.Equals(otherStat, this);
        }
    }
}