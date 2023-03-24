using System.Collections.Generic;
using LessonIsMath.DoorSystems;
using LessonIsMath.PlayerSystems;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using XIV;
using XIV.Extensions;
using XIV.Utils;

namespace LessonIsMath.InteractionSystems
{
    [System.Serializable]
    public class UnlockedDoorInteraction
    {
        [SerializeField] TwoBoneIKConstraint rightHandIKConstraint;
        [SerializeField] TwoBoneIKConstraint leftHandIKConstraint;
        [SerializeField] float hintOffsetAmount = 0.25f;
        [SerializeField] float weightIncreaseSpeed = 2f;
        [SerializeField] float maxDoorDistance = 2f;
        [SerializeField] float openDoorForce = 50f;
        List<Door> targets = new List<Door>(2);

        public bool hasTarget => rightHandInteraction.hasTarget || leftHandInteraction.hasTarget;

        const float MAX_VELOCITY = 6F;
        const float DISTANCE_COST = 0.6f;
        const float ANGLE_COST = 0.1f;
        DoorHandInteraction rightHandInteraction;
        DoorHandInteraction leftHandInteraction;
        Transform transform;
        PlayerAnimationController playerAnimationController;
        Timer updateTargetsTimer = new Timer(0.5f);
        
        public void Init(Transform transform)
        {
            this.transform = transform;
            playerAnimationController = transform.GetComponentInChildren<PlayerAnimationController>();
            var doorHandIKSettings = new DoorHandIKSettings(hintOffsetAmount, weightIncreaseSpeed, maxDoorDistance, openDoorForce, MAX_VELOCITY);
            rightHandInteraction = new DoorHandInteraction(transform, rightHandIKConstraint, playerAnimationController.BendRightHandFingers, doorHandIKSettings);
            leftHandInteraction = new DoorHandInteraction(transform, leftHandIKConstraint, playerAnimationController.BendLeftHandFingers, doorHandIKSettings);
        }

        public void Update()
        {
            if (updateTargetsTimer.Update(Time.deltaTime))
            {
                UpdateTargets();
                updateTargetsTimer.Restart();
            }

            if (rightHandInteraction.hasTarget) rightHandInteraction.Update();
            if (leftHandInteraction.hasTarget) leftHandInteraction.Update();
        }

        public bool IsTarget(Door door) => rightHandInteraction.IsTarget(door) || leftHandInteraction.IsTarget(door);

        public void SetTarget(params Door[] doors)
        {
            targets.AddRange(doors);
            UpdateTargets();
        }
        
        float GetCost(Door door, Vector3 handPosition)
        {
            Vector3 handlePosition = door.GetClosestHandlePosition(handPosition);
            Vector3 transformForward = transform.forward;
            // Calculate the distance between each hand and the handle
            float handDistance = Vector3.Distance(handlePosition, handPosition);
            float dot = Vector3.Dot((handlePosition - handPosition.SetY(handlePosition.y)).normalized, transformForward);
            
            // Calculate the cost for each hand
            float cost = handDistance * DISTANCE_COST + dot * ANGLE_COST;
            return cost;
        }

        void UpdateTargets()
        {
            void SetTarget(DoorHandInteraction current, DoorHandInteraction other, Door door)
            {
                current.SetTarget(door);
                if (other.hasTarget) other.ClearTarget();
                for (var i = 0; i < targets.Count; i++)
                {
                    Door target = targets[i];
                    if (target != door)
                    {
                        other.SetTarget(target);
                    }
                }
            }
            
            var rightLowest = GetLowestCost(rightHandIKConstraint.data.tip.position, out var rightScore);
            var leftLowest = GetLowestCost(leftHandIKConstraint.data.tip.position, out var leftScore);
            if (rightLowest == leftLowest)
            {
                if (rightScore < leftScore)
                {
                    SetTarget(rightHandInteraction, leftHandInteraction, rightLowest);
                }
                else
                {
                    SetTarget(leftHandInteraction, rightHandInteraction, rightLowest);
                }

            }
            else
            {
                rightHandInteraction.SetTarget(rightLowest);
                leftHandInteraction.SetTarget(leftLowest);
            }
        }

        Door GetLowestCost(Vector3 handPos, out float cost)
        {
            Door door = default;
            cost = float.MaxValue;
            for (var i = 0; i < targets.Count; i++)
            {
                Door target = targets[i];
                var tempCost = GetCost(target, handPos);
                if (tempCost < cost)
                {
                    cost = tempCost;
                    door = target;
                }
            }

            return door;
        }

        public void ClearTarget()
        {
            if (rightHandInteraction.hasTarget) rightHandInteraction.ClearTarget();
            if (leftHandInteraction.hasTarget) leftHandInteraction.ClearTarget();
            targets.Clear();
        }
    }
}