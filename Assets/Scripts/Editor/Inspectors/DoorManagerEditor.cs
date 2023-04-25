using LessonIsMath.DoorSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.XIVEditor.Windows;
using UnityEditor;
using UnityEngine;
using XIV.XIVEditor.Utils;
using XIV.Core.Utils;
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
                        CardReader[] cardReaders = ReflectionUtils.GetFieldValue<CardReader[]>("cardReaders", keycardRequiredDoor);
                        for (int i = 0; i < cardReaders.Length; i++)
                        {
                            Undo.DestroyObjectImmediate(cardReaders[i].gameObject);
                        }
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
            
            // Use default references
            // var arithmeticDoorUIChannel = AssetUtils.GetScriptableObject<ArithmeticDoorEventChannelSO>("arithmeticDoorUIChannel");
            // ReflectionUtils.SetField("arithmeticDoorUIChannel", arithmeticOperationDoor, arithmeticDoorUIChannel);
            //
            // var arithmeticDoorQuestionTimerChannel = AssetUtils.GetScriptableObject<ArithmeticDoorEventChannelSO>("arithmeticDoorQuestionTimerChannel");
            // ReflectionUtils.SetField("arithmeticDoorQuestionTimerChannel", arithmeticOperationDoor, arithmeticDoorQuestionTimerChannel);
            
            ReflectionUtils.SetField("generateQuestionDuration", arithmeticOperationDoor, 90f);
            
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
            doorCreatorEditor.Show();
            var cardReader1 = doorCreatorEditor.CreateCardReader();
            var cardReader2 = doorCreatorEditor.CreateCardReader();
            doorCreatorEditor.Close();
            var doorTransform = doorManager.transform;
            var doorTransformPosition = doorTransform.position;
            var doorTransformForward = doorTransform.forward;
            cardReader1.transform.position = doorTransformPosition + doorTransformForward * 0.2f;
            cardReader2.transform.position = doorTransformPosition + doorTransformForward * 0.2f + Vector3.up * 0.2f;
            cardReader1.transform.rotation = Quaternion.LookRotation(doorTransformForward);
            cardReader2.transform.rotation = Quaternion.LookRotation(-doorTransformForward);
            
            Undo.SetTransformParent(cardReader1.transform, doorTransform, true, "SetParent");
            Undo.SetTransformParent(cardReader2.transform, doorTransform, true, "SetParent");
            
            ReflectionUtils.SetField("cardReaders", keycardRequiredDoor, new CardReader[]
            {
                cardReader1.GetComponent<CardReader>(),
                cardReader2.GetComponent<CardReader>()
            });
            
            Selection.objects = currentSelection;
        }
        
    }
}