using System;
using XIV.Core.Utils;

namespace XIV.EventSystem.Events
{
    public class InvokeAfterEvent : IEvent<InvokeAfterEvent>
    {
        Timer timer;
        Action onCompleted;
        Action onCanceled;
        
        public InvokeAfterEvent(float duration)
        {
            timer = new Timer(duration);
        }

        void IEvent.Update(float deltaTime)
        {
            timer.Update(deltaTime);
        }

        bool IEvent.IsDone()
        {
            return timer.IsDone;
        }

        void IEvent.Complete()
        {
            onCompleted?.Invoke();
            onCompleted = null;
            onCanceled = null;
        }

        void IEvent.Cancel()
        {
            onCanceled?.Invoke();
            onCompleted = null;
            onCanceled = null;
        }

        public InvokeAfterEvent OnCompleted(Action action)
        {
            onCompleted += action;
            return this;
        }

        public InvokeAfterEvent OnCanceled(Action action)
        {
            onCanceled += action;
            return this;
        }
    }
}