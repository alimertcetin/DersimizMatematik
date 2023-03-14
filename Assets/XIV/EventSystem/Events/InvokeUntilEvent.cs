using System;

namespace XIV.EventSystem.Events
{
    public class InvokeUntilEvent : IEvent<InvokeUntilEvent>
    {
        Action action;
        Func<bool> condition;
        Action onCompleted;
        Action onCanceled;

        bool hasAction;

        public InvokeUntilEvent AddAction(Action action)
        {
            hasAction = true;
            this.action = action;
            return this;
        }

        public InvokeUntilEvent AddCancelCondition(Func<bool> condition)
        {
            this.condition = condition;
            return this;
        }
        
        void IEvent.Update(float deltaTime)
        {
            if (hasAction) action.Invoke();
        }

        bool IEvent.IsDone()
        {
            return condition.Invoke();
        }

        void IEvent.Complete()
        {
            onCompleted?.Invoke();
            condition = null;
            onCanceled = null;
            onCompleted = null;
        }

        void IEvent.Cancel()
        {
            onCanceled?.Invoke();
            condition = null;
            onCanceled = null;
            onCompleted = null;
        }

        public InvokeUntilEvent OnCompleted(Action action)
        {
            onCompleted = action;
            return this;
        }

        public InvokeUntilEvent OnCanceled(Action action)
        {
            onCanceled = action;
            return this;
        }
    }
}