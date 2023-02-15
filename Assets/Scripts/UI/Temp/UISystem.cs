using System.Collections.Generic;

namespace LessonIsMath.UI
{
    public static class UISystem
    {
        static List<GameUI> gameUIList = new List<GameUI>();

        public static void AddUI(GameUI gameUI)
        {
            if (gameUIList.Contains(gameUI)) return;
            gameUIList.Add(gameUI);
            UIEventSystem.AddUI(gameUI);
        }

        public static void RemoveUI(GameUI gameUI)
        {
            var index = gameUIList.IndexOf(gameUI);
            if (index < 0) return;
            gameUIList.RemoveAt(index);
            UIEventSystem.RemoveUI(gameUI);
        }

        public static void Show<T>() where T : GameUI
        {
            var ui = GetUI<T>();
            if (ui == null) return;
            ui.Show();
            UIEventSystem.OnShowUI(ui);
        }

        public static void Hide<T>() where T : GameUI
        {
            var ui = GetUI<T>();
            if (ui == null) return;
            ui.Hide();
            UIEventSystem.OnHideUI(ui);
        }

        public static T GetUI<T>() where T : GameUI
        {
            for (int i = 0; i < gameUIList.Count; i++)
            {
                if (gameUIList[i] is T ui) return ui;
            }
            return default;
        }
    }

}