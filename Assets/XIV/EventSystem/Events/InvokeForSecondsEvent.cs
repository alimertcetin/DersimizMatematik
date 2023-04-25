using System;
using XIV.Core.Utils;

namespace XIV.EventSystem.Events
{
    public class InvokeForSecondsEvent : IEvent<InvokeForSecondsEvent>
    {
        float waitDuration;
        Timer timer;
        Action<Timer> action;
        Action onCompleted;
        Action onCanceled;
        Func<bool> cancelationCondition;
        bool hasCancelCondition;

        public InvokeForSecondsEvent(float duration)
        {
            timer = new Timer(duration);
        }

        public InvokeForSecondsEvent AddAction(Action<Timer> action)
        {
            this.action = action;
            return this;
        }
        
        public InvokeForSecondsEvent AddCancelCondition(Func<bool> condition)
        {
            this.cancelationCondition = condition;
            hasCancelCondition = true;
            return this;
        }
        
        public InvokeForSecondsEvent Wait(float seconds)
        {
            this.waitDuration = seconds;
            return this;
        }

        void IEvent.Update(float deltaTime)
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

        bool IEvent.IsDone()
        {
            return timer.IsDone;
        }

        void IEvent.Complete()
        {
            onCompleted?.Invoke();
            action = null;
            onCompleted = null;
            onCanceled = null;
            cancelationCondition = null;
        }

        void IEvent.Cancel()
        {
            onCanceled?.Invoke();
            action = null;
            onCompleted = null;
            onCanceled = null;
            cancelationCondition = null;
        }

        public InvokeForSecondsEvent OnCompleted(Action action)
        {
            onCompleted = action;
            return this;
        }

        public InvokeForSecondsEvent OnCanceled(Action action)
        {
            onCanceled = action;
            return this;
        }
    }
}