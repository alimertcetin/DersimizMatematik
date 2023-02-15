using System;
using System.Collections.Generic;
using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;

namespace LessonIsMath.InteractionSystems
{
    public class PlayerInteraction : MonoBehaviour, IInteractor
    {
        [Tooltip("To define the interaction area")]
        [SerializeField] Collider triggerCollider;
        [SerializeField] StringEventChannelSO notificationChannel = default;
        List<IInteractable> interactables = new List<IInteractable>(8);
        IInteractable currentInteractable;

        void Awake()
        {
            triggerCollider.gameObject.AddComponent<InteractionHelper>().playerInteraction = this;
        }

        void OnEnable()
        {
            InputManager.PlayerControls.Gameplay.Interact.performed += Interact_performed;
        }

        void OnDisable()
        {
            InputManager.PlayerControls.Gameplay.Interact.performed -= Interact_performed;
        }

        public void TriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var otherInteractable) == false) return;

            if (interactables.Contains(otherInteractable) == false) interactables.Add(otherInteractable);
            currentInteractable = otherInteractable;
            notificationChannel.RaiseEvent(currentInteractable.GetInteractionString());
        }

        public void TriggerExit(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var otherInteractable) == false) return;

            interactables.Remove(otherInteractable);
            currentInteractable = GetClosestInteractable();
            if (currentInteractable != null) notificationChannel.RaiseEvent(currentInteractable.GetInteractionString());
            else notificationChannel.RaiseEvent("", false);
        }

        IInteractable GetClosestInteractable()
        {
            float distance = float.MaxValue;
            IInteractable closestInteractable = default;
            for (int i = 0; i < interactables.Count; i++)
            {
                IInteractable interactable = interactables[i];
                Component interactableComponent = interactable as Component;
                var dist = Vector3.Distance(this.transform.position, interactableComponent.transform.position);
                if (dist < distance)
                {
                    distance= dist;
                    closestInteractable = interactable;
                }
            }
            return closestInteractable;
        }

        private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (currentInteractable == null) return;

            notificationChannel.RaiseEvent("", false);
            currentInteractable.Interact(this);
        }

        void IInteractor.OnInteractionEnd(IInteractable interactable)
        {
            if (interactable.CanInteract()) currentInteractable = interactable;
            else currentInteractable = GetClosestInteractable();

            if (currentInteractable != null) notificationChannel.RaiseEvent(currentInteractable.GetInteractionString());
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