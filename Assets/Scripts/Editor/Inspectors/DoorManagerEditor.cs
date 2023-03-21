using LessonIsMath.DoorSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.XIVEditor.Windows;
using UnityEditor;
using UnityEngine;
using XIV.Utils;
using XIVEditor.Utils;
using Object = UnityEngine.Object;

namespace LessonIsMath.XIVEditor.Inspectors
{
    [CustomEditor(typeof(DoorManager))]
    [CanEditMultipleObjects]
    public class DoorManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            foreach (Object target in targets)
            {
                var doorManager = (DoorManager)target;
                bool hasKeycard = doorManager.gameObject.TryGetComponent<KeycardRequiredDoor>(out var keycardRequiredDoor);
                var keycardDoorStr = hasKeycard ? "Remove Keycard" : "Require Keycard";
                if (GUILayout.Button(keycardDoorStr))
                {
                    if (hasKeycard)
                    {
                        Undo.DestroyObjectImmediate(keycardRequiredDoor.GetCardReader().gameObject);
                        Undo.DestroyObjectImmediate(keycardRequiredDoor);
                    }
                    else
                    {
                        SetupKeycard(doorManager);
                    }
                }

                bool hasArithmeticOperation = doorManager.gameObject.TryGetComponent<ArithmeticOperationDoor>(out var arithmeticOperationDoor);
                var arithmeticOpButtonStr = hasArithmeticOperation ? "Remove Arithmetic Operation" : "Require Arithmetic Operation";
                if (GUILayout.Button(arithmeticOpButtonStr))
                {
                    if (hasArithmeticOperation)
                    {
                        Undo.DestroyObjectImmediate(arithmeticOperationDoor);
                    }
                    else
                    {
                        SetupArithmetiOperation(doorManager);
                    }
                }
            }
        }

        void SetupArithmetiOperation(DoorManager doorManager)
        {
            if (doorManager.gameObject.TryGetComponent<ArithmeticOperationDoor>(out _))
            {
                Debug.LogWarning(doorManager + " has already arithmetic operation setup");
                return;
            }
            
            ArithmeticOperationDoor arithmeticOperationDoor = Undo.AddComponent<ArithmeticOperationDoor>(doorManager.gameObject);
            var arithmeticDoorUIChannel = AssetUtils.GetScriptableObject<ArithmeticDoorEventChannelSO>("arithmeticDoorUIChannel");
            ReflectionUtils.SetField("arithmeticDoorUIChannel", arithmeticOperationDoor, arithmeticDoorUIChannel);
            var arithmeticOperation = new ArithmeticOperation();
            arithmeticOperation.GenerateQuestion(500);
            ReflectionUtils.SetField("arithmeticOperation", arithmeticOperationDoor, arithmeticOperation);
            ReflectionUtils.SetField("maxValueOfAnswer", arithmeticOperationDoor, 500);
        }

        void SetupKeycard(DoorManager doorManager)
        {
            var currentSelection = Selection.objects;
            KeycardRequiredDoor keycardRequiredDoor = Undo.AddComponent<KeycardRequiredDoor>(doorManager.gameObject);
            var keycardUIChannel = AssetUtils.GetScriptableObject<BoolEventChannelSO>("keycardUIChannel");
            ReflectionUtils.SetField("keycardUIChannel", keycardRequiredDoor, keycardUIChannel);
            var doorCreatorEditor = EditorWindow.CreateWindow<DoorCreatorEditor>();
            doorCreatorEditor.Show(true);
            var cardReaderGo = doorCreatorEditor.CreateCardReader();
            doorCreatorEditor.Close();
            cardReaderGo.transform.position = doorManager.transform.position;
            ReflectionUtils.SetField("cardReader", keycardRequiredDoor, cardReaderGo.GetComponent<CardReader>());
            
            Selection.objects = currentSelection;
        }
        
    }
}