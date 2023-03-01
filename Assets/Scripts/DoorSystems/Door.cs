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
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LessonIsMath.DoorSystems
{
    public class Door : MonoBehaviour, IUIEventListener, IInteractable, ISaveable
    {
        [SerializeField] Transform[] interactionPositions;
        [SerializeField] Transform doorHandle;
        [SerializeField] DoorAnimation doorAnimation;
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        [SerializeField] DoorEventChannelSO lockedDoorUIChannel = default;
        [SerializeField] KeycardItemSO[] requiredKeycards;
        [SerializeField] bool useArithmeticOperation;
        [SerializeField] int maxValueOfAnswer;
        [SerializeField] ArithmeticOperation arithmeticOperation;
        bool isLocked;
        bool[] removedKeycards;
        bool isOpen;

        Inventory inventory;
        IInteractor interactor;

        private void Awake()
        {
            removedKeycards = new bool[requiredKeycards.Length];
            isLocked = useArithmeticOperation;
        }

        void OnEnable()
        {
            inventoryLoadedChannel.Register(OnInventoryLoaded);
        }

        void OnDisable()
        {
            inventoryLoadedChannel.Unregister(OnInventoryLoaded);
        }

        private void OnInventoryLoaded(Inventory obj) => this.inventory = obj;
        bool IInteractable.IsAvailable() => true;

        void IInteractable.Interact(IInteractor interactor)
        {
            this.interactor = interactor;

            if (IsRemovedAllKeycards() == false)
            {
                for (int i = 0; i < removedKeycards.Length; i++)
                {
                    if (removedKeycards[i] || inventory.Contains(requiredKeycards[i].GetItem(), out var index) == false) continue;

                    removedKeycards[i] = true;
                    int amount = 1;
                    inventory.RemoveAt(index, ref amount);
                    break;
                }
                interactor.OnInteractionEnd(this);
                return;
            }
            if (useArithmeticOperation && isLocked)
            {
                lockedDoorUIChannel.RaiseEvent(this, true);
                UIEventSystem.Register<LockedDoor_UI>(this);
                return;
            }
            isOpen = !isOpen;
            if (isOpen) doorAnimation.OpenDoor();
            else doorAnimation.CloseDoor();
            interactor.OnInteractionEnd(this);
        }

        string IInteractable.GetInteractionString()
        {
            if (IsRemovedAllKeycards() == false)
            {
                CountKeycards(out int greenCount, out int yellowCount, out int redCount);

                string str = "You need ";

                if (greenCount > 0) str += $"{greenCount} green".Green();
                if (yellowCount > 0) str += $" {yellowCount} yellow".Yellow();
                if (redCount > 0) str += $" {redCount} red".Red();

                var total = greenCount + yellowCount + redCount;
                if (total < 2) str += " keycard";
                else str += " keycards";

                return total > 0 ? str : "";
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

        InteractionTargetData IInteractable.GetInteractionTargetData(IInteractor interactor)
        {
            var interactorPos = (interactor as Component).transform.position;
            Transform interactionPos = VectorUtils.GetClosest(interactorPos, out _, interactionPositions);
            
            return new InteractionTargetData
            {
                startPos = interactorPos,
                targetPosition = interactionPos.position,
                targetForwardDirection = interactionPos.forward,
            };
        }

        public Vector3 GetHandlePosition() => doorHandle.position;

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
            //greenCount = ArrayUtils.Count(requiredKeycards, (itemSO) => itemSO.item.KeycardType == KeycardType.Green);
            //yellowCount = ArrayUtils.Count(requiredKeycards, (itemSO) => itemSO.item.KeycardType == KeycardType.Yellow);
            //redCount = ArrayUtils.Count(requiredKeycards, (itemSO) => itemSO.item.KeycardType == KeycardType.Red);
            greenCount = 0;
            yellowCount = 0;
            redCount = 0;
            for (int i = 0; i < requiredKeycards.Length; i++)
            {
                if (removedKeycards[i]) continue;

                switch (requiredKeycards[i].item.KeycardType)
                {
                    case KeycardType.Green:
                        greenCount++;
                        break;
                    case KeycardType.Yellow:
                        yellowCount++;
                        break;
                    case KeycardType.Red:
                        redCount++;
                        break;
                    default: throw new NotImplementedException(requiredKeycards[i].item.KeycardType + " is not implemented.");
                }
            }
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

            if (isOpen) doorAnimation.OpenDoor();
            else doorAnimation.CloseDoor();
        }

        #endregion


    }
}