namespace LessonIsMath.StatSystems.Drivers
{
    public abstract class StatDriver
    {
        protected StatContainer statContainer;
        
        public StatDriver(StatContainer statContainer)
        {
            this.statContainer = statContainer;
        }
        
        public abstract void Update();
    }
}