using System.Collections;
using LessonIsMath.DoorSystems;
using UnityEngine;

namespace LessonIsMath.InteractionSystems
{
    public class PlayerDoorInteraction : InteractionHandlerBase
    {
        [SerializeField] UnlockedDoorInteraction unlockedDoorInteraction;
        [SerializeField] KeycardDoorInteraction keycardDoorInteraction;
        DoorManager currentUnavailableDoorManager;

        void Awake()
        {
            unlockedDoorInteraction.Init(this.transform);
        }

        void Update()
        {
            if (unlockedDoorInteraction.hasTarget)
            {
                unlockedDoorInteraction.Update();
            }
        }

        public override void TriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out DoorManager doorManager) == false) return;
            if (doorManager.GetState().HasFlag(DoorState.Unlocked))
            {
                unlockedDoorInteraction.SetTarget(doorManager.managedDoors);
            }
        }

        public override void TriggerExit(Collider other)
        {
            if (other.TryGetComponent(out DoorManager doorManager) == false) return;
            for (int i = 0; i < doorManager.managedDoors.Length; i++)
            {
                if (unlockedDoorInteraction.IsTarget(doorManager.managedDoors[i]))
                {
                    unlockedDoorInteraction.ClearTarget();
                    return;
                }
            }
        }

        public override IEnumerator OnInteractionStart(IInteractable interactable)
        {
            if (interactable is not DoorManager doorManager) yield break;

            currentUnavailableDoorManager = doorManager;

            if (doorManager.GetState().HasFlag(DoorState.RequiresKeycard))
            {
                yield return keycardDoorInteraction.OnInteractionStart(doorManager);
            }
            
        }

        public override IEnumerator OnInteractionEnd(IInteractable interactable)
        {
            if (interactable is DoorManager doorManager && doorManager == currentUnavailableDoorManager 
                                                        && currentUnavailableDoorManager.GetState().HasFlag(DoorState.Unlocked))
            {
                unlockedDoorInteraction.SetTarget(currentUnavailableDoorManager.managedDoors);
            }

            yield return keycardDoorInteraction.OnInteractionEnd();
        }
    }
}