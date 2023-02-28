using UnityEngine;
using XIV.Utils;

namespace XIV.EventSystem
{
    public class XIVTimedEvent : XIVEvent
    {
        Timer timer;

        public XIVTimedEvent(float duration)
        {
            timer = new Timer(duration);
        }

        public override void UpdateEvent()
        {
            timer.Update(Time.deltaTime);
        }

        public override bool IsDone()
        {
            return timer.IsDone;
        }
    }
}