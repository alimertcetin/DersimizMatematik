using System;
using System.Collections.Generic;
using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LessonIsMath.InteractionSystems
{
    public class PlayerInteraction : MonoBehaviour, PlayerControls.IInteractionActions, IInteractor
    {
        [Tooltip("To define the interaction area")]
        [SerializeField] Collider triggerCollider;
        [SerializeField] StringEventChannelSO notificationChannel = default;
        HashSet<IInteractable> interactables = new HashSet<IInteractable>(8);
        List<Collider> otherColliders = new List<Collider>();
        IInteractable currentInteractable;

        void Awake()
        {
            triggerCollider.gameObject.AddComponent<InteractionHelper>().playerInteraction = this;
            InputManager.Interaction.SetCallbacks(this);
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
            if (context.performed == false || currentInteractable == null) return;

            InputManager.Interaction.Disable();
            notificationChannel.RaiseEvent("", false);
            currentInteractable.Interact(this);
        }

        void IInteractor.OnInteractionEnd(IInteractable interactable)
        {
            InputManager.Interaction.Enable();
            if (interactable.IsAvailable())
            {
                ChangeCurrentInteractable(interactable);
                return;
            }

            interactables.Remove(interactable);
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
                interactables.Add(otherInteractable);
                //if (IsAvailableForInteraction(other)) ChangeCurrentInteractable(otherInteractable);
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
            var targetPos = other.transform.position;
            // A little cheat to workaround of overlapping object problems
            var dir = (targetPos - currentPos).normalized;
            targetPos -= dir;

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
                Selection.activeGameObject = otherColliders[i].gameObject;
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