using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using XIVEditor.Utils;

namespace LessonIsMath.XIVEditor.Windows
{
    public class EasySceneLoaderWindow : EditorWindow
    {
        bool isInitialized;
        List<SceneAsset> scenes;
        List<SceneAsset> testScenes;
        Vector2 scrollPos;

        void OnProjectChange()
        {
            isInitialized = false;
        }

        void OnGUI()
        {
            if (isInitialized == false)
            {
                isInitialized = true;
                scenes = new List<SceneAsset>(8);
                testScenes = new List<SceneAsset>(8);
                AddScenes("Assets/Scenes", scenes);
                AddScenes("Assets/Tests", testScenes);
            }
            
            GUILayout.Label("Game Scenes", EditorStyles.boldLabel);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            foreach (SceneAsset sceneAsset in scenes)
            {
                GUILayout.Space(10);
                if (GUILayout.Button(sceneAsset.name, GUILayout.Height(50)) == false) continue;

                LoadScene(sceneAsset);
            }
            
            GUILayout.Label("Test Scenes", EditorStyles.boldLabel);

            foreach (SceneAsset sceneAsset in testScenes)
            {
                GUILayout.Space(10);
                if (GUILayout.Button(sceneAsset.name, GUILayout.Height(50)) == false) continue;

                LoadScene(sceneAsset);
            }
            EditorGUILayout.EndScrollView();
        }

        static void LoadScene(SceneAsset sceneAsset)
        {
            int countLoaded = SceneManager.sceneCount;
            Scene[] loadedScenes = new Scene[countLoaded];
            for (int i = 0; i < countLoaded; i++)
            {
                loadedScenes[i] = SceneManager.GetSceneAt(i);
            }

            bool isSaved = EditorSceneManager.SaveModifiedScenesIfUserWantsTo(loadedScenes);
            if (isSaved)
            {
                if (Application.isPlaying) SceneManager.LoadScene(sceneAsset.name);
                else EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneAsset));
            }
        }

        void AddScenes(string path, List<SceneAsset> collection)
        {
            var valueCollection = AssetUtils.LoadAssetsOfType<SceneAsset>(path).Values;
            foreach (List<SceneAsset> sceneAssets in valueCollection)
            {
                collection.AddRange(sceneAssets);
            }
        }
    }
}