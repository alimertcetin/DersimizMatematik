using System;
using System.Collections.Generic;
using LessonIsMath.UI.Components;
using LessonIsMath.XIVEditor.Windows;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using XIV.Extensions;
using XIVEditor.Utils;
using Object = UnityEngine.Object;

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

        // hierarchy
        [MenuItem("GameObject/Select Lights/All", false, 0)]
        static void SelectLights(MenuCommand menuCommand)
        {
            SelectLights(LightmapBakeType.Realtime | LightmapBakeType.Baked | LightmapBakeType.Mixed);
        }
        
        [MenuItem("GameObject/Select Lights/Realtime", false, 0)]
        static void SelectLightsRealtime(MenuCommand menuCommand)
        {
            SelectLights(LightmapBakeType.Realtime);
        }
        
        [MenuItem("GameObject/Select Lights/Baked", false, 0)]
        static void SelectLightsBaked(MenuCommand menuCommand)
        {
            SelectLights(LightmapBakeType.Baked);
        }
        
        [MenuItem("GameObject/Select Lights/Mixed", false, 0)]
        static void SelectLightsMixed(MenuCommand menuCommand)
        {
            SelectLights(LightmapBakeType.Mixed);
        }

        static void SelectLights(LightmapBakeType lightmapBakeType)
        {
            var parent = Selection.activeGameObject;
            if (parent == null) return;
            Light[] lights = GetComponentInChildrenRecursive<Light>(parent)
                .RemoveIf((light) => (light.lightmapBakeType & lightmapBakeType) == 0);
            
            var childObjects = ToGameObjectArray(lights);
            for (var i = 0; i < childObjects.Length; i++)
            {
                // Still doesnt expand child objects of GameObject/Prefab as expected but works better than just setting Selection.objects
                Selection.activeGameObject = childObjects[i];
            }

            Selection.objects = childObjects;
        }

        // hierarchy
        [MenuItem("GameObject/Add Default Area Light", false, 0)]
        static void AddDefaultAreaLight(MenuCommand menuCommand)
        {
            var prefab = AssetUtils.FindPrefab("DefaultAreaLight");
            CreatePrefabUsingSelection(prefab);
            Selection.objects = null;
        }
        
        // hierarchy
        [MenuItem("GameObject/Add Default Spot Light", false, 0)]
        static void AddDefaultSpotLight(MenuCommand menuCommand)
        {
            var prefab = AssetUtils.FindPrefab("DefaultSpotLight");
            CreatePrefabUsingSelection(prefab);
            Selection.objects = null;
        }

        static GameObject[] CreatePrefabUsingSelection(GameObject prefab)
        {
            var activeObjects = Selection.objects;
            GameObject[] createdPrefabs = new GameObject[activeObjects.Length];
            for (int i = 0; i < activeObjects.Length; i++)
            {
                var parent = activeObjects[i];
                var parentTransform = parent == null ? null : parent as GameObject == null ? null : ((GameObject)parent).transform;
                var newPrefab = PrefabUtility.InstantiatePrefab(prefab, parentTransform);
                createdPrefabs[i] = (GameObject)newPrefab;
                Undo.RegisterCreatedObjectUndo(newPrefab, "Created " + prefab.name);
            }

            return createdPrefabs;
        }

        static T[] GetComponentInChildrenRecursive<T>(GameObject go) where T : Component
        {
            int childCount = go.transform.childCount;
            List<T> list = new List<T>(childCount);
            for (int i = 0; i < childCount; i++)
            {
                if (go.transform.GetChild(i).TryGetComponent<T>(out var comp))
                {
                    list.Add(comp);
                }
            }
            for (int i = 0; i < childCount; i++)
            {
                var childlist = GetComponentInChildrenRecursive<T>(go.transform.GetChild(i).gameObject);
                list.AddRange(childlist);
            }

            return list.ToArray();
        }

        static GameObject[] ToGameObjectArray<T>(IList<T> list) where T : Component
        {
            int count = list.Count;
            var goArr = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                goArr[i] = list[i].gameObject;
            }

            return goArr;
        }

    }
}