using UnityEditor;
using UnityEngine;
using XIV.XIVEditor.Utils;

namespace LessonIsMath.XIVEditor.Windows
{
    public class KeycardCreatorWindow : EditorWindow
    {
        GameObject[] keycardPrefabs = new GameObject[3];
        bool isSet;

        void OnGUI()
        {
            if (isSet == false)
            {
                isSet = true;
                string[] guids = AssetDatabase.FindAssets("t:prefab");
                for (var i = 0; i < guids.Length; i++)
                {
                    var prefabGo = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guids[i]));
                    if (prefabGo == null) continue;
                    if (prefabGo.name == "KeycardPrefab_Green") keycardPrefabs[0] = prefabGo;
                    if (prefabGo.name == "KeycardPrefab_Yellow") keycardPrefabs[1] = prefabGo;
                    if (prefabGo.name == "KeycardPrefab_Red") keycardPrefabs[2] = prefabGo;
                    
                }

            }

            for (int i = 0; i < 3; i++)
            {
                keycardPrefabs[i] = (GameObject)EditorGUILayout.ObjectField(keycardPrefabs[i].name, keycardPrefabs[i], typeof(GameObject), false);
            }
            
            GUILayout.Space(20);

            for (int i = 0; i < 3; i++)
            {
                if (GUILayout.Button("Instantiate " + keycardPrefabs[i].name, GUILayout.Height(20)))
                {
                    EditorUtils.CreatePrefab(keycardPrefabs[i], 2f);
                }
            }
            
        }
        
    }
}