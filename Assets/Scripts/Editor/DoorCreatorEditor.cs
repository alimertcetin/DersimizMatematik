using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XIV.EditorUtils
{
    public class DoorCreatorEditor : EditorWindow
    {
        GameObject singleDoor;
        GameObject doubleDoor;

        [MenuItem("Lesson Is Math/DoorCreator")]
        static void Init()
        {
            if (EditorWindow.HasOpenInstances<DoorCreatorEditor>())
            {
                EditorWindow.GetWindow<DoorCreatorEditor>().Focus();
                return;
            }

            EditorWindow.CreateWindow<DoorCreatorEditor>().Show();
        }

        void OnBecameVisible()
        {
            titleContent = new GUIContent(nameof(DoorCreatorEditor));
            FindPrefabs();
        }

        void OnGUI()
        {
            DrawObjectField("Single Door", ref singleDoor);
            DrawObjectField("Double Door", ref doubleDoor);

            EditorGUILayout.Space(20);

            if (GUILayout.Button("Create Single Door"))
            {
                CreatePrefab(singleDoor);
            }

            if (GUILayout.Button("Create Double Door"))
            {
                CreatePrefab(doubleDoor);
            }
        }

        static void DrawObjectField(string label, ref GameObject obj)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(100));
            obj = (GameObject)EditorGUILayout.ObjectField(obj, typeof(GameObject), false);
            EditorGUILayout.EndHorizontal();
        }

        void CreatePrefab(GameObject obj)
        {
            GameObject doorPrefab = (GameObject)PrefabUtility.InstantiatePrefab(obj, SceneManager.GetActiveScene());

            if (EditorWindow.HasOpenInstances<SceneView>() == false)
            {
                var focusedWindowType = EditorWindow.focusedWindow.GetType();
                var currentSceneView = EditorWindow.CreateWindow<SceneView>();
                
                Transform currentCamTransform = currentSceneView.camera.transform;
                doorPrefab.transform.position = currentCamTransform.position + currentCamTransform.forward * 5f;
                doorPrefab.transform.rotation = Quaternion.Euler(0, currentCamTransform.eulerAngles.y, 0);
                currentSceneView.Close();
                EditorWindow.FocusWindowIfItsOpen(focusedWindowType);
            }
            else
            {
                var currentSceneView = EditorWindow.GetWindow<SceneView>();
                
                Transform currentCamTransform = currentSceneView.camera.transform;
                doorPrefab.transform.position = currentCamTransform.position + currentCamTransform.forward * 5f;
                doorPrefab.transform.rotation = Quaternion.Euler(0, currentCamTransform.eulerAngles.y, 0);
            }

            Undo.RegisterCreatedObjectUndo(doorPrefab, "Created " + doorPrefab.name);
            Selection.activeObject = doorPrefab;
        }

        void FindPrefabs()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab");

            for (var i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (asset == null) continue;
                var assetName = asset.name.ToLower();
                if (assetName.Contains("door") == false) continue;

                if (assetName.Contains("single")) singleDoor = asset;
                else if (assetName.Contains("double")) doubleDoor = asset;

                if (singleDoor != null && doubleDoor != null) break;
            }
        }

        // void DropAreaGUI()
        // {
        //     Event evt = Event.current;
        //     Rect drop_area = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        //     GUI.Box(drop_area, "Add Trigger");
        //
        //     switch (evt.type)
        //     {
        //         case EventType.DragUpdated:
        //         case EventType.DragPerform:
        //             if (!drop_area.Contains(evt.mousePosition))
        //                 return;
        //
        //             DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        //
        //             if (evt.type == EventType.DragPerform)
        //             {
        //                 DragAndDrop.AcceptDrag();
        //
        //                 foreach (Object dragged_object in DragAndDrop.objectReferences)
        //                 {
        //                     // Do On Drag Stuff here
        //                 }
        //             }
        //
        //             break;
        //     }
        // }

    }
}