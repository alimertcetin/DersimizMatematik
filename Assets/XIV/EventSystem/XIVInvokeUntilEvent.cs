using System;
using UnityEngine;
using XIV.Utils;

namespace XIV.EventSystem
{
    public class XIVInvokeUntilEvent : IEvent<XIVInvokeUntilEvent>
    {
        float waitDuration;
        Timer timer;
        Action<Timer> action;
        Action onCompleted;
        Action onCanceled;
        Func<bool> cancelationCondition;
        bool hasCancelCondition;

        public XIVInvokeUntilEvent(float duration)
        {
            timer = new Timer(duration);
        }

        public XIVInvokeUntilEvent AddAction(Action<Timer> action)
        {
            this.action = action;
            return this;
        }
        
        public XIVInvokeUntilEvent AddCancelCondition(Func<bool> condition)
        {
            this.cancelationCondition = condition;
            hasCancelCondition = true;
            return this;
        }
        
        public XIVInvokeUntilEvent Wait(float seconds)
        {
            this.waitDuration = seconds;
            return this;
        }

        public void Update()
        {
            var deltaTime = Time.deltaTime;
            waitDuration -= deltaTime;
            if (waitDuration > 0) return;
            
            if (hasCancelCondition && cancelationCondition.Invoke())
            {
                XIVEventSystem.CancelEvent(this);
                return;
            }

            timer.Update(deltaTime);
            action.Invoke(timer);
        }

        public bool IsDone()
        {
            return timer.IsDone;
        }

        public void Complete()
        {
            onCompleted?.Invoke();
        }

        public void Cancel()
        {
            onCanceled?.Invoke();
            action = null;
            onCompleted = null;
            onCanceled = null;
            cancelationCondition = null;
        }

        public XIVInvokeUntilEvent OnCompleted(Action action)
        {
            onCompleted = action;
            return this;
        }

        public XIVInvokeUntilEvent OnCanceled(Action action)
        {
            onCanceled = action;
            return this;
        }
    }
}