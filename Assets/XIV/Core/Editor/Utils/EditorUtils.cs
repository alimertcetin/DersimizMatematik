using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XIVEditor.Utils
{
    public static class EditorUtils
    {
        public static GameObject CreatePrefab(GameObject obj, float distanceToSceneView = 5f)
        {
            GameObject prefab = (GameObject)PrefabUtility.InstantiatePrefab(obj, SceneManager.GetActiveScene());

            if (EditorWindow.HasOpenInstances<SceneView>() == false)
            {
                var focusedWindowType = EditorWindow.focusedWindow.GetType();
                var currentSceneView = EditorWindow.CreateWindow<SceneView>();
                
                Transform currentCamTransform = currentSceneView.camera.transform;
                prefab.transform.position = currentCamTransform.position + currentCamTransform.forward * distanceToSceneView;
                prefab.transform.rotation = Quaternion.Euler(0, currentCamTransform.eulerAngles.y, 0);
                currentSceneView.Close();
                EditorWindow.FocusWindowIfItsOpen(focusedWindowType);
            }
            else
            {
                var currentSceneView = EditorWindow.GetWindow<SceneView>();
                
                Transform currentCamTransform = currentSceneView.camera.transform;
                prefab.transform.position = currentCamTransform.position + currentCamTransform.forward * distanceToSceneView;
                prefab.transform.rotation = Quaternion.Euler(0, currentCamTransform.eulerAngles.y, 0);
            }

            Undo.RegisterCreatedObjectUndo(prefab, "Created " + prefab.name);
            Selection.activeObject = prefab;
            return prefab;
        }

        public static void Select(string directory)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(directory, typeof(Object));
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }

        public static void Select(Object asset)
        {
            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }
        
        public static void Highlight(string directory)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(directory, typeof(Object));
            EditorGUIUtility.PingObject(obj);
        }
        
        public static void Highlight(Object asset)
        {
            EditorGUIUtility.PingObject(asset);
        }

        public static void HighlightOrCreateFolder(string path)
        {
            var appPath = Application.dataPath.Replace("/Assets", string.Empty);
            var directoryPath = Path.Combine(appPath, path);
            directoryPath = directoryPath.Replace('/', '\\');
            if (Directory.Exists(directoryPath))
            {
                Highlight(path);
                return;
            }
            if (EditorUtility.DisplayDialog("Directory Not Found", $"Directory {path} is not exists. Would you like to create?", "Yes", "No"))
            {
                Directory.CreateDirectory(directoryPath);
                AssetDatabase.Refresh();
                Highlight(path);
            }
        }
        
    }
}