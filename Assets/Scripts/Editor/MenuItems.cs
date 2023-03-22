using LessonIsMath.UI.Components;
using LessonIsMath.XIVEditor.Windows;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace LessonIsMath.XIVEditor
{
    public static class MenuItems
    {
        public const string BASE_MENU = "Lesson Is Math";
        public const string WINDOW_MENU = BASE_MENU + "/Window";

        public const string DOOR_CREATOR_WINDOW_MENU = WINDOW_MENU + "/Door Creator";
        public const string EASY_SCENE_WINDOW_MENU = WINDOW_MENU + "/Easy Scene Loader";
        public const string KEYCARD_CREATOR_WINDOW_MENU = WINDOW_MENU + "/Keycard Creator";
        
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
        
        [MenuItem(EASY_SCENE_WINDOW_MENU)]
        public static void ShowSceneLoaderWindow()
        {
            if (EditorWindow.HasOpenInstances<EasySceneLoaderWindow>())
            {
                EditorWindow.GetWindow<EasySceneLoaderWindow>("Easy Scene Loader").Focus();
                return;
            }

            EditorWindow.CreateWindow<EasySceneLoaderWindow>("Easy Scene Loader").Show();
        }
        
        [MenuItem(KEYCARD_CREATOR_WINDOW_MENU)]
        public static void ShowKeycardCreatorWindow()
        {
            if (EditorWindow.HasOpenInstances<KeycardCreatorWindow>())
            {
                EditorWindow.GetWindow<KeycardCreatorWindow>("Keycard Creator").Focus();
                return;
            }

            EditorWindow.CreateWindow<KeycardCreatorWindow>("Keycard Creator").Show();
        }

        // hierarchy
        [MenuItem("GameObject/UI/Custom Button", false, 0)]
        static void AddComponent(MenuCommand menuCommand)
        {
            var parent = (GameObject)Selection.activeObject;
            EditorApplication.ExecuteMenuItem("GameObject/UI/Button - TextMeshPro");
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                var activeGo = (GameObject)Selection.objects[i];
                Object.DestroyImmediate(activeGo.GetComponent<Button>());
                activeGo.AddComponent<CustomButton>();
                activeGo.transform.SetParent(parent.transform);
            }
        }
        
    }
}