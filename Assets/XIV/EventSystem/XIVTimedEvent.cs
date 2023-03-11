using System;
using UnityEngine;
using XIV.Utils;

namespace XIV.EventSystem
{
    public class XIVTimedEvent : IEvent<XIVTimedEvent>
    {
        Timer timer;
        Action onCompleted;
        Action onCanceled;
        
        public XIVTimedEvent(float duration)
        {
            timer = new Timer(duration);
        }

        public void Update(float deltaTime)
        {
            timer.Update(deltaTime);
        }

        public bool IsDone()
        {
            return timer.IsDone;
        }

        public void Complete()
        {
            onCompleted?.Invoke();
            onCompleted = null;
            onCanceled = null;
        }

        public void Cancel()
        {
            onCanceled?.Invoke();
            onCompleted = null;
            onCanceled = null;
        }

        public XIVTimedEvent OnCompleted(Action action)
        {
            onCompleted += action;
            return this;
        }

        public XIVTimedEvent OnCanceled(Action action)
        {
            onCanceled += action;
            return this;
        }
    }
}