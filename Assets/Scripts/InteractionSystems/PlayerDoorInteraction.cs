using System.Collections;
using System.Collections.Generic;
using LessonIsMath.DoorSystems;
using UnityEngine;
using XIV.EventSystem;
using XIV.EventSystem.Events;
using XIV.Extensions;
using XIV.XIVMath;

namespace LessonIsMath.InteractionSystems
{
    public class PlayerDoorInteraction : InteractionHandlerBase
    {
        [SerializeField] AudioSource audioFeedbackSource;
        [SerializeField] AudioClip[] doorLockedClips;
        [SerializeField] UnlockedDoorInteraction unlockedDoorInteraction;
        [SerializeField] KeycardDoorInteraction keycardDoorInteraction;
        DoorManager currentUnavailableDoorManager;
        List<DoorManager> doorManagers = new List<DoorManager>(2);
        bool isPlayedFeedback;
        IEvent playFeedbackEvent;

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

            int doorManagerCount = doorManagers.Count;
            if (isPlayedFeedback || doorManagerCount == 0) return;
            var currentPos = transform.position;
            var currentPosXZ = currentPos.OnXZ();
            for (var i = 0; i < doorManagerCount; i++)
            {
                if (doorManagers[i].GetState().HasFlag(DoorState.Unlocked)) continue;
                
                var door = doorManagers[i].managedDoors.GetClosestOnXZPlane(currentPos);
                var distance = Vector3.Distance(door.GetClosestHandlePosition(currentPos).OnXZ(), currentPosXZ);
                if (distance > 0.5f) continue;
                isPlayedFeedback = true;
                AudioClip clip = doorLockedClips.PickRandom();
                float defaultPitch = audioFeedbackSource.pitch;
                const float min = 0.8f;
                const float max = 1.5f;
                float newPitch = Random.Range(min, max);
                audioFeedbackSource.pitch = newPitch;
                audioFeedbackSource.PlayOneShot(clip);
                playFeedbackEvent = new InvokeAfterEvent(clip.length).OnCompleted(() =>
                {
                    newPitch = XIVMathf.IsCloseToMax(newPitch, min, max) ? Random.Range(min, newPitch) : Random.Range(newPitch, max);
                    audioFeedbackSource.PlayOneShot(doorLockedClips.PickRandom());
                    audioFeedbackSource.pitch = defaultPitch;
                }).OnCanceled(() => audioFeedbackSource.pitch = defaultPitch);
                XIVEventSystem.SendEvent(playFeedbackEvent);
            }
        }

        public override void TriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out DoorManager doorManager) == false) return;
            unlockedDoorInteraction.SetTarget(doorManager.managedDoors);
            doorManagers.Add(doorManager);
        }

        public override void TriggerExit(Collider other)
        {
            if (other.TryGetComponent(out DoorManager doorManager) == false) return;
            for (int i = 0; i < doorManager.managedDoors.Length; i++)
            {
                if (unlockedDoorInteraction.IsTarget(doorManager.managedDoors[i]))
                {
                    unlockedDoorInteraction.ClearTarget();
                    break;
                }
            }
            doorManagers.Remove(doorManager);
            isPlayedFeedback = doorManagers.Count > 0;
            if (isPlayedFeedback == false && playFeedbackEvent != null)
            {
                XIVEventSystem.CancelEvent(playFeedbackEvent);
            }
        }

        public override IEnumerator OnInteractionStart(IInteractable interactable)
        {
            if (interactable is not DoorManager doorManager) yield break;

            currentUnavailableDoorManager = doorManager;

            if (unlockedDoorInteraction.hasTarget && doorManager.GetState().HasFlag(DoorState.Unlocked) == false)
            {
                unlockedDoorInteraction.ClearTarget();
            }

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