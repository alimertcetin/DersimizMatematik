
namespace LessonIsMath.StatSystems
{
    [System.Serializable]
    public struct StatLevelData
    {
        public float requiredExperience;
        public StatData statData;
    }
    
    [System.Serializable]
    public abstract class StatItemBase
    {
        public float currentExperience { get; private set; }
        public int currentLevel;
        public ref StatData statData => ref levels[currentLevel].statData;
        public StatLevelData[] levels;

        public bool UpdateExperience(float amount)
        {
            if (currentLevel + 1 >= levels.Length) return false;
            currentExperience += amount;
            if (levels[currentLevel + 1].requiredExperience > currentExperience) return false;
            
            currentLevel++;
            return true;
        }
        
        public abstract bool Equals(StatItemBase other);
    }
}