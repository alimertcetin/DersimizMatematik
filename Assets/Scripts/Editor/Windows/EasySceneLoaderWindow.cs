using System;
using System.Collections.Generic;
using System.IO;
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
        bool additiveLoadToggle;

        string sceneFolder;
        string testFolder;

        string sceneFolderKey => nameof(EasySceneLoaderWindow) + "_SceneFolderPath";
        string testFolderKey => nameof(EasySceneLoaderWindow) + "_TestFolderPath";

        void OnProjectChange()
        {
            scenes.Clear();
            testScenes.Clear();
            AddScenes(sceneFolder, scenes);
            AddScenes(testFolder, testScenes);
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            EditorPrefs.SetString(sceneFolderKey, sceneFolder);
            EditorPrefs.SetString(testFolderKey, testFolder);
        }

        void OnEnable()
        {
            sceneFolder = EditorPrefs.GetString(sceneFolderKey, "Assets/Scenes");
            testFolder = EditorPrefs.GetString(testFolderKey, "Assets/Tests");
        }

        void OnDestroy()
        {
            SaveChanges();
        }

        void OnGUI()
        {
            if (isInitialized == false)
            {
                isInitialized = true;
                scenes = new List<SceneAsset>(8);
                testScenes = new List<SceneAsset>(8);
                AddScenes(sceneFolder, scenes);
                AddScenes(testFolder, testScenes);
            }

            EditorGUILayout.BeginHorizontal();
            additiveLoadToggle = GUILayout.Toggle(additiveLoadToggle, "Load additive");
            if (GUILayout.Button("Select scene folder")) EditorUtils.HighlightOrCreateFolder(sceneFolder);
            if (GUILayout.Button("Select test folder")) EditorUtils.HighlightOrCreateFolder(testFolder);
            if (GUILayout.Button("Refresh")) OnProjectChange();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Scene Folder Path");
            sceneFolder = EditorGUILayout.TextField(sceneFolder);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Test Folder Path");
            testFolder = EditorGUILayout.TextField(testFolder);
            EditorGUILayout.EndHorizontal();
            
            if (scenes.Count > 0) GUILayout.Label("Game Scenes", EditorStyles.boldLabel);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            for (var i = 0; i < scenes.Count; i++)
            {
                SceneAsset sceneAsset = scenes[i];
                GUILayout.Space(10);
                if (GUILayout.Button(sceneAsset.name, GUILayout.Height(50)) == false) continue;

                LoadScene(sceneAsset, additiveLoadToggle);
            }

            if (testScenes.Count > 0) GUILayout.Label("Test Scenes", EditorStyles.boldLabel);

            for (var i = 0; i < testScenes.Count; i++)
            {
                SceneAsset sceneAsset = testScenes[i];
                GUILayout.Space(10);
                if (GUILayout.Button(sceneAsset.name, GUILayout.Height(50)) == false) continue;

                LoadScene(sceneAsset, additiveLoadToggle);
            }

            EditorGUILayout.EndScrollView();
        }

        static void LoadScene(SceneAsset sceneAsset, bool additive)
        {
            int countLoaded = SceneManager.sceneCount;
            Scene[] loadedScenes = new Scene[countLoaded];
            for (int i = 0; i < countLoaded; i++)
            {
                loadedScenes[i] = SceneManager.GetSceneAt(i);
            }

            if (additive)
            {
                if (Application.isPlaying) SceneManager.LoadScene(sceneAsset.name, LoadSceneMode.Additive);
                else EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneAsset), OpenSceneMode.Additive);
                return;
            }
            // returns false if canceled
            bool shouldLoad = EditorSceneManager.SaveModifiedScenesIfUserWantsTo(loadedScenes);
            if (shouldLoad == false) return;
            if (Application.isPlaying) SceneManager.LoadScene(sceneAsset.name);
            else EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneAsset));
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