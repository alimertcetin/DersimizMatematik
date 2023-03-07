using System;
using LessonIsMath.Input;
using LessonIsMath.InteractionSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI;
using UnityEditor;
using UnityEngine;
using XIV.Extensions;
using XIV.InventorySystem;
using XIV.InventorySystem.Items;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;
using XIV.SaveSystems;
using XIV.Utils;

namespace LessonIsMath.DoorSystems
{
    [Flags]
    public enum DoorState
    {
        None = 0,
        Unlocked = 1,
        RequiresKeycard = 2,
        HasQuestion = 4,
    }
    public class DoorManager : MonoBehaviour, IUIEventListener, IInteractable, ISaveable
    {
        public Door[] managedDoors;
        public Transform[] interactionPositions;
        
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        [SerializeField] DoorEventChannelSO lockedDoorUIChannel;
        [SerializeField] KeycardItemSO[] requiredKeycards;
        [SerializeField] bool useArithmeticOperation;
        [SerializeField] int maxValueOfAnswer;
        [SerializeField] ArithmeticOperation arithmeticOperation;
        bool isLocked;
        bool[] removedKeycards;

        Inventory inventory;
        IInteractor interactor;

        void Awake()
        {
            removedKeycards = new bool[requiredKeycards.Length];
            isLocked = useArithmeticOperation || IsRemovedAllKeycards() == false;
            HandleDoorActivation();
        }

        void OnEnable() => inventoryLoadedChannel.Register(OnInventoryLoaded);
        void OnDisable() => inventoryLoadedChannel.Unregister(OnInventoryLoaded);

        void HandleDoorActivation()
        {
            for (int i = 0; i < managedDoors.Length; i++)
            {
                managedDoors[i].enabled = !isLocked;
            }
        }

        void OnInventoryLoaded(Inventory inventory) => this.inventory = inventory;
        bool IInteractable.IsAvailableForInteraction() => isLocked;

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
                isLocked = useArithmeticOperation || IsRemovedAllKeycards();
                interactor.OnInteractionEnd(this);
                return;
            }
            if (useArithmeticOperation && isLocked)
            {
                lockedDoorUIChannel.RaiseEvent(this, true);
                UIEventSystem.Register<LockedDoor_UI>(this);
                return;
            }
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

            if (isLocked) return "Door is locked. Press " + InputManager.InteractionKeyName + " button to see the Question.";

            return "";
        }

        InteractionTargetData IInteractable.GetInteractionTargetData(IInteractor interactor)
        {
            var interactorPos = (interactor as Component).transform.position;
            Transform interactionPos = interactionPositions.GetClosest(interactorPos);
            
            return new InteractionTargetData
            {
                startPos = interactorPos,
                targetPosition = interactionPos.position,
                targetForwardDirection = interactionPos.forward,
            };
        }

        public DoorState GetState()
        {
            DoorState state = DoorState.None;
            if (IsRemovedAllKeycards() == false) state |= DoorState.RequiresKeycard;
            if (useArithmeticOperation && isLocked) state |= DoorState.HasQuestion;
            if (isLocked == false) state |= DoorState.Unlocked;
            
            return state;
        }

        public bool SolveQuestion(int answer)
        {
            if (arithmeticOperation.CalculateAnswer() == answer)
            {
                isLocked = false;
                HandleDoorActivation();
                return true;
            }
            return false;
        }

        void CountKeycards(out int greenCount, out int yellowCount, out int redCount)
        {
            greenCount = 0;
            yellowCount = 0;
            redCount = 0;
            for (int i = 0; i < removedKeycards.Length; i++)
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
            int length = requiredKeycards.Length;
            for (int i = 0; i < length; i++)
            {
                if (removedKeycards[i] == false) return false;
            }
            return true;
        }

        public string GetQuestionString() => arithmeticOperation.ToString();

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

        #region --- Save ---

        [Serializable]
        struct SaveData
        {
            public bool isLocked;
            public bool[] removedKeycards;
        }

        object ISaveable.CaptureState()
        {
            return new SaveData
            {
                isLocked = isLocked,
                removedKeycards = removedKeycards,
            };
        }

        void ISaveable.RestoreState(object state)
        {
            SaveData saveData = (SaveData)state;
            removedKeycards = saveData.removedKeycards;
            isLocked = saveData.isLocked;
            HandleDoorActivation();
        }

        #endregion

    }
}