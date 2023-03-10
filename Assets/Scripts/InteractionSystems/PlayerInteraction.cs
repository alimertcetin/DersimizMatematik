using System;
using System.Collections.Generic;
using LessonIsMath.DoorSystems;
using LessonIsMath.Input;
using LessonIsMath.PlayerSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.Extensions;
using XIV.XIVMath;
#if UNITY_EDITOR
using UnityEditor;
using XIV;
#endif

namespace LessonIsMath.InteractionSystems
{
    public struct InteractionData
    {
        public InteractionTargetData targetData;
        public Action OnTargetReached;
        public Action OnMovementCanceled;
    }
    
    // TODO : Split interactions
    public class PlayerInteraction : MonoBehaviour, PlayerControls.IInteractionActions, IInteractor
    {
        [Tooltip("To define the interaction area")]
        [SerializeField] Collider triggerCollider;
        [SerializeField] StringEventChannelSO notificationChannel = default;
        [SerializeField] float openDoorForce;
        HashSet<IInteractable> interactables = new HashSet<IInteractable>(8);
        List<Collider> otherColliders = new List<Collider>(8);
        IInteractable currentInteractable;
        AutoMovementInput autoMovementInput;
        PlayerAnimationController playerAnimationController;
        PlayerDoorInteraction playerDoorInteraction;

        const float INTERACTION_THRESHOLD = 0.5f;
        BoxCollider interactionBoundingBox;

        void Awake()
        {
            triggerCollider.gameObject.AddComponent<InteractionHelper>().playerInteraction = this;
            InputManager.Interaction.SetCallbacks(this);
            autoMovementInput = GetComponent<AutoMovementInput>();
            playerDoorInteraction = GetComponent<PlayerDoorInteraction>();
            playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
            interactionBoundingBox = new GameObject("InteractionBoundingBox", typeof(BoxCollider)).GetComponent<BoxCollider>();
            interactionBoundingBox.isTrigger = true;
        }

        void OnEnable()
        {
            InputManager.Interaction.Enable();
        }

        void OnDisable()
        {
            InputManager.Interaction.Disable();
        }

        void PlayerControls.IInteractionActions.OnInteract(InputAction.CallbackContext context)
        {
            void OnTargetReached(IInteractable interactable)
            {
                if (interactable.IsAvailableForInteraction() == false) return;

                var targetData = interactable.GetInteractionTargetData(this);
                // TODO : Consider making target data class and update it when necessary
                // target data could be changed until we reach the target
                var distance = Vector3.Distance(transform.position, targetData.targetPosition);

                if (distance > INTERACTION_THRESHOLD)
                {
                    autoMovementInput.SetTarget(new InteractionData
                    {
                        targetData = targetData,
                        OnTargetReached = () => OnTargetReached(interactable),
                    });
                    return;
                }

                HandleInteraction(interactable);
            }
            
            void HandleInteraction(IInteractable interactable)
            {
                InputManager.CharacterMovement.Disable();
                InputManager.Interaction.Disable();
                playerAnimationController.HandleInteractionAnimation(interactable, (IInteractable interactable) =>
                {
                    InputManager.CharacterMovement.Enable();
                    notificationChannel.RaiseEvent("", false);
                    if (interactable == null)
                    {
                        InputManager.Interaction.Enable();
                        return;
                    }
                    interactable.Interact(this);
                });
            }

            if (context.performed == false || currentInteractable == null) return;

            InteractionTargetData interactionTargetData = currentInteractable.GetInteractionTargetData(this);
            var dot = Vector3.Dot(transform.forward, -interactionTargetData.targetForwardDirection);
            var distance = Vector3.Distance(transform.position, interactionTargetData.targetPosition);
            if (distance > INTERACTION_THRESHOLD || dot < 0.6f)
            {
                var interactable = currentInteractable;
                autoMovementInput.SetTarget(new InteractionData
                {
                    targetData = interactionTargetData,
                    OnTargetReached = () => OnTargetReached(interactable),
                });
                return;
            }
            
            HandleInteraction(currentInteractable);
        }

        void IInteractor.OnInteractionEnd(IInteractable interactable)
        {
            InputManager.Interaction.Enable();
            if (interactable.IsAvailableForInteraction() && IsBlockedByAnything(interactable as Component) == false && interactables.Contains(interactable))
            {
                ChangeCurrentInteractable(interactable);
                return;
            }

            interactables.Remove(interactable);
            if (interactable is DoorManager doorManager && doorManager.GetState().HasFlag(DoorState.Unlocked))
            {
                playerDoorInteraction.SetTarget(openDoorForce, doorManager.managedDoors);
            }

            RefreshCurrentInteractable();
        }

        public void TriggerEnter(Collider other)
        {
            if (other.isTrigger == false)
            {
                otherColliders.Add(other);
            }

            if (other.TryGetComponent<IInteractable>(out var otherInteractable))
            {
                if (otherInteractable is DoorManager doorManager && doorManager.GetState().HasFlag(DoorState.Unlocked))
                {
                    playerDoorInteraction.SetTarget(openDoorForce, doorManager.managedDoors);
                    return;
                }
                
                interactables.Add(otherInteractable);
            }

            RefreshCurrentInteractable();
        }

        public void TriggerExit(Collider other)
        {
            if (other.isTrigger == false)
            {
                otherColliders.Remove(other);
            }

            if (other.TryGetComponent<IInteractable>(out var otherInteractable))
            {
                if (otherInteractable is DoorManager doorManager)
                {
                    for (int i = 0; i < doorManager.managedDoors.Length; i++)
                    {
                        if (playerDoorInteraction.HasTarget(doorManager.managedDoors[i]))
                        {
                            playerDoorInteraction.ClearTarget();
                            return;
                        }
                    }
                }
                interactables.Remove(otherInteractable);
            }

            RefreshCurrentInteractable();
        }

        IInteractable GetClosestInteractable()
        {
            float distance = float.MaxValue;
            IInteractable closestInteractable = default;
            var currentPos = this.transform.position;
            foreach (IInteractable interactable in interactables)
            {
                Component interactableComponent = interactable as Component;
                var interactablePos = interactableComponent.transform.position;
                var dist = Vector3.Distance(currentPos, interactablePos);
                if (dist < distance && IsBlockedByAnything(interactableComponent) == false)
                {
                    distance = dist;
                    closestInteractable = interactable;
                }
            }
            return closestInteractable;
        }

        bool IsBlockedByAnything(Component target)
        {
            var currentPos = transform.position;
            // A little cheat to workaround of overlapping object problems
            var targetPos = Vector3.MoveTowards(target.transform.position, currentPos, 0.25f);
            var center = currentPos + (targetPos - currentPos) * 0.5f;
            interactionBoundingBox.transform.position = center;
            var size = (targetPos - currentPos).Abs();
            interactionBoundingBox.size = size;

            var targetCollider = target.GetComponent<Collider>();
            for (int i = 0; i < otherColliders.Count; i++)
            {
                var otherCollider = otherColliders[i];
                if (otherCollider == targetCollider) continue;
                
                if (Physics.ComputePenetration(interactionBoundingBox, center, interactionBoundingBox.transform.rotation,
                        otherCollider, otherCollider.transform.position, otherCollider.transform.rotation, out var dir, out var distance))
                {
                    XIVDebug.DrawLine(center, center + (dir * distance), 8f);
                    Debug.Log($"{target} is blocked by {otherColliders[i]}");
                    return true;
                }
            }
            
            return false;
        }

        void RefreshCurrentInteractable()
        {
            ChangeCurrentInteractable(GetClosestInteractable());
        }

        void ChangeCurrentInteractable(IInteractable otherInteractable)
        {
            currentInteractable = otherInteractable;
            if (currentInteractable != null) notificationChannel.RaiseEvent(currentInteractable.GetInteractionString());
            else notificationChannel.RaiseEvent("", false);
        }

        class InteractionHelper : MonoBehaviour
        {
            public PlayerInteraction playerInteraction;

            void OnTriggerEnter(Collider other)
            {
                playerInteraction.TriggerEnter(other);
            }

            void OnTriggerExit(Collider other)
            {
                playerInteraction.TriggerExit(other);
            }
        }
    }

}