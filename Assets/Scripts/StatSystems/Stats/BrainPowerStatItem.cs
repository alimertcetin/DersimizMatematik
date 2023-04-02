namespace LessonIsMath.StatSystems.Stats
{
    [System.Serializable]
    public class BrainPowerStatItem : StatItemBase
    {
        public override bool Equals(StatItemBase other)
        {
            if (other is not BrainPowerStatItem otherStat) return false;

            return System.Object.Equals(otherStat, this);
        }
    }
}