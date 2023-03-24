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
        DoorHandInteraction rightHandInteraction;
        DoorHandInteraction leftHandInteraction;
        Transform transform;
        PlayerAnimationController playerAnimationController;
        Timer updateTargetsTimer = new Timer(0.25f);
        
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

        List<Door> GetPossibleDoors(Vector3 handPosition)
        {
            float distance = float.MaxValue;
            Vector3 handPositionXZ = handPosition.OnXZ();
            Vector3 transformForward = transform.forward;
            List<Door> doors = new List<Door>(targets.Count);
            int count = 0;
            for (var i = 0; i < targets.Count; i++)
            {
                Door target = targets[i];
                Vector3 tempXZ = target.GetClosestHandlePosition(handPosition).OnXZ();
                float dist = Vector3.Distance(tempXZ, handPositionXZ);
                bool sameDirection = transformForward.IsSameDirection((tempXZ - handPositionXZ).normalized, 0.6f);
#if UNITY_EDITOR
                Vector3 temp = target.GetClosestHandlePosition(handPosition);
                XIVDebug.DrawLine(handPosition, temp, Color.red, 0.2f);
                XIVDebug.DrawTextOnLine(handPosition, temp, "Distance : " + dist, 8, Color.red);
#endif
                if (dist < distance && sameDirection)
                {
                    distance = dist;
                    doors.Add(target);
                    count++;
                }
            }

            return doors;
        }
        
        float GetScore(Door door, Vector3 handPosition)
        {
            Vector3 handlePosition = door.GetClosestHandlePosition(handPosition);
            Vector3 transformForward = transform.forward;
            // Calculate the distance between each hand and the handle
            float handDistance = Vector3.Distance(handlePosition, handPosition);
            float dot = Vector3.Dot((handlePosition - handPosition.SetY(handlePosition.y)).normalized, transformForward);
            
            // Assign weights to the distance and angle factors
            float distanceWeight = 0.6f;
            float dotWeight = 0.1f;
            
            // Calculate the score for each hand
            float score = handDistance * distanceWeight + dot * dotWeight;
            // Return true if the right hand has a higher score
            return score;
        }

        public bool IsTarget(Door door) => rightHandInteraction.IsTarget(door) || leftHandInteraction.IsTarget(door);

        public void SetTarget(params Door[] doors)
        {
            targets.AddRange(doors);
            UpdateTargets();
        }

        void UpdateTargets()
        {
            var rightLowest = GetLowestScoreDoor(rightHandIKConstraint.data.tip.position);
            var leftLowest = GetLowestScoreDoor(leftHandIKConstraint.data.tip.position);
            if (rightLowest == leftLowest)
            {
                var rightHandScore = GetScore(rightLowest, rightHandIKConstraint.data.tip.position);
                var leftHandScore = GetScore(leftLowest, leftHandIKConstraint.data.tip.position);
                if (rightHandScore < leftHandScore)
                {
                    rightHandInteraction.SetTarget(rightLowest);
                    
                    if (targets.Count > 1)
                    {
                        foreach (Door target in targets)
                        {
                            if (target != rightLowest)
                            {
                                leftHandInteraction.SetTarget(target);
                            }
                        }
                    }
                    else
                    {
                        if (leftHandInteraction.hasTarget) leftHandInteraction.ClearTarget();
                    }
                }
                else
                {
                    leftHandInteraction.SetTarget(rightLowest);
                    
                    if (targets.Count > 1)
                    {
                        foreach (Door target in targets)
                        {
                            if (target != rightLowest)
                            {
                                rightHandInteraction.SetTarget(target);
                            }
                        }
                    }
                    else
                    {
                        if (rightHandInteraction.hasTarget) rightHandInteraction.ClearTarget();
                    }
                }

            }
            else
            {
                rightHandInteraction.SetTarget(rightLowest);
                leftHandInteraction.SetTarget(leftLowest);
            }
        }

        Door GetLowestScoreDoor(Vector3 handPos)
        {
            Door door = default;
            float lowestScore = float.MaxValue;
            foreach (Door target in targets)
            {
                var score = GetScore(target, handPos);
                if (score < lowestScore)
                {
                    lowestScore = score;
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