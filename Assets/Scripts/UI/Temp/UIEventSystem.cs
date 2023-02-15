using System;
using System.Collections.Generic;

namespace LessonIsMath.UI
{
    public static class UIEventSystem
    {
        static Dictionary<Type, List<IUIEventListener>> listeners = new Dictionary<Type, List<IUIEventListener>>();

        public static void AddUI(GameUI gameUI)
        {
            var type = gameUI.GetType();
            if (listeners.ContainsKey(type)) return;
            listeners.Add(type, new List<IUIEventListener>());
        }

        public static void RemoveUI(GameUI gameUI)
        {
            var type = gameUI.GetType();
            if (listeners.ContainsKey(type) == false) return;
            listeners.Remove(type);
        }

        public static void Register<T>(IUIEventListener uiEvenetListener) where T : GameUI
        {
            if (listeners.TryGetValue(typeof(T), out var list) == false || list.Contains(uiEvenetListener)) return;
            list.Add(uiEvenetListener);
        }

        public static void Unregister<T>(IUIEventListener uiEvenetListener) where T : GameUI
        {
            if (listeners.TryGetValue(typeof(T), out var list) == false) return;

            int index = list.IndexOf(uiEvenetListener);
            if (index < 0) return;
            list.RemoveAt(index);
        }

        public static void OnShowUI(GameUI gameUI)
        {
            if (listeners.TryGetValue(gameUI.GetType(), out var uiListeners) == false) return;

            for (int i = 0; i < uiListeners.Count; i++)
            {
                uiListeners[i].OnShowUI(gameUI);
            }
        }

        public static void OnHideUI(GameUI gameUI)
        {
            if (listeners.TryGetValue(gameUI.GetType(), out var uiListeners) == false) return;

            for (int i = 0; i < uiListeners.Count; i++)
            {
                uiListeners[i].OnHideUI(gameUI);
            }
        }
    }

}