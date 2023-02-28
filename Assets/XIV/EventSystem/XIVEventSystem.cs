using System.Collections.Generic;
using UnityEngine;

namespace XIV.EventSystem
{
    public static class XIVEventSystem
    {
        static EventHelperMono helper;

        static EventHelperMono Helper
        {
            get
            {
                if (helper == null) helper = new GameObject().AddComponent<EventHelperMono>();
                return helper;
            }
        }

        class EventHelperMono : MonoBehaviour
        {
            public List<XIVEvent> events = new List<XIVEvent>();

            void Update()
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    var @event = events[i];
                    @event.UpdateEvent();
                    if (@event.IsDone())
                    {
                        events.RemoveAt(i);
                        @event.OnCompleted?.Invoke();
                    }
                }
            }

            void OnDestroy()
            {
                helper = null;
            }
        }

        public static void SendEvent(XIVEvent @event)
        {
            Helper.events.Add(@event);
        }
    }
}