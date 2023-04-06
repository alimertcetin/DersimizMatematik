using System;
using UnityEditor;
using UnityEngine;
using XIV.InventorySystem.ScriptableObjects.NonSerializedData;
using XIV.Utils;

namespace XIV.InventorySystem.XIVEditor
{
    [CustomEditor(typeof(NonSerializedItemDataSO), true, isFallback = false), CanEditMultipleObjects]
    public class NonSerializedItemDataSOEditor : Editor
    {
        static bool isCached;
        static GUIContent cached;

        void OnEnable()
        {
            isCached = false;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck()) isCached = false;
            
            var itemDataSO = (NonSerializedItemDataSO)target;
            if (isCached == false)
            {
                if (itemDataSO.uiSprite != null)
                {
                    cached = new GUIContent(TextureUtils.CreateTexture(itemDataSO.uiSprite));
                    isCached = true;
                }
            }

            if (isCached) GUILayout.Label(cached);
        }
    }
}