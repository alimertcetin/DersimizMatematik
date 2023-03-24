using System;
using LessonIsMath.DoorSystems;
using LessonIsMath.PlayerSystems;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using XIV;
using XIV.EventSystem;
using XIV.EventSystem.Events;
using XIV.Extensions;
using XIV.Utils;
using XIV.XIVMath;
using Random = UnityEngine.Random;

namespace LessonIsMath.InteractionSystems
{
    public struct DoorHandIKSettings
    {
        public float hintOffsetAmount;
        public float weightIncreaseSpeed;
        public float maxDoorDistance;
        public float openDoorForce;
        public float maxVelocity;

        public DoorHandIKSettings(float hintOffsetAmount, float weightIncreaseSpeed, float maxDoorDistance, float openDoorForce, float maxVelocity)
        {
            this.hintOffsetAmount = hintOffsetAmount;
            this.weightIncreaseSpeed = weightIncreaseSpeed;
            this.maxDoorDistance = maxDoorDistance;
            this.openDoorForce = openDoorForce;
            this.maxVelocity = maxVelocity;
        }
    }
    
    class DoorHandInteraction
    {
        public bool hasTarget { get; private set; }
        public Door targetDoor { get; private set; }

        Vector3 previousPosition;
        DoorHandIKSettings doorHandIKSettings;
        Transform transform;
        TwoBoneIKConstraint handIKConstraint;
        Action<float> bendHandFingersAction;
        
        float animationWeight;
        IEvent clearEvent;

        public DoorHandInteraction(Transform transform, TwoBoneIKConstraint handIKConstraint, Action<float> bendHandFingersAction, DoorHandIKSettings doorHandIKSettings)
        {
            this.transform = transform;
            this.handIKConstraint = handIKConstraint;
            this.bendHandFingersAction = bendHandFingersAction;
            this.doorHandIKSettings = doorHandIKSettings;
        }

        public void Update()
        {
            var velocity = GetVelocity();
            var transformPosition = transform.position;

            var handlePosition = targetDoor.GetClosestHandlePosition(transformPosition);
            handIKConstraint.data.target.position = handlePosition;

            Vector3 handlePosXZ = handlePosition.OnXZ();
            Vector3 transformPositionXZ = transformPosition.OnXZ();

            Vector3 transformRight = transform.right;
            var changeDelta = doorHandIKSettings.weightIncreaseSpeed * Time.deltaTime;
            handIKConstraint.weight = Mathf.MoveTowards(handIKConstraint.weight, GetWeight(handIKConstraint), changeDelta);
            var offset = transformRight * doorHandIKSettings.hintOffsetAmount;
            SetHintPosition(handIKConstraint, offset);
            animationWeight = XIVMathf.RemapClamped(handIKConstraint.weight, 0.75f, 1f, 0f, 1f);
            bendHandFingersAction.Invoke(animationWeight);

            var velocityMagnitude = velocity.magnitude;
#if UNITY_EDITOR
            XIVDebug.DrawLine(transformPosition.SetY(handlePosition.y), handlePosition, Color.green, 0.5f);
            Debug.Log("Velocity Magnitude : " + velocityMagnitude);
#endif

            if (Vector3.Distance(transformPositionXZ, handlePosXZ) > doorHandIKSettings.maxDoorDistance) return;
            if (velocityMagnitude > doorHandIKSettings.maxVelocity)
            {
                targetDoor.ApplyRotationToDoor(transform.forward * (doorHandIKSettings.openDoorForce * doorHandIKSettings.maxVelocity));
                return;
            }
            targetDoor.RotateDoorHandle(animationWeight);

            if (Vector3.Distance(handIKConstraint.data.tip.position,handlePosition) > 0.1f) return;
            
            targetDoor.ApplyRotationToDoor(transform.forward * (doorHandIKSettings.openDoorForce * (velocityMagnitude < 1 ? 1 : velocityMagnitude)));
#if UNITY_EDITOR
            Debug.Log("Velocity Magnitude : " + velocityMagnitude);
            Vector3 rndVec = Random.insideUnitSphere;
            velocity = velocity.normalized * velocityMagnitude;
            Debug.DrawLine(transform.position, transform.position + velocity, new Color(rndVec.x, rndVec.y, rndVec.z), 3f);
#endif
        }

        Vector3 GetVelocity()
        {
            var currentPos = transform.position;
            var velocity = (currentPos - previousPosition) / Time.deltaTime;
            previousPosition = currentPos;
            return velocity;
        }

        float GetWeight(TwoBoneIKConstraint handIK)
        {
            Vector3 transformRootPosXZ = transform.position.OnXZ();
            Vector3 ikTargetPos = handIK.data.target.position;
            Vector3 ikTargetPosXZ = ikTargetPos.OnXZ();

            float tipToTargetDistance = Vector3.Distance(handIK.data.tip.position, ikTargetPos);
            float weightMultiplier = Vector3.Dot(transform.forward, (ikTargetPosXZ - transformRootPosXZ).normalized);
            if (weightMultiplier < 0) return 0f;
            weightMultiplier *= 1.5f;
            float weight = (1 - (tipToTargetDistance / doorHandIKSettings.maxDoorDistance)) * weightMultiplier;
            return Mathf.Clamp01(weight);
        }

        static void SetHintPosition(TwoBoneIKConstraint handIK, Vector3 offset)
        {
            Vector3 ikTipPos = handIK.data.tip.position;
            Vector3 ikMidPos = handIK.data.mid.position;
            Vector3 mid1 = ikMidPos;
            mid1 += offset;
            Vector3 mid2 = ikTipPos;
            mid2 += offset;

            Vector3 hintPos = BezierMath.GetPoint(ikMidPos, mid1, mid2, ikTipPos, 0.65f);
            handIK.data.hint.position = hintPos;
#if UNITY_EDITOR
            XIVDebug.DrawBezier(ikMidPos, mid1, mid2, ikTipPos, Color.blue);
            XIVDebug.DrawSphere(hintPos, 0.1f, Color.green);
#endif
        }

        public bool IsTarget(Door door) => door == targetDoor;

        public void SetTarget(Door door)
        {
            this.targetDoor = door;
            hasTarget = door != null;
            previousPosition = transform.position;
        }

        public void ClearTarget()
        {
            if (clearEvent != null) return;
            
            targetDoor.CloseDoor();
            hasTarget = false;
            targetDoor = null;
            
            Vector3 transformRight = transform.right;
            float rightHandWeight = handIKConstraint.weight;
            float animWeight = animationWeight;
            clearEvent = new InvokeForSecondsEvent(1f).AddAction((Timer timer) =>
            {
                float normalizedTime = timer.NormalizedTime;
                handIKConstraint.weight = Mathf.Lerp(rightHandWeight, 0f, normalizedTime);
                SetHintPosition(handIKConstraint, transformRight * doorHandIKSettings.hintOffsetAmount);
                animationWeight = Mathf.Lerp(animWeight, 0f, normalizedTime);
                bendHandFingersAction.Invoke(animationWeight);
            })
                .AddCancelCondition(() => hasTarget)
                .OnCanceled(() => clearEvent = null)
                .OnCompleted(() => clearEvent = null);
            XIVEventSystem.SendEvent(clearEvent);
        }
    }
}