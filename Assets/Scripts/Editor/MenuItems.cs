using LessonIsMath.XIVEditor.Windows;
using UnityEditor;

namespace LessonIsMath.XIVEditor
{
    public static class MenuItems
    {
        public const string BASE_MENU = "Lesson Is Math";
        public const string WINDOW_MENU = BASE_MENU + "/Window";

        public const string DOOR_CREATOR_WINDOW_MENU = WINDOW_MENU + "/Door Creator";
        
        [MenuItem(DOOR_CREATOR_WINDOW_MENU)]
        public static void ShowDoorCreatorWindow()
        {
            if (EditorWindow.HasOpenInstances<DoorCreatorEditor>())
            {
                EditorWindow.GetWindow<DoorCreatorEditor>().Focus();
                return;
            }

            EditorWindow.CreateWindow<DoorCreatorEditor>().Show();
        }
    }
}