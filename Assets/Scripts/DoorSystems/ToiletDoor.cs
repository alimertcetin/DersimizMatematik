using System;
using LessonIsMath.Input;
using LessonIsMath.InteractionSystems;
using UnityEngine;
using XIV.Easing;
using XIV.Extensions;
using XIV.Utils;
using XIV.XIVMath;

namespace LessonIsMath.DoorSystems
{
    public class ToiletDoor : MonoBehaviour, IInteractable
    {
        [SerializeField] Transform[] interactionPositions;
        [SerializeField] Transform door;
        [SerializeField] float maxAngle;
        [SerializeField] float doorSpeed;
        bool isOpen;
        bool open;
        bool close;
        Quaternion doorInitialRotation;

        void Awake()
        {
            doorInitialRotation = door.rotation;
        }

        void Update()
        {
            if (open)
            {
                var doorOpenRotation = doorInitialRotation * Quaternion.Euler(0f, maxAngle, 0f);
                var doorCurrentRotation = door.rotation;
                var doorNewRotation = Quaternion.RotateTowards(doorCurrentRotation, doorOpenRotation, doorSpeed * Time.deltaTime);
                var newAngle = Quaternion.Angle(doorNewRotation, doorOpenRotation);
                if (newAngle < Mathf.Epsilon)
                {
                    open = false;
                    return;
                }
                door.rotation = doorNewRotation;
            }
            else if (close)
            {
                var doorCurrentRotation = door.rotation;
                var doorNewRotation = Quaternion.RotateTowards(doorCurrentRotation, doorInitialRotation, doorSpeed * Time.deltaTime);
                var newAngle = Quaternion.Angle(doorNewRotation, doorInitialRotation);
                if (newAngle < Mathf.Epsilon)
                {
                    close = false;
                    return;
                }
                door.rotation = doorNewRotation;
            }
        }

        bool IInteractable.IsAvailableForInteraction() => true;

        void IInteractable.Interact(IInteractor interactor)
        {
            if (isOpen)
            {
                close = true;
                open = false;
            }
            else
            {
                close = false;
                open = true;
            }
            isOpen = !isOpen;
            interactor.OnInteractionEnd(this);
        }

        string IInteractable.GetInteractionString()
        {
            var str = isOpen ? "Close" : "Open";
            return $"Press {InputManager.InteractionKeyName} to {str}";
        }

        InteractionTargetData IInteractable.GetInteractionTargetData(IInteractor interactor)
        {
            Vector3 interactorPos = (interactor as Component).transform.position;
            Transform interactionPos = interactionPositions.GetClosestOnXZPlane(interactorPos);
            return new InteractionTargetData
            {
                startPos = interactorPos,
                targetPosition = interactionPos.position,
                targetForwardDirection = interactionPos.forward,
            };
        }
    }
}