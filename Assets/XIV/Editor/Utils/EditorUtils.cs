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
    }
}