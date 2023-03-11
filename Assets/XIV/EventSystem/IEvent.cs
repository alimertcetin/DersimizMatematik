using System;

namespace XIV.EventSystem
{
    public interface IEvent
    {
        void Update(float deltaTime);
        bool IsDone();
        void Complete();
        void Cancel();
    }
    
    public interface IEvent<out T> : IEvent
    {
        T OnCompleted(Action action);
        T OnCanceled(Action action);
    }

    public interface IEvent<out T, out P0> : IEvent
    {
        T OnCompleted(Action<P0> action);
        T OnCanceled(Action<P0> action);
    }

    public interface IEvent<out T, out P0, out P1> : IEvent
    {
        T OnCompleted(Action<P0, P1> action);
        T OnCanceled(Action<P0, P1> action);
    }

    public interface IEvent<out T, out P0, out P1, out P2> : IEvent
    {
        T OnCompleted(Action<P0, P1, P2> action);
        T OnCanceled(Action<P0, P1, P2> action);
    }

    public interface IEvent<out T, out P0, out P1, out P2, out P3> : IEvent
    {
        T OnCompleted(Action<P0, P1, P2, P3> action);
        T OnCanceled(Action<P0, P1, P2, P3> action);
    }

    public interface IEvent<out T, out P0, out P1, out P2, out P3, out P4> : IEvent
    {
        T OnCompleted(Action<P0, P1, P2, P3, P4> action);
        T OnCanceled(Action<P0, P1, P2, P3, P4> action);
    }
}