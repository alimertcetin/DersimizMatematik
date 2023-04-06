using XIV.XIVMath;

namespace LessonIsMath.StatSystems
{
    [System.Serializable]
    public struct StatData
    {
        public float decreaseSpeed;
        public float increaseSpeed;
        public float min;
        public float max;
        public float current;

        public float normalizedCurrent => XIVMathf.Normalize(current, min, max);

        public StatData(StatData other)
        {
            this.decreaseSpeed = other.decreaseSpeed;
            this.increaseSpeed = other.increaseSpeed;
            this.min = other.min;
            this.max = other.max;
            this.current = other.current;
        }

        public void Update(StatData other)
        {
            this.decreaseSpeed = other.decreaseSpeed;
            this.increaseSpeed = other.increaseSpeed;
            this.min = other.min;
            this.max = other.max;
            this.current = XIVMathf.Clamp(other.current, min, max);
        }
    }
}