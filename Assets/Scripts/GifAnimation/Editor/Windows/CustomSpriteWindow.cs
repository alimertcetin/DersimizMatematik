using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using XIVEditor.Utils;

namespace XIVEditor.Windows
{
    // https://forum.unity.com/threads/extending-instead-of-replacing-built-in-inspectors.407612/
    public class CustomSpriteWindow : EditorWindow
    {
        //Unity's built-in editor
        static Editor defaultEditor;
        static Sprite sprite;

        public static void Show(Sprite sprite)
        {
            CustomSpriteWindow.sprite = sprite;
            EditorWindow.GetWindow<CustomSpriteWindow>(nameof(CustomSpriteWindow)).Show();
        }
    
        void OnEnable()
        {
            //When this inspector is created, also create the built-in inspector
            defaultEditor = Editor.CreateEditor(sprite, Type.GetType("UnityEditor.SpriteInspector, UnityEditor"));
        }
    
        void OnDisable()
        {
            //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
            //Also, make sure to call any required methods like OnDisable
            MethodInfo disableMethod = defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (disableMethod != null)
                disableMethod.Invoke(defaultEditor, null);
            DestroyImmediate(defaultEditor);
        }

        void OnGUI()
        {
            defaultEditor.OnInspectorGUI();
            if (GUILayout.Button("Select Sprite"))
            {
                AssetUtils.HighlightAsset(sprite);
            }
            
            EditorGUILayout.Space(50);
            
            defaultEditor.DrawPreview(GUILayoutUtility.GetRect(sprite.rect.width, sprite.rect.height));
        }
    }
}