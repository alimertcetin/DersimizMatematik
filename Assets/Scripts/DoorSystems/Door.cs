using System;
using UnityEngine;
using XIV.Extensions;
using XIV.InventorySystem.Items;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;
using XIV.Utils;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.InteractionSystems;
using LessonIsMath.Input;
using XIV.SaveSystems;
using LessonIsMath.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LessonIsMath.DoorSystems
{
    [RequireComponent(typeof(DoorAnimation))]
    public class Door : MonoBehaviour, IUIEventListener, IInteractable, ISaveable
    {
        [SerializeField] DoorEventChannelSO lockedDoorUIChannel = default;
        [SerializeField] KeycardItemSO[] requiredKeycards;
        [SerializeField] bool useArithmeticOperation;
        [SerializeField] int maxValueOfAnswer;
        [SerializeField] ArithmeticOperation arithmeticOperation;
        DoorAnimation doorAnimation;
        bool isLocked;
        bool[] removedKeycards;
        bool isOpen;

        IInteractor interactor;

        private void Awake()
        {
            removedKeycards = new bool[requiredKeycards.Length];
            doorAnimation = GetComponent<DoorAnimation>();
            isLocked = useArithmeticOperation;
        }

        bool IInteractable.CanInteract() => true;

        void IInteractable.Interact(IInteractor interactor)
        {
            this.interactor = interactor;

            // TODO : Keycard logic here
            if (useArithmeticOperation && isLocked)
            {
                lockedDoorUIChannel.RaiseEvent(this, true);
                UIEventSystem.Register<LockedDoor_UI>(this);
                return;
            }
            isOpen = !isOpen;
            doorAnimation.Interact(isOpen);
            interactor.OnInteractionEnd(this);
        }

        string IInteractable.GetInteractionString()
        {
            if (InputManager.PlayerControls.LockedDoorUI.enabled)
            {
                return "";
            }
            else if (IsRemovedAllKeycards() == false)
            {
                CountKeycards(out int greenCount, out int yellowCount, out int redCount);

                static string Format(int amount, string color) => $"{amount} {color} keycard ";

                string str = "You need ";

                if (greenCount > 0) str += Format(greenCount, "green".Green());
                if (yellowCount > 0) str += Format(yellowCount, "yellow".Yellow());
                if (redCount > 0) str += Format(redCount, "red".Red());

                return greenCount > 0 || yellowCount > 0 || redCount > 0 ? str : "";
            }
            else if (isLocked == false)
            {
                return isOpen
                    ? "Press " + InputManager.InteractionKeyName + " to Close"
                    : "Press " + InputManager.InteractionKeyName + " to Open";
            }
            else if (isLocked)
            {
                return "Door is locked. Press " + InputManager.InteractionKeyName + " button to see the Question.";
            }
            else
            {
                return "";
            }
        }

        public bool SolveQuestion(int answer)
        {
            if (arithmeticOperation.CalculateAnswer() == answer)
            {
                isLocked = false;
                return true;
            }
            return false;
        }

        void CountKeycards(out int greenCount, out int yellowCount, out int redCount)
        {
            greenCount = ArrayUtils.Count(requiredKeycards, (itemSO) => itemSO.item.KeycardType == KeycardType.Green);
            yellowCount = ArrayUtils.Count(requiredKeycards, (itemSO) => itemSO.item.KeycardType == KeycardType.Yellow);
            redCount = ArrayUtils.Count(requiredKeycards, (itemSO) => itemSO.item.KeycardType == KeycardType.Red);
        }

        bool IsRemovedAllKeycards()
        {
            int length = removedKeycards.Length;
            for (int i = 0; i < length; i++)
            {
                if (removedKeycards[i] == false) return false;
            }
            return true;
        }

        public string GetQuestionString()
        {
            return arithmeticOperation.ToString();
        }

        void IUIEventListener.OnShowUI(GameUI ui) { }
        void IUIEventListener.OnHideUI(GameUI ui)
        {
            interactor.OnInteractionEnd(this);
            UIEventSystem.Unregister<LockedDoor_UI>(this);
        }

#if UNITY_EDITOR

        [ContextMenu(nameof(CalculateAnswer))]
        void CalculateAnswer()
        {
            if (useArithmeticOperation == false) return;
            Undo.RecordObject(this, gameObject.name + nameof(CalculateAnswer));
            arithmeticOperation.CalculateAnswer();
            EditorUtility.SetDirty(this);
        }

        [ContextMenu(nameof(GenerateQuestionDependingOnAnswer))]
        void GenerateQuestionDependingOnAnswer()
        {
            if (useArithmeticOperation == false) return;
            Undo.RecordObject(this, gameObject.name + nameof(GenerateQuestionDependingOnAnswer));
            arithmeticOperation.GenerateQuestion(arithmeticOperation.answer, maxValueOfAnswer);
            EditorUtility.SetDirty(this);
        }

        [ContextMenu(nameof(GenerateRandomQuestion))]
        void GenerateRandomQuestion()
        {
            if (useArithmeticOperation == false) return;
            Undo.RecordObject(this, gameObject.name + nameof(GenerateRandomQuestion));
            arithmeticOperation.GenerateQuestion(maxValueOfAnswer);
            EditorUtility.SetDirty(this);
        }

#endif

        #region -_- Save -_-

        [Serializable]
        private struct SaveData
        {
            public bool isLocked;
            public bool isOpen;
            public bool[] removedKeycards;
        }

        object ISaveable.CaptureState()
        {
            return new SaveData
            {
                isLocked = isLocked,
                isOpen = isOpen,
                removedKeycards = removedKeycards,
            };
        }

        void ISaveable.RestoreState(object state)
        {
            SaveData saveData = (SaveData)state;
            removedKeycards = saveData.removedKeycards;
            isOpen = saveData.isOpen;
            isLocked = saveData.isLocked;

            doorAnimation.Interact(isOpen);
        }

        #endregion


    }
}