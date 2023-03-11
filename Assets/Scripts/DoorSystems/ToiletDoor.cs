using LessonIsMath.Input;
using LessonIsMath.InteractionSystems;
using UnityEngine;
using XIV.Extensions;

namespace LessonIsMath.DoorSystems
{
    public class ToiletDoor : MonoBehaviour, IInteractable
    {
        [SerializeField] Transform door;
        [SerializeField] float maxAngle;
        [SerializeField] float doorSpeed;
        bool isOpen;
        bool open;
        bool close;
        Quaternion doorInitialRotation;
        public bool IsInInteraction => open || close;

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

        InteractionSettings IInteractable.GetInteractionSettings() => new InteractionSettings();

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

        InteractionPositionData IInteractable.GetInteractionPositionData(IInteractor interactor)
        {
            var interactorTransform = (interactor as Component).transform;
            Vector3 interactorPos = interactorTransform.position;
            return new InteractionPositionData
            {
                startPos = interactorPos,
                targetPosition = interactorPos,
                targetForwardDirection = -interactorTransform.forward,
            };
        }
    }
}