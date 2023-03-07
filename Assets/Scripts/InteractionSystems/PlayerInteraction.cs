using System;
using System.Collections.Generic;
using LessonIsMath.DoorSystems;
using LessonIsMath.Input;
using LessonIsMath.PlayerSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
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
        PlayerController playerController;
        PlayerAnimationController playerAnimationController;
        PlayerUnlockedDoorInteraction unlockedDoorInteraction;

        const float INTERACTION_DISTANCE = 0.5f;

        void Awake()
        {
            triggerCollider.gameObject.AddComponent<InteractionHelper>().playerInteraction = this;
            InputManager.Interaction.SetCallbacks(this);
            playerController = GetComponent<PlayerController>();
            unlockedDoorInteraction = GetComponent<PlayerUnlockedDoorInteraction>();
            playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
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

            void OnTargetReached(IInteractable interactable)
            {
                if (interactable.IsAvailableForInteraction() == false) return;

                var targetData = interactable.GetInteractionTargetData(this);
                // TODO : Consider making target data class and update it when necessary
                // target data could be changed until we reach the target
                var distance = Vector3.Distance(transform.position, targetData.targetPosition);

                if (distance > INTERACTION_DISTANCE)
                {
                    playerController.SetTarget(new InteractionData
                    {
                        targetData = targetData,
                        OnTargetReached = () => OnTargetReached(interactable),
                    });
                    return;
                }

                HandleInteraction(interactable);
            }

            if (context.performed == false || currentInteractable == null) return;

            InteractionTargetData interactionTargetData = currentInteractable.GetInteractionTargetData(this);
            var forward = transform.forward;
            var dot = Vector3.Dot(forward, -interactionTargetData.targetForwardDirection);
            var distance = Vector3.Distance(transform.position, interactionTargetData.targetPosition);
            if (distance > INTERACTION_DISTANCE || dot < 0.6f)
            {
                var interactable = currentInteractable;
                playerController.SetTarget(new InteractionData
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
            if (interactable.IsAvailableForInteraction())
            {
                ChangeCurrentInteractable(interactable);
                return;
            }

            interactables.Remove(interactable);
            if (interactable is DoorManager doorManager && doorManager.GetState().HasFlag(DoorState.Unlocked))
            {
                unlockedDoorInteraction.SetTarget(openDoorForce, doorManager.managedDoors);
            }
            ChangeCurrentInteractable(GetClosestInteractable());
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
                    unlockedDoorInteraction.SetTarget(openDoorForce, doorManager.managedDoors);
                    return;
                }
                
                interactables.Add(otherInteractable);
            }
            ChangeCurrentInteractable(GetClosestInteractable());
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
                    for (int i = 0; i < unlockedDoorInteraction.targets.Length; i++)
                    {
                        if (Array.Exists(doorManager.managedDoors, door => door == unlockedDoorInteraction.targets[i]))
                        {
                            unlockedDoorInteraction.ClearTarget();
                            return;
                        }
                    }
                }
                interactables.Remove(otherInteractable);
            }
            ChangeCurrentInteractable(GetClosestInteractable());
        }

        bool IsPointBetween(Vector3 current, Vector3 target, Vector3 point)
        {
            Vector3 line = target - current;
            Vector3 lineToPoint = point - current;
            float dot = Vector3.Dot(lineToPoint, line);
            return dot >= 0f && dot <= line.sqrMagnitude;
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
                if (dist < distance && IsAvailableForInteraction(interactableComponent))
                {
                    distance = dist;
                    closestInteractable = interactable;
                }
            }
            return closestInteractable;
        }

        bool IsAvailableForInteraction(Component other)
        {
            var currentPos = transform.position;
            // A little cheat to workaround of overlapping object problems
            var targetPos = Vector3.MoveTowards(other.transform.position, currentPos, 0.1f);

#if UNITY_EDITOR
            const float lineDuration = 8;
            Debug.DrawLine(targetPos, targetPos + Vector3.up * 5f, Color.cyan, lineDuration);
            Debug.DrawLine(currentPos, targetPos, Color.red, lineDuration);
#endif

            for (int i = 0; i < otherColliders.Count; i++)
            {
                var possibleBlockPos = otherColliders[i].transform.position;
                if (otherColliders[i] == other || IsPointBetween(currentPos, targetPos, possibleBlockPos) == false) continue;

#if UNITY_EDITOR
                Debug.Log($"{otherColliders[i]} is between {this.gameObject} and {other}");
                Debug.DrawLine(currentPos, otherColliders[i].transform.position, Color.blue, lineDuration);
#endif

                return false;
            }

            return true;
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