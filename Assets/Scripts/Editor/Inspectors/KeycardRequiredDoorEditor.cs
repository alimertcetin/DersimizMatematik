using System;
using LessonIsMath.InventorySystem.ItemsSOs;
using LessonIsMath.DoorSystems;
using UnityEditor;
using UnityEngine;
using XIVEditor.Utils;

namespace LessonIsMath.XIVEditor.Inspectors
{
    [CustomEditor(typeof(KeycardRequiredDoor))]
    public class KeycardRequiredDoorEditor : Editor
    {
        KeycardItemSO[] keycards = new KeycardItemSO[3];
        bool isSet;
        const string FIELD_NAME = "requiredKeycards";
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (isSet == false)
            {
                isSet = true;
                keycards[0] = AssetUtils.GetScriptableObject<KeycardItemSO>("Keycard_Green");
                keycards[1] = AssetUtils.GetScriptableObject<KeycardItemSO>("Keycard_Yellow");
                keycards[2] = AssetUtils.GetScriptableObject<KeycardItemSO>("Keycard_Red");
            }

            var keycardRequiredDoor = (KeycardRequiredDoor)target;
            for (int i = 0; i < 3; i++)
            {
                if (GUILayout.Button("Add " + keycards[i].name))
                {
                    Add(keycardRequiredDoor, i);
                }
            }
        }

        void Add(KeycardRequiredDoor keycardRequiredDoor, int keycardIndex)
        {
            Undo.RecordObject(keycardRequiredDoor, "Add Keycard");
            KeycardItemSO[] requiredItems = ReflectionUtils.GetFieldValue<KeycardItemSO[]>(FIELD_NAME, keycardRequiredDoor);
            if (requiredItems == null) requiredItems = Array.Empty<KeycardItemSO>();
            Array.Resize(ref requiredItems, requiredItems.Length + 1);
            requiredItems[^1] = keycards[keycardIndex];
            ReflectionUtils.SetField(FIELD_NAME, keycardRequiredDoor, requiredItems);
        }

        void OnDisable()
        {
            isSet = false;
        }
    }
}