using System;
using LessonIsMath.DoorSystems;
using LessonIsMath.PlayerSystems;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using XIV;
using XIV.EventSystem;
using XIV.Extensions;
using XIV.Utils;
using XIV.XIVMath;
using Random = UnityEngine.Random;

namespace LessonIsMath.InteractionSystems
{
    public class PlayerUnlockedDoorInteraction : MonoBehaviour
    {
        [SerializeField] TwoBoneIKConstraint rightHandIKConstraint;
        [SerializeField] TwoBoneIKConstraint leftHandIKConstraint;
        [SerializeField] float hintOffsetAmount = 0.25f;
        [SerializeField] float weightIncreaseSpeed = 2f;
        [SerializeField] float maxDoorDistance = 2f;
        
        bool hasTarget;
        float openDoorForce; // = 25f;
        float animationWeight;
        Vector3 previousPosition;
        [HideInInspector] public Door[] targets;
        PlayerAnimationController playerAnimationController;

        const float MAX_VELOCITY = 3F;
        
        void Start()
        {
            playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
        }

        void Update()
        {
            var velocity = CalculateVelocity();
            if (hasTarget == false) return;
            var transformPosition = transform.position;
            
            var door = targets.GetClosestOnXZPlane(transformPosition);
            Vector3 handlePosition = door.GetHandlePosition();
            rightHandIKConstraint.data.target.position = handlePosition;
            leftHandIKConstraint.data.target.position = handlePosition;

            Vector3 handlePosXZ = handlePosition.OnXZ();
            Vector3 transformPositionXZ = transformPosition.OnXZ();
            if (Vector3.Distance(transformPositionXZ, handlePosXZ) > maxDoorDistance) return;

            Vector3 transformRight = transform.right;
            bool isRightDominant = transformRight.IsSameDirection((handlePosXZ - transformPositionXZ).normalized);

            TwoBoneIKConstraint dominantIK = GetDominant(isRightDominant, out var other);
            SetIKWeights(dominantIK, other);
            var offset = transformRight * hintOffsetAmount;
            SetHintPosition(rightHandIKConstraint, offset);
            SetHintPosition(leftHandIKConstraint, -offset);
            animationWeight = XIVMathf.Remap(dominantIK.weight, 0.75f, 1f, 0f, 1f);
            HandleAnimation(isRightDominant, animationWeight);
            door.RotateDoorHandle(animationWeight);

            if (Vector3.Distance(dominantIK.data.tip.position,handlePosition) > 0.1f) return;
            
            var velocityMagnitude = velocity.magnitude;
            velocityMagnitude = Mathf.Clamp(velocityMagnitude, 0, MAX_VELOCITY);
            door.ApplyRotationToDoor(transform.forward * (openDoorForce * (velocityMagnitude < 1 ? 1 : velocityMagnitude)));
#if UNITY_EDITOR
            Debug.Log("Velocity Magnitude : " + velocityMagnitude);
            Vector3 rndVec = Random.insideUnitSphere;
            velocity = velocity.normalized * velocityMagnitude;
            Debug.DrawLine(transform.position, transform.position + velocity, new Color(rndVec.x, rndVec.y, rndVec.z), 3f);
#endif
        }
            
        void SetIKWeights(TwoBoneIKConstraint ik, TwoBoneIKConstraint otherIk)
        {
            var changeDelta = weightIncreaseSpeed * Time.deltaTime;
            ik.weight = Mathf.MoveTowards(ik.weight, GetWeight(ik), changeDelta);
            otherIk.weight = Mathf.MoveTowards(otherIk.weight, 0f, changeDelta);
        }
        
        TwoBoneIKConstraint GetDominant(bool isRightDominant, out TwoBoneIKConstraint other)
        {
            if (isRightDominant)
            {
                other = leftHandIKConstraint;
                return rightHandIKConstraint;
            }
            other = rightHandIKConstraint;
            return leftHandIKConstraint;
        }

        void HandleAnimation(bool isRightDominant, float weight)
        {
            if (isRightDominant) playerAnimationController.BendRightHandFingers(weight);
            else playerAnimationController.BendLeftHandFingers(weight);
        }

        Vector3 CalculateVelocity()
        {
            var currentPos = transform.position;
            var velocity = (currentPos - previousPosition) / Time.deltaTime;
            previousPosition = currentPos;
            return velocity;
        }

        float GetWeight(TwoBoneIKConstraint handIK)
        {
            Vector3 transformRootPosXZ = transform.position;
            transformRootPosXZ.y = 0f;
            Vector3 ikTargetPos = handIK.data.target.position;
            Vector3 ikTargetPosXZ = ikTargetPos;
            ikTargetPosXZ.y = 0f;

            float tipToTargetDistance = Vector3.Distance(handIK.data.tip.position, ikTargetPos);
            float weightMultiplier = Vector3.Dot(transform.forward, (ikTargetPosXZ - transformRootPosXZ).normalized);
            weightMultiplier *= 1.2f;
            float weight = (1 - (tipToTargetDistance / maxDoorDistance)) * weightMultiplier;
            return Mathf.Clamp01(weight);
        }

        static void SetHintPosition(TwoBoneIKConstraint handIK, Vector3 offset)
        {
            Vector3 ikTipPos = handIK.data.tip.position;
            Vector3 ikMidPos = handIK.data.mid.position;
            Vector3 mid2 = ikTipPos;
            mid2 += offset;
            Vector3 mid1 = ikMidPos;
            mid1 += offset;

            Vector3 hintPos = BezierMath.GetPoint(ikMidPos, mid1, mid2, ikTipPos, 0.65f);
            handIK.data.hint.position = hintPos;
#if UNITY_EDITOR
            XIVDebug.DrawBezier(ikMidPos, mid1, mid2, ikTipPos, Color.blue);
            XIVDebug.DrawSphere(hintPos, 0.1f, Color.green);
#endif
        }

        public void SetTarget(float forceToApply, params Door[] doors)
        {
            hasTarget = true;
            this.targets = doors;
            this.openDoorForce = forceToApply;
        }

        public void ClearTarget()
        {
            for (int i = 0; i < this.targets.Length; i++)
            {
                targets[i].CloseDoor();
            }
            hasTarget = false;
            this.targets = Array.Empty<Door>();
            
            Vector3 transformRight = transform.right;
            XIVEventSystem.SendEvent(new XIVInvokeUntilEvent(1f).AddAction((Timer timer) =>
            {
                float normalizedTime = timer.NormalizedTime;
                rightHandIKConstraint.weight = Mathf.Lerp(rightHandIKConstraint.weight, 0f, normalizedTime);
                leftHandIKConstraint.weight = Mathf.Lerp(leftHandIKConstraint.weight, 0f, normalizedTime);
                SetHintPosition(rightHandIKConstraint, transformRight * hintOffsetAmount);
                SetHintPosition(leftHandIKConstraint, -(transformRight * hintOffsetAmount));
                animationWeight = Mathf.Lerp(animationWeight, 0f, normalizedTime);
                playerAnimationController.BendRightHandFingers(animationWeight);
                playerAnimationController.BendLeftHandFingers(animationWeight);
            }).AddCancelCondition(() => hasTarget));
        }
        
    }
}