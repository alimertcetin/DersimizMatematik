using System;
using UnityEngine;
using XIV.Utils;
using XIV.Easing;
using XIV.XIVMath;

namespace LessonIsMath.DoorSystems
{
    public class Door : MonoBehaviour
    {
        [SerializeField] float maxAngle;
        [SerializeField] float damping;
        [SerializeField] Transform door;
        [SerializeField] Transform doorHandle;
        [SerializeField] Timer closeDoorTimer;
        [SerializeField] Timer closeDelayTimer;
        bool rotate;
        bool close;
        Vector3 force;
        Quaternion closeStartRotation;
        Quaternion doorInitialRotation;

        void Awake()
        {
            doorInitialRotation = door.rotation;
        }

        void Update()
        {
            if (rotate)
            {
                RotateByForce();
            }
            else if (close && closeDelayTimer.Update(Time.deltaTime))
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

            this.force = force;
            rotate = true;
            close = false;
            closeDoorTimer.Restart();
            closeDelayTimer.Restart();
        }

        public void CloseDoor()
        {
            if (this.enabled == false) return;

            this.closeStartRotation = door.rotation;
            rotate = false;
            close = true;
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
                rotate = false;
                force = Vector3.zero;
                return;
            }
            door.rotation = newRotation;
        }

        void CloseByTimer()
        {
            closeDoorTimer.Update(Time.deltaTime);
            var normalizedTime = EasingFunction.SmoothStop3(closeDoorTimer.NormalizedTime);
            var handleRotationTime = 1 - XIVMathf.Remap(normalizedTime, 0.75f, 1f, 0f, 1f);
            RotateDoorHandle(handleRotationTime);
            door.rotation = Quaternion.Lerp(closeStartRotation, doorInitialRotation, normalizedTime);
            if (!closeDoorTimer.IsDone) return;
            close = false;
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