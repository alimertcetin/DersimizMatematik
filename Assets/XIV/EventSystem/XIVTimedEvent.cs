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

        public void Update()
        {
            timer.Update(Time.deltaTime);
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
            onCanceled = null;
            onCompleted = null;
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