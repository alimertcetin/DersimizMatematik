using System;
using UnityEngine;
using XIV.Utils;

namespace XIV.EventSystem
{
    public class XIVInvokeUntilEvent : XIVEvent
    {
        Timer timer;
        Action<Timer> action;

        public XIVInvokeUntilEvent(float duration, Action<Timer> action)
        {
            timer = new Timer(duration);
            this.action = action;
        }

        public new XIVInvokeUntilEvent OnCompleted(Action action)
        {
            base.OnCompleted = action;
            return this;
        }

        public override void UpdateEvent()
        {
            timer.Update(Time.deltaTime);
            action.Invoke(timer);
        }

        public override bool IsDone()
        {
            return timer.IsDone;
        }
    }
}