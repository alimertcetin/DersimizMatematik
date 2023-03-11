using System;
using XIV.Utils;

namespace XIV.EventSystem
{
    public class XIVInvokeForSecondsEvent : IEvent<XIVInvokeForSecondsEvent>
    {
        float waitDuration;
        Timer timer;
        Action<Timer> action;
        Action onCompleted;
        Action onCanceled;
        Func<bool> cancelationCondition;
        bool hasCancelCondition;

        public XIVInvokeForSecondsEvent(float duration)
        {
            timer = new Timer(duration);
        }

        public XIVInvokeForSecondsEvent AddAction(Action<Timer> action)
        {
            this.action = action;
            return this;
        }
        
        public XIVInvokeForSecondsEvent AddCancelCondition(Func<bool> condition)
        {
            this.cancelationCondition = condition;
            hasCancelCondition = true;
            return this;
        }
        
        public XIVInvokeForSecondsEvent Wait(float seconds)
        {
            this.waitDuration = seconds;
            return this;
        }

        public void Update(float deltaTime)
        {
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
            action = null;
            onCompleted = null;
            onCanceled = null;
            cancelationCondition = null;
        }

        public void Cancel()
        {
            onCanceled?.Invoke();
            action = null;
            onCompleted = null;
            onCanceled = null;
            cancelationCondition = null;
        }

        public XIVInvokeForSecondsEvent OnCompleted(Action action)
        {
            onCompleted = action;
            return this;
        }

        public XIVInvokeForSecondsEvent OnCanceled(Action action)
        {
            onCanceled = action;
            return this;
        }
    }
}