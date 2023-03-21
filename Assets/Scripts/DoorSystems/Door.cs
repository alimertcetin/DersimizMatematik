using System;
using UnityEngine;
using XIV.Utils;
using XIV.Easing;
using XIV.Extensions;
using XIV.XIVMath;

namespace LessonIsMath.DoorSystems
{
    // TODO : Needs cleanup
    // Audio stuff and booleans...
    public class Door : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField, Range(-3f, 3f)] float doorOpenSoundPitch;
        [SerializeField, Range(-3f, 3f)] float doorCloseSoundPitch;
        [SerializeField] AudioClip[] doorOpenClips;
        [SerializeField] AudioClip[] doorCloseClips;
        [SerializeField] float maxAngle;
        [SerializeField] float damping;
        [SerializeField] Transform door;
        [SerializeField] Transform doorHandle;
        [SerializeField] Timer closeDoorTimer;
        [SerializeField] Timer closeDelayTimer;
        bool openDoorFlag;
        bool closeDoorFlag;
        Vector3 force;
        Quaternion closeStartRotation;
        Quaternion doorInitialRotation;
        bool isPlayedCloseSound;
        bool isOpen;

        void Awake()
        {
            doorInitialRotation = door.rotation;
        }

        void Update()
        {
            if (openDoorFlag)
            {
                RotateByForce();
            }
            else if (closeDoorFlag && closeDelayTimer.Update(Time.deltaTime))
            {
                CloseByTimer();
            }
        }

        public void RotateDoorHandle(float t)
        {
            if (this.enabled == false) return;
            
            var angle = Mathf.Lerp(0, -30f, t);
            var newRotation = Quaternion.Euler(0, 0, angle);
            doorHandle.localRotation = newRotation;
        }

        public void ApplyRotationToDoor(Vector3 force)
        {
            if (this.enabled == false) return;

            if (isOpen == false)
            {
                audioSource.pitch = doorOpenSoundPitch;
                audioSource.PlayOneShot(doorOpenClips.PickRandom());
            }
            if (openDoorFlag)
            {
                this.force = force;
                return;
            }
            openDoorFlag = true;
            isOpen = true;
            this.force = force;
            closeDoorFlag = false;
            closeDoorTimer.Restart();
            closeDelayTimer.Restart();
        }

        public void CloseDoor()
        {
            if (this.enabled == false || isOpen == false) return;

            this.closeStartRotation = door.rotation;
            openDoorFlag = false;
            closeDoorFlag = true;
        }
        
        public Vector3 GetHandlePosition() => doorHandle.position;

        void RotateByForce()
        {
            force -= force * (damping * Time.deltaTime);
            var currentRotation = door.rotation;
            var newRotation = currentRotation * Quaternion.AngleAxis(force.magnitude * Time.deltaTime, GetAxis());
            var angle = Quaternion.Angle(doorInitialRotation, newRotation);
            if (angle > maxAngle)
            {
                force = Vector3.zero;
                return;
            }
            door.rotation = newRotation;
        }

        void CloseByTimer()
        {
            closeDoorTimer.Update(Time.deltaTime);
            var normalizedTime = EasingFunction.SmoothStop3(closeDoorTimer.NormalizedTime);
            var handleRotationTime = 1 - XIVMathf.RemapClamped(normalizedTime, 0.75f, 1f, 0f, 1f);
            RotateDoorHandle(handleRotationTime);
            door.rotation = Quaternion.Lerp(closeStartRotation, doorInitialRotation, normalizedTime);
            if (normalizedTime > 0.8f && isPlayedCloseSound == false)
            {
                isPlayedCloseSound = true;
                audioSource.pitch = doorCloseSoundPitch;
                audioSource.PlayOneShot(doorCloseClips.PickRandom());
            }
            
            if (!closeDoorTimer.IsDone) return;
            isPlayedCloseSound = false;
            closeDoorFlag = false;
            isOpen = false;
            closeDoorTimer.Restart();
            closeDelayTimer.Restart();
        }

        Vector3 GetAxis()
        {
            float dot = Vector3.Dot(force.normalized, door.forward);
            var axis = dot < 0 ? Vector3.down : Vector3.up;
            return axis;
        }
    }
}