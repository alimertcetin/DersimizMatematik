using System;
using System.Collections.Generic;
using LessonIsMath.DoorSystems;
using LessonIsMath.Input;
using LessonIsMath.PlayerSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.EventSystem;
using XIV.EventSystem.Events;
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
        public InteractionPositionData PositionData;
        public Action OnTargetReached;
        public Action OnMovementCanceled;
    }
    
    // TODO : Split interactions
    public class PlayerInteraction : MonoBehaviour, PlayerControls.IInteractionActions, IInteractor
    {
        [Tooltip("To define the interaction area")]
        [SerializeField] Collider triggerCollider;
        [SerializeField] StringEventChannelSO notificationChannel = default;
        HashSet<IInteractable> interactables = new HashSet<IInteractable>(8);
        List<Collider> otherColliders = new List<Collider>(8);
        IInteractable currentInteractable;
        AutoMovementInput autoMovementInput;
        PlayerAnimationController playerAnimationController;
        InteractionHandlerBase[] interactionHandlers;

        const float INTERACTION_DISTANCE_THRESHOLD = 0.5f;
        BoxCollider interactionBoundingBox;

        void Awake()
        {
            triggerCollider.gameObject.AddComponent<InteractionHelper>().playerInteraction = this;
            InputManager.Interaction.SetCallbacks(this);
            autoMovementInput = GetComponent<AutoMovementInput>();
            interactionHandlers = GetComponentsInChildren<InteractionHandlerBase>();
            playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
            interactionBoundingBox = new GameObject("InteractionBoundingBox", typeof(BoxCollider)).GetComponent<BoxCollider>();
            interactionBoundingBox.isTrigger = true;
            
            for (int i = 0; i < interactionHandlers.Length; i++)
            {
                interactionHandlers[i].Init(this);
            }
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
                var targetData = interactable.GetInteractionPositionData(this);
                // TODO : Consider making target data class and update it when necessary
                // target data could be changed until we reach the target
                var distance = Vector3.Distance(transform.position, targetData.targetPosition);

                if (distance > INTERACTION_DISTANCE_THRESHOLD)
                {
                    autoMovementInput.SetTarget(new InteractionData
                    {
                        PositionData = targetData,
                        OnTargetReached = () => OnTargetReached(interactable),
                    });
                    return;
                }

                HandleInteraction(interactable);
            }
            
            void HandleInteraction(IInteractable interactable)
            {
                var interactionSettings = interactable.GetInteractionSettings();
                if (interactionSettings.suspendMovement) InputManager.CharacterMovement.Disable();
                if (interactionSettings.disableInteractionKey) InputManager.Interaction.Disable();
                playerAnimationController.HandleInteractionAnimation(interactable, (IInteractable interactable) =>
                {
                    notificationChannel.RaiseEvent("");
                    if (interactable == null)
                    {
                        if (interactionSettings.suspendMovement) InputManager.CharacterMovement.Enable();
                        if (interactionSettings.disableInteractionKey) InputManager.Interaction.Enable();
                        return;
                    }
                    for (int i = 0; i < interactionHandlers.Length; i++)
                    {
                        interactionHandlers[i].OnInteractionStart(currentInteractable);
                    }
                    interactable.Interact(this);
                });
            }

            if (context.performed == false || currentInteractable == null) return;
            if (currentInteractable.IsAvailableForInteraction() == false) return;
            
            InteractionPositionData interactionPositionData = currentInteractable.GetInteractionPositionData(this);
            var dot = Vector3.Dot(transform.forward, -interactionPositionData.targetForwardDirection);
            var distance = Vector3.Distance(transform.position, interactionPositionData.targetPosition);
            if (distance > INTERACTION_DISTANCE_THRESHOLD || dot < 0.6f)
            {
                var interactable = currentInteractable;
                autoMovementInput.SetTarget(new InteractionData
                {
                    PositionData = interactionPositionData,
                    OnTargetReached = () => OnTargetReached(interactable),
                });
                return;
            }
            
            HandleInteraction(currentInteractable);
        }

        void IInteractor.OnInteractionEnd(IInteractable interactable)
        {
            for (int i = 0; i < interactionHandlers.Length; i++)
            {
                interactionHandlers[i].OnInteractionEnd(interactable);
            }

            var interactionSettings = interactable.GetInteractionSettings();
            if (interactionSettings.suspendMovement) InputManager.CharacterMovement.Enable();
            if (interactionSettings.disableInteractionKey) InputManager.Interaction.Enable();
            if (interactable.IsAvailableForInteraction() && IsBlockedByAnything(interactable) == false)
            {
                ChangeCurrentInteractable(interactable);
                return;
            }

            interactables.Remove(interactable);
            RefreshCurrentInteractable();
        }

        public void TriggerEnter(Collider other)
        {
            if (other == interactionBoundingBox) return;
            if (other.isTrigger == false)
            {
                otherColliders.Add(other);
            }

            if (other.TryGetComponent<IInteractable>(out var otherInteractable))
            {
                for (int i = 0; i < interactionHandlers.Length; i++)
                {
                    interactionHandlers[i].TriggerEnter(other);
                }
                
                interactables.Add(otherInteractable);
            }

            RefreshCurrentInteractable();
        }

        public void TriggerExit(Collider other)
        {
            if (other == interactionBoundingBox) return;
            if (other.isTrigger == false)
            {
                otherColliders.Remove(other);
            }

            if (other.TryGetComponent<IInteractable>(out var otherInteractable))
            {
                for (int i = 0; i < interactionHandlers.Length; i++)
                {
                    interactionHandlers[i].TriggerExit(other);
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
                var interactionTargetData = interactable.GetInteractionPositionData(this);
                var dist = Vector3.Distance(currentPos, interactionTargetData.targetPosition);
                if (dist < distance && IsBlockedByAnything(interactable) == false)
                {
                    distance = dist;
                    closestInteractable = interactable;
                }
            }
            return closestInteractable;
        }

        bool IsBlockedByAnything(IInteractable target)
        {
            Vector3 currentPos = transform.position;
            Vector3 targetPos = target.GetInteractionPositionData(this).targetPosition;
            Vector3 center = currentPos + (targetPos - currentPos) * 0.5f;
            interactionBoundingBox.transform.position = center;
            Vector3 size = (targetPos - currentPos).Abs();
            interactionBoundingBox.size = size;

            var targetCollider = ((Component)target).GetComponent<Collider>();
            for (int i = 0; i < otherColliders.Count; i++)
            {
                var otherCollider = otherColliders[i];
                if (otherCollider == targetCollider) continue;
                
                if (Physics.ComputePenetration(interactionBoundingBox, center, interactionBoundingBox.transform.rotation,
                        otherCollider, otherCollider.transform.position, otherCollider.transform.rotation, out var dir, out var distance))
                {
#if UNITY_EDITOR
                    XIVEventSystem.CancelEvent(XIVEventSystem.GetEvent<InvokeAfterEvent>());
                    var mat = otherColliders[i].GetComponentInChildren<Renderer>().material;
                    var color = mat.color;
                    mat.color = Color.red;
                    XIVEventSystem.SendEvent(new InvokeAfterEvent(5f)
                        .OnCompleted(() => mat.color = color)
                        .OnCanceled(() => mat.color = color));
                    XIVDebug.DrawLine(center, center + (dir * distance), 8f);
                    Debug.Log($"{target} is blocked by {otherColliders[i]}");
#endif
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
            else notificationChannel.RaiseEvent("");
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