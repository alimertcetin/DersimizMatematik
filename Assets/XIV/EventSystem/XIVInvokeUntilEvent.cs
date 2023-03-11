using System;

namespace XIV.EventSystem
{
    public class XIVInvokeUntilEvent : IEvent<XIVInvokeUntilEvent>
    {
        Action action;
        Func<bool> condition;
        Action onCompleted;
        Action onCanceled;

        bool hasAction;

        public XIVInvokeUntilEvent AddCondition(Func<bool> condition)
        {
            this.condition = condition;
            return this;
        }

        public XIVInvokeUntilEvent AddAction(Action action)
        {
            hasAction = true;
            this.action = action;
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