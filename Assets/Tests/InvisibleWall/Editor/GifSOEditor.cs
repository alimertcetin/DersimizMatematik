using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using XIV.Utils;
using Object = UnityEngine.Object;

namespace XIV.EditorUtils
{
    [CustomEditor(typeof(GifSO))]
    public class GifSOEditor : Editor
    {
        static readonly int frameWidth = 32;
        static readonly int frameHeight = 32;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GifSO gifSO = (GifSO)target;
            var framesLength = gifSO.frames.Length;
            if (framesLength == 0) return;
            
            float currentViewWidth = EditorGUIUtility.currentViewWidth / 1.5f; // offset
            int maxHorizontalFrameCount = (int)(currentViewWidth / frameWidth);

            for (int i = 0; i < framesLength;)
            {
                EditorGUILayout.BeginHorizontal();
                int left = framesLength - i;
                var horizontalCount = left > maxHorizontalFrameCount ? maxHorizontalFrameCount : left;
                for (int j = 0; j < horizontalCount; j++, i++)
                {
                    Sprite sprite = gifSO.frames[i];
                    Texture2D texture = TextureUtils.CreateTexture(sprite, frameWidth, frameHeight);
                    if (GUILayout.Button(new GUIContent(texture, $"Open {sprite.name} in new Inspector")))
                    {
                        OpenInspectorForAsset(sprite);
                        HighlightAsset(sprite);
                    }
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }
        
        public static void OpenInspectorForAsset(Object asset)
        {
            Type inspectorType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
            EditorWindow inspectorWindow = (EditorWindow)CreateInstance(inspectorType);
            MethodInfo targetMethod = inspectorType.GetMethod("SetObjectsLocked", BindingFlags.NonPublic | BindingFlags.Instance);
            targetMethod.Invoke(inspectorWindow, new object[] { new List<UnityEngine.Object>() { asset } });
            inspectorWindow.Show();
        }

        public static void SelectAsset(Object asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            Selection.activeObject = AssetDatabase.LoadAssetAtPath(path, asset.GetType());
        }

        public static void HighlightAsset(Object asset)
        {
            EditorGUIUtility.PingObject(asset);
        }
    }
}
