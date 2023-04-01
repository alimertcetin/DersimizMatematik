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
    }
}