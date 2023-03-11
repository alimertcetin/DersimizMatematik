using System;
using LessonIsMath.DoorSystems;
using UnityEngine;

namespace LessonIsMath.InteractionSystems
{
    public class PlayerDoorInteraction : InteractionHandlerBase
    {
        [SerializeField] UnlockedDoorInteraction unlockedDoorInteraction;
        IInteractor interactor;
        DoorManager currentUnavailableDoorManager;

        void Awake()
        {
            unlockedDoorInteraction.Init(this.transform);
        }

        void Update()
        {
            unlockedDoorInteraction.Update();
        }

        public override void Init(IInteractor interactor)
        {
            this.interactor = interactor;
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
                if (unlockedDoorInteraction.HasTarget(doorManager.managedDoors[i]))
                {
                    unlockedDoorInteraction.ClearTarget();
                    return;
                }
            }
        }

        public override void OnInteractionStart(IInteractable interactable)
        {
            if (interactable is DoorManager doorManager)
            {
                currentUnavailableDoorManager = doorManager;
            }
        }

        public override void OnInteractionEnd(IInteractable interactable)
        {
            if (interactable is DoorManager doorManager && doorManager == currentUnavailableDoorManager 
                                                        && currentUnavailableDoorManager.GetState().HasFlag(DoorState.Unlocked))
            {
                unlockedDoorInteraction.SetTarget(currentUnavailableDoorManager.managedDoors);
            }
        }
    }
}