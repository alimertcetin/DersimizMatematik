using System;
using LessonIsMath.Input;
using LessonIsMath.InteractionSystems;
using LessonIsMath.UI;
using UnityEngine;
using XIV.Extensions;

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
    
    public class DoorManager : MonoBehaviour, IInteractable
    {
        public Door[] managedDoors;
        public Transform[] interactionPositions;

        ArithmeticOperationDoor arithmeticOperationDoor;
        KeycardRequiredDoor keycardRequiredDoor;

        bool arithmeticOperationLock;
        bool keycardLock;
        IInteractor interactor;
        public bool IsInInteraction { get; private set; }

        void Awake()
        {
            keycardLock = TryGetComponent(out keycardRequiredDoor);
            arithmeticOperationLock = TryGetComponent(out arithmeticOperationDoor);
            HandleDoorActivation();
        }

        public void RefreshDoorState()
        {
            HandleDoorActivation();
        }

        void HandleDoorActivation()
        {
            bool val = true;
            if (arithmeticOperationLock) val = arithmeticOperationDoor.IsQuestionSolved();
            for (int i = 0; i < managedDoors.Length; i++)
            {
                managedDoors[i].enabled = val;
            }
        }

        public void OnInteractionEnd()
        {
            IsInInteraction = false;
            interactor.OnInteractionEnd(this);
        }

        InteractionSettings IInteractable.GetInteractionSettings()
        {
            return new InteractionSettings
            {
                disableInteractionKey = GetState().HasFlag(DoorState.RequiresKeycard) == false,
                suspendMovement = true,
            };
        }

        bool IInteractable.IsAvailableForInteraction()
        {
            if (arithmeticOperationLock)
            {
                return arithmeticOperationDoor.IsQuestionSolved() == false && IsInInteraction == false;
            }

            if (keycardLock)
            {
                return keycardRequiredDoor.IsKeycardRequired() && IsInInteraction == false;
            }
            return false;
        }

        void IInteractable.Interact(IInteractor interactor)
        {
            IsInInteraction = true;
            this.interactor = interactor;
            if (keycardLock && keycardRequiredDoor.IsKeycardRequired())
            {
                keycardRequiredDoor.OnInteract();
                return;
            }
            if (arithmeticOperationLock && arithmeticOperationDoor.IsQuestionSolved() == false)
            {
                arithmeticOperationDoor.OnInteract();
                return;
            }

            OnInteractionEnd();
        }

        string IInteractable.GetInteractionString()
        {
            if (keycardLock && keycardRequiredDoor.IsKeycardRequired())
            {
                return keycardRequiredDoor.GetKeycardString();
            }

            if (arithmeticOperationLock && arithmeticOperationDoor.IsQuestionSolved() == false)
            {
                return "Door is locked. Press " + InputManager.InteractionKeyName + " button to see the Question.";
            }

            return "";
        }

        InteractionPositionData IInteractable.GetInteractionPositionData(IInteractor interactor)
        {
            var interactorPos = (interactor as Component).transform.position;
            Transform interactionPos = interactionPositions.GetClosest(interactorPos);
            
            return new InteractionPositionData
            {
                startPos = interactorPos,
                targetPosition = interactionPos.position,
                targetForwardDirection = interactionPos.forward,
            };
        }

        public DoorState GetState()
        {
            DoorState state = DoorState.None;
            bool requiresKeycard = keycardLock && keycardRequiredDoor.IsKeycardRequired();
            bool hasQuestion = arithmeticOperationLock && arithmeticOperationDoor.IsQuestionSolved() == false;
            if (requiresKeycard) state |= DoorState.RequiresKeycard;
            if (hasQuestion) state |= DoorState.HasQuestion;
            if (requiresKeycard == false && hasQuestion == false) state |= DoorState.Unlocked;
            
            return state;
        }
    }
}