using System;
using LessonIsMath.DoorSystems;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using XIV.XIVMath;

namespace LessonIsMath.InteractionSystems
{
    public class CharacterUnlockedDoorInteraction : MonoBehaviour
    {
        public TwoBoneIKConstraint rightHandIKConstraint;
        public TwoBoneIKConstraint leftHandIKConstraint;
        public Door door;
        public Transform doorPivot;

        const float MAX_DOOR_DISTANCE = 1.5f;
        const float MAX_DOOR_ROTATION_ANGLE = 90f;
        
        public bool toggle;

        void Update()
        {
            if (toggle == false) return;

            Vector3 handlePosition = door.GetHandlePosition();
            float distance = Vector3.Distance(rightHandIKConstraint.data.tip.position, handlePosition);
            if (distance > MAX_DOOR_DISTANCE) return;

            rightHandIKConstraint.data.target.position = handlePosition;
            
            Transform root = rightHandIKConstraint.transform.root;
            Vector3 rootPosition = root.position;
            rootPosition.y = 0f;
            handlePosition.y = 0f;
            float constraintWeightMultiplier = Vector3.Dot(root.forward, (handlePosition - rootPosition).normalized);
            rightHandIKConstraint.weight = (1 - (distance / MAX_DOOR_DISTANCE)) * constraintWeightMultiplier;
            
            if (distance > 0.2f) return;

            // TODO : Bend player fingers
            float dot2 = Vector3.Dot(root.forward, doorPivot.forward);
            var axis = dot2 < 0 ? Vector3.down : Vector3.up;
            
            var doorPivotRotation = doorPivot.rotation;
            var newRotation = doorPivotRotation * Quaternion.AngleAxis(45f * Time.deltaTime, axis);
            var angle = Quaternion.Angle(door.transform.rotation, newRotation);
            if (angle > MAX_DOOR_ROTATION_ANGLE) return;
            
            doorPivot.rotation = newRotation;
        }
    }
}