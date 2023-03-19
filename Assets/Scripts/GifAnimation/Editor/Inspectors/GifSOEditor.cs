using UnityEditor;
using UnityEngine;
using XIV.GifAnimation.ScriptableObjects;
using XIV.Utils;

namespace XIV.GifAnimation.XIVEditor.Inspectors
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
                        XIVEditor.Windows.CustomSpriteWindow.Show(sprite);
                    }
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
