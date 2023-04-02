using LessonIsMath.StatSystems.Stats;
using UnityEngine;

namespace LessonIsMath.StatSystems.Drivers
{
    public class BrainStatDriver : StatDriver
    {
        public bool isDecreasing;

        public BrainStatDriver(StatContainer statContainer) : base(statContainer)
        {
            
        }

        public override void Update()
        {
            if (isDecreasing) Decrease();
            else Increase();
        }

        void Decrease()
        {
            var brainPowerStat = statContainer.GetStatData<BrainPowerStatItem>();
            if (Mathf.Abs(brainPowerStat.current - brainPowerStat.min) > Mathf.Epsilon)
            {
                statContainer.UpdateStat<BrainPowerStatItem>(DecreaseStat(brainPowerStat));
                return;
            }
            
            var brainCoreStat = statContainer.GetStatData<BrainCoreStatItem>();
            if (Mathf.Abs(brainCoreStat.current - brainCoreStat.min) > Mathf.Epsilon)
            {
                brainCoreStat.current = brainCoreStat.min;
                statContainer.UpdateStat<BrainCoreStatItem>(brainCoreStat);
            }
        }

        void Increase()
        {
            var brainCoreStat = statContainer.GetStatData<BrainCoreStatItem>();
            if (Mathf.Abs(brainCoreStat.current - brainCoreStat.max) > Mathf.Epsilon)
            {
                statContainer.UpdateStat<BrainCoreStatItem>(IncreaseStat(brainCoreStat));
                return;
            }
            
            var brainPowerStat = statContainer.GetStatData<BrainPowerStatItem>();
            if (Mathf.Abs(brainPowerStat.current - brainPowerStat.max) > Mathf.Epsilon)
            {
                statContainer.UpdateStat<BrainPowerStatItem>(IncreaseStat(brainPowerStat));
            }
            
        }

        static StatData DecreaseStat(StatData stat)
        {
            stat.current = Mathf.MoveTowards(stat.current, stat.min, stat.decreaseSpeed * Time.deltaTime);
            return stat;
        }

        static StatData IncreaseStat(StatData stat)
        {
            stat.current = Mathf.MoveTowards(stat.current, stat.max, stat.increaseSpeed * Time.deltaTime);
            return stat;
        }
    }
}