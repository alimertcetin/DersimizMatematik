using System;

namespace XIV.EventSystem
{
    public abstract class XIVEvent
    {
        public Action OnCompleted;
        public Action OnCanceled;

        public abstract void UpdateEvent();
        public abstract bool IsDone();
    }
}