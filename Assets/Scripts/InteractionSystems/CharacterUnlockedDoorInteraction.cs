using LessonIsMath.DoorSystems;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using XIV;
using XIV.Easing;
using XIV.EventSystem;
using XIV.Utils;
using XIV.XIVMath;

namespace LessonIsMath.InteractionSystems
{
    public class CharacterUnlockedDoorInteraction : MonoBehaviour
    {
        public TwoBoneIKConstraint rightHandIKConstraint;
        public TwoBoneIKConstraint leftHandIKConstraint;
        public Door door;
        public Transform doorPivot;

        const float MAX_DOOR_DISTANCE = 2f;
        const float MAX_DOOR_ROTATION_ANGLE = 90f;
        const float LERP_SPEED = 2f;
        const float DISTANCE_MULTIPLIER = 0.25f;
        
        public bool toggle;
        bool canSendEvent;
        Vector3 lastRotationAxis;
        float openDoorForce;
        Vector3 previousPos;

        void Start()
        {
            previousPos = transform.root.position;
        }

        void Update()
        {
            var currentPos = transform.root.position;
            openDoorForce = ((previousPos - currentPos).magnitude / Time.deltaTime) * 50f;
            previousPos = currentPos;
            if (toggle == false) return;
            
            Vector3 handlePosition = door.GetHandlePosition();
            rightHandIKConstraint.data.target.position = handlePosition;
            leftHandIKConstraint.data.target.position = handlePosition;
            
            var root = rightHandIKConstraint.transform.root; // Assume both has the same root
            var rootRight = root.right;

            var handlePosXZ = handlePosition;
            handlePosXZ.y = 0f;
            var rootPosXZ = root.position;
            rootPosXZ.y = 0f;
            var distanceToHandle = Vector3.Distance(handlePosXZ, rootPosXZ);
            
            float deltaTime = Time.deltaTime;
            if (distanceToHandle > MAX_DOOR_DISTANCE)
            {
                rightHandIKConstraint.weight = Mathf.MoveTowards(rightHandIKConstraint.weight, 0f, LERP_SPEED * deltaTime);
                leftHandIKConstraint.weight = Mathf.MoveTowards(leftHandIKConstraint.weight, 0f, LERP_SPEED * deltaTime);
                SetHintPosition(rightHandIKConstraint, rootRight * DISTANCE_MULTIPLIER);
                SetHintPosition(leftHandIKConstraint, -(rootRight * DISTANCE_MULTIPLIER));
                if (canSendEvent) SendPushDoorEvent();
                canSendEvent = false;
                return;
            }
            var isHandleCloseToRight = Vector3.Dot(rootRight, (handlePosXZ - rootPosXZ).normalized) > 0f;

            float tipToHandleDistance = 0f;  
            if (isHandleCloseToRight)
            {
                float rightIKWeight = GetWeight(rightHandIKConstraint);
                rightHandIKConstraint.weight = Mathf.MoveTowards(rightHandIKConstraint.weight, rightIKWeight, LERP_SPEED * deltaTime);
                leftHandIKConstraint.weight = Mathf.MoveTowards(leftHandIKConstraint.weight, 0f, LERP_SPEED * deltaTime);
                tipToHandleDistance = Vector3.Distance(rightHandIKConstraint.data.tip.position, handlePosition);
            }
            else
            {
                float leftIKWeight = GetWeight(leftHandIKConstraint);
                rightHandIKConstraint.weight = Mathf.MoveTowards(rightHandIKConstraint.weight, 0f, LERP_SPEED * deltaTime);
                leftHandIKConstraint.weight = Mathf.MoveTowards(leftHandIKConstraint.weight, leftIKWeight, LERP_SPEED * deltaTime);
                tipToHandleDistance = Vector3.Distance(leftHandIKConstraint.data.tip.position, handlePosition);
            }
            
            SetHintPosition(rightHandIKConstraint, rootRight * DISTANCE_MULTIPLIER);
            SetHintPosition(leftHandIKConstraint, -(rootRight * DISTANCE_MULTIPLIER));

            if (tipToHandleDistance < 0.25f)
            {
                lastRotationAxis = GetAxis();
                ApplyRotationToDoor(openDoorForce * deltaTime, lastRotationAxis);
                canSendEvent = true;
            }
        }

        void ApplyRotationToDoor(float rotationAmount, Vector3 axis)
        {
            var currentRotation = doorPivot.rotation;
            var newRotation = currentRotation * Quaternion.AngleAxis(rotationAmount, axis);
            var angle = Quaternion.Angle(door.transform.rotation, newRotation);
            if (angle > MAX_DOOR_ROTATION_ANGLE) return;
            doorPivot.rotation = newRotation;
        }

        Vector3 GetAxis()
        {
            Transform root = rightHandIKConstraint.transform.root; // Assume both (right and left ik) has the same root
            float dot = Vector3.Dot(root.forward, doorPivot.forward);
            var axis = dot < 0 ? Vector3.down : Vector3.up;
            return axis;
        }

        void SendPushDoorEvent()
        {
            var force = openDoorForce * 0.15f;
            XIVEventSystem.SendEvent(new XIVInvokeUntilEvent(2f, (Timer timer) =>
            {
                ApplyRotationToDoor(force * (1 - timer.NormalizedTime) * Time.deltaTime, lastRotationAxis);
            }));
            var invokeLaterEvent = new XIVTimedEvent(5f);
            invokeLaterEvent.OnCompleted = () =>
            {
                if (canSendEvent) return;
                var initialRotation = doorPivot.rotation;
                XIVEventSystem.SendEvent(new XIVInvokeUntilEvent(2.5f, (Timer timer) =>
                {
                    if (canSendEvent) return; // TODO : Cancel event
                    var normalizedTime = EasingFunction.SmoothStop3(timer.NormalizedTime);
                    doorPivot.rotation = Quaternion.Lerp(initialRotation, door.transform.rotation, normalizedTime);
                }));
            };
            XIVEventSystem.SendEvent(invokeLaterEvent);
        }

        float GetWeight(TwoBoneIKConstraint handIK)
        {
            Transform root = handIK.transform.root;
            
            Vector3 transformRootPosXZ = root.position;
            transformRootPosXZ.y = 0f;
            Vector3 ikTargetPos = handIK.data.target.position;
            Vector3 ikTargetPosXZ = ikTargetPos;
            ikTargetPosXZ.y = 0f;

            float tipToTargetDistance = Vector3.Distance(handIK.data.tip.position, ikTargetPos);
            float weightMultiplier = Vector3.Dot(root.forward, (ikTargetPosXZ - transformRootPosXZ).normalized);
            weightMultiplier *= 1.2f;
            float weight = (1 - (tipToTargetDistance / MAX_DOOR_DISTANCE)) * weightMultiplier;
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
            XIVDebug.DrawBezier(ikMidPos, mid1, mid2, ikTipPos, Color.blue);
            XIVDebug.DrawSphere(hintPos, 0.1f, Color.green);
        }
    }
}